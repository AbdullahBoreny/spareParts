CREATE TABLE [Security].[Users]
(
	[UserID]						UNIQUEIDENTIFIER NOT NULL,
	[UserName]						NVARCHAR(100) NOT NULL,
	[UserNameShort]					NVARCHAR(100) NOT NULL,
	[UserEmail]						NVARCHAR(100) NOT NULL,
	[UserPassword]					NVARCHAR(MAX) NOT NULL,
	[UserCreationDate]				DATETIME NOT NULL,
	[UserRoleID]					INT NOT NULL

CONSTRAINT [PK_Security_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
CONSTRAINT [FK_Users_Roles] FOREIGN KEY (UserRoleID) REFERENCES [Security].[Roles]([RoleID])
);