CREATE TABLE [Communication].[Messages]
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ConversationId INT NOT NULL,          
    SenderId UNIQUEIDENTIFIER NOT NULL,    
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    IsRead BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (ConversationId) REFERENCES [Communication].[Conversation](Id),
    FOREIGN KEY (SenderId) REFERENCES [Security].[Users](UserID)
);
