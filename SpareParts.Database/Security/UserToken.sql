CREATE TABLE [Security].[UserToken]
(
    [ID]                    UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [UserID]                UNIQUEIDENTIFIER NOT NULL,
    [Token]                 NVARCHAR(500) NOT NULL,          -- hashed refresh token
    [JwtID]                 NVARCHAR(100) NOT NULL,          -- unique identifier of access token
    [CreatedAt]             DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    [ExpiresAt]             DATETIME2 NOT NULL,          -- expiry date of refresh token
    [RevokedAt]             DATETIME2 NULL,              -- when user logs out (optional)
    [ReplacedByToken]       NVARCHAR(500) NULL,    -- for token rotation
    [IsUsed]                BIT NOT NULL DEFAULT 0,         -- mark when token is used
    [IsRevoked]             BIT NOT NULL DEFAULT 0,      -- token is no longer valid
CONSTRAINT [PK_Security_UserToken] PRIMARY KEY ([ID] ASC, [UserID] ASC),
CONSTRAINT [FK_Security_Users] FOREIGN KEY ([UserID]) REFERENCES [Security].[Users]([UserID])
);