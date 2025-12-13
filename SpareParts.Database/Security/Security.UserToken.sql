CREATE TABLE [Security].[UserToken]
(
	[TokenID]						UNIQUEIDENTIFIER NOT NULL,
	[UserID]						UNIQUEIDENTIFIER NOT NULL,
	[TokenCreationDate]				DATETIME NOT NULL,

CONSTRAINT [PK_Security_UserToken] PRIMARY KEY ([TokenID] ASC, [UserID] ASC),
CONSTRAINT [FK_Security_Users] FOREIGN KEY ([UserID]) REFERENCES [Security].[Users]([UserID])
);