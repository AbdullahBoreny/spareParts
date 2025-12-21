CREATE TABLE [Communication].[Messages]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [ConversationId] INT NOT NULL,
    [SenderId] UNIQUEIDENTIFIER NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
    [IsRead] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [PK_Messages] PRIMARY KEY ([Id]),

    CONSTRAINT [FK_Messages_Conversation]
        FOREIGN KEY ([ConversationId])
        REFERENCES [Communication].[Conversation]([Id]),

    CONSTRAINT [FK_Messages_Users]
        FOREIGN KEY ([SenderId])
        REFERENCES [Security].[Users]([UserID])
);
