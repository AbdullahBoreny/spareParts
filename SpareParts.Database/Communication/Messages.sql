CREATE TABLE [Communication].[Messages]
(
    [MessageID] INT IDENTITY(1,1) NOT NULL,
    [ConversationID] INT NOT NULL,
    [SenderUserID] UNIQUEIDENTIFIER NOT NULL,
    [MessageContent] NVARCHAR(MAX) NOT NULL,
    [MessageCreationDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [IsRead] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [PK_Messages] PRIMARY KEY ([MessageID] ASC),
    CONSTRAINT [FK_Messages_Conversation] FOREIGN KEY ([ConversationID]) REFERENCES [Communication].[Conversation]([ConversationID]),
    CONSTRAINT [FK_Messages_Users] FOREIGN KEY ([SenderUserID]) REFERENCES [Security].[Users]([UserID])
);
