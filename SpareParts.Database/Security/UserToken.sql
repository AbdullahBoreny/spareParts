CREATE TABLE [Security].[UserToken]
(
    [ID]                    UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [UserID]                UNIQUEIDENTIFIER NOT NULL,
    [Token]                 NVARCHAR(500) NOT NULL,          
    [JwtID]                 NVARCHAR(100) NOT NULL,          
    [CreatedAt]             DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    [ExpiresAt]             DATETIME2 NOT NULL,          
    [RevokedAt]             DATETIME2 NULL,              
    [ReplacedByToken]       NVARCHAR(500) NULL,    
    [IsUsed]                BIT NOT NULL DEFAULT 0,         
    [IsRevoked]             BIT NOT NULL DEFAULT 0,      
CONSTRAINT [PK_Security_UserToken] PRIMARY KEY ([ID] ASC, [UserID] ASC),
CONSTRAINT [FK_Security_Users] FOREIGN KEY ([UserID]) REFERENCES [Security].[Users]([UserID])
);