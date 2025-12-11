CREATE TABLE [Security].[UserToken]
(
	[TokenID]						UNIQUEIDENTIFIER NOT NULL,
	[TokenUserID]					INT NOT NULL,
	[TokenCreationDate]				DATETIME NOT NULL,

CONSTRAINT [PK_Security_UserToken] PRIMARY KEY ([TokenID] ASC),
CONSTRAINT [FK_Security_Users] FOREIGN KEY ([TokenUserID]) REFERENCES [Security].[Users]([UserID])
);