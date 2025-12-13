CREATE TABLE [Security].[Users]
(
	[UserID]						UNIQUEIDENTIFIER NOT NULL,
	[UserName]						NVARCHAR(100) NOT NULL,
	[UserNameShort]					NVARCHAR(100) NOT NULL,
	[UserEmail]						NVARCHAR(100) NOT NULL,
	[UserPassword]					NVARCHAR(MAX) NOT NULL,
	[UserMobileNumber]				NUMERIC(21, 13) NOT NULL,
	[UserGender]					INT NOT NULL,
	[UserCreationDate]				DATETIME NOT NULL,
	[UserIsAuthenticated]			TINYINT	NOT NULL,

CONSTRAINT [PK_Security_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
);