CREATE TABLE [Security].[UserRole]
(
	[UserRoleID]	UNIQUEIDENTIFIER NOT NULL,
	[UserID]		UNIQUEIDENTIFIER NOT NULL,
	[RoleID]		INT NOT NULL,
	[ModifiedOn]	DATETIME NOT NULL,

CONSTRAINT [PK_Security_UserRole] PRIMARY KEY CLUSTERED ([UserRoleID] ASC),
CONSTRAINT [FK_Security_Users_UserRole] FOREIGN KEY (UserID) REFERENCES [Security].[Users](UserID),
CONSTRAINT [FK_Security_Roles_UserRole] FOREIGN KEY (RoleID) REFERENCES [Security].[Roles](RoleID)
)
