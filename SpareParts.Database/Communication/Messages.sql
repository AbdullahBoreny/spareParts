CREATE TABLE [Communication].[Messages]
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,

    ConversationId INT NOT NULL,
    SenderId INT NOT NULL,

    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    IsRead BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_Messages_Conversations
        FOREIGN KEY (ConversationId)
        REFERENCES [Communication].[Conversations](Id)
);
