CREATE TABLE [Communication].[Conversation]
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerId UNIQUEIDENTIFIER NOT NULL,
    ShopOwnerId UNIQUEIDENTIFIER NOT NULL, 
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (CustomerId) REFERENCES [Security].[Users](UserID),
    FOREIGN KEY (ShopOwnerId) REFERENCES [Security].[Users](UserID)
);
