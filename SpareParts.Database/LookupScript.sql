CREATE PROCEDURE [dbo].[LookupScript] (@LookupJson NVARCHAR(MAX))
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @TableName SYSNAME,
        @SchemaName SYSNAME,
        @PureTable SYSNAME,
        @ColumnList NVARCHAR(MAX),
        @UpdateList NVARCHAR(MAX),
        @WithClause NVARCHAR(MAX),
        @JoinClause NVARCHAR(MAX),
        @Sql NVARCHAR(MAX);

    SELECT @TableName = JSON_VALUE(@LookupJson, '$.table');

    SET @SchemaName = PARSENAME(@TableName, 2);
    SET @PureTable  = PARSENAME(@TableName, 1);
    IF @SchemaName IS NULL SET @SchemaName = 'dbo';

    SELECT 
        @JoinClause = STRING_AGG('target.' + QUOTENAME(c.name) + ' = source.' + QUOTENAME(c.name), ' AND '),
        @UpdateList = STRING_AGG(
            CASE WHEN ic.column_id IS NULL THEN 'target.' + QUOTENAME(c.name) + ' = source.' + QUOTENAME(c.name) END, 
            ', '
        ),
        @ColumnList = STRING_AGG(QUOTENAME(c.name), ','),
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
    LEFT JOIN sys.index_columns ic 
        ON ic.object_id = c.object_id 
        AND ic.column_id = c.column_id
        AND EXISTS (SELECT 1 FROM sys.indexes i WHERE i.object_id = ic.object_id AND i.index_id = ic.index_id AND i.is_primary_key = 1)
    WHERE c.object_id = OBJECT_ID(QUOTENAME(@SchemaName) + '.' + QUOTENAME(@PureTable))
      AND c.is_identity = 0;

    SET @UpdateList = REPLACE(REPLACE(@UpdateList, ', ,', ','), ' ,', '');

    IF LEFT(@UpdateList, 1) = ',' SET @UpdateList = STUFF(@UpdateList, 1, 1, '');

    IF RIGHT(@UpdateList, 1) = ',' SET @UpdateList = LEFT(@UpdateList, LEN(@UpdateList) - 1);

    SET @Sql = N'
    MERGE ' + QUOTENAME(@SchemaName) + '.' + QUOTENAME(@PureTable) + ' AS target
    USING (
        SELECT *
        FROM OPENJSON(@json, ''$.records'')
        WITH (
            ' + @WithClause + '
        )
    ) AS source
    ON ' + @JoinClause + '
    WHEN MATCHED THEN
        UPDATE SET ' + @UpdateList + '
    WHEN NOT MATCHED THEN
        INSERT (' + @ColumnList + ')
        VALUES (' + @ColumnList + ');';

    EXEC sp_executesql
        @Sql,
        @params = N'@json NVARCHAR(MAX)',
        @json = @LookupJson;
END