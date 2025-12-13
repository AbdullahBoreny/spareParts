CREATE PROCEDURE [dbo].[LookupScript] (@LookupJson NVARCHAR(MAX))
AS

BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @TableName SYSNAME,
        @SchemaName SYSNAME,
        @PureTable SYSNAME,
        @KeyColumn SYSNAME,
        @ColumnList NVARCHAR(MAX),
        @UpdateList NVARCHAR(MAX),
        @WithClause NVARCHAR(MAX),
        @Sql NVARCHAR(MAX);

    -- Read table name from JSON
    SELECT @TableName = JSON_VALUE(@LookupJson, '$.table');

    -- Parse schema.table
    SET @SchemaName = PARSENAME(@TableName, 2);
    SET @PureTable  = PARSENAME(@TableName, 1);
    IF @SchemaName IS NULL SET @SchemaName = 'dbo';

    -- Detect primary key column
    SELECT TOP 1
        @KeyColumn = c.name
    FROM sys.indexes i
    INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
    INNER JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
    WHERE i.object_id = OBJECT_ID(QUOTENAME(@SchemaName) + '.' + QUOTENAME(@PureTable))
      AND i.is_primary_key = 1;

    IF @KeyColumn IS NULL
        THROW 50001, 'Table does not have a primary key', 1;

    -- Build column list from table (exclude identity columns)
    SELECT
        @ColumnList = STRING_AGG(QUOTENAME(c.name), ','),
        @UpdateList = STRING_AGG(
            'target.' + QUOTENAME(c.name) + ' = source.' + QUOTENAME(c.name),
            ', '
        ),
        @WithClause = STRING_AGG(
            QUOTENAME(c.name) + ' ' +
            TYPE_NAME(c.user_type_id) +
            CASE 
                WHEN c.max_length = -1 THEN '(MAX)'
                WHEN c.user_type_id IN (167, 231) THEN '(' + CAST(c.max_length AS VARCHAR) + ')'
                WHEN c.user_type_id IN (106,108) THEN '(' + CAST(c.precision AS VARCHAR) + ',' + CAST(c.scale AS VARCHAR) + ')'
                ELSE ''
            END +
            ' ''$.' + c.name + '''',
            ',' + CHAR(10)
        )
    FROM sys.columns c
    WHERE c.object_id = OBJECT_ID(QUOTENAME(@SchemaName) + '.' + QUOTENAME(@PureTable))
      AND c.is_identity = 0;

    -- Remove key column from update list
    SET @UpdateList = REPLACE(
        @UpdateList,
        'target.' + QUOTENAME(@KeyColumn) + ' = source.' + QUOTENAME(@KeyColumn) + ', ',
        ''
    );

    -- Build dynamic MERGE
    SET @Sql = N'
    MERGE ' + QUOTENAME(@SchemaName) + '.' + QUOTENAME(@PureTable) + ' AS target
    USING (
        SELECT *
        FROM OPENJSON(@json, ''$.records'')
        WITH (
            ' + @WithClause + '
        )
    ) AS source
    ON target.' + QUOTENAME(@KeyColumn) + ' = source.' + QUOTENAME(@KeyColumn) + '
    WHEN MATCHED THEN
        UPDATE SET ' + @UpdateList + '
    WHEN NOT MATCHED THEN
        INSERT (' + @ColumnList + ')
        VALUES (' + @ColumnList + ');';

    EXEC sp_executesql
        @Sql,
        N'@json NVARCHAR(MAX)',
        @json = @LookupJson;
END