CREATE TABLE [Communication].[Conversation]
(
    [ConversationID] INT IDENTITY(1,1) NOT NULL,
    [SenderUserID] UNIQUEIDENTIFIER NOT NULL,
    [ReceiverUserID] UNIQUEIDENTIFIER NOT NULL,
    [ConversationDate] DATETIME NOT NULL DEFAULT GETDATE(),

CONSTRAINT [PK_Conversation] PRIMARY KEY ([ConversationID] ASC),
CONSTRAINT [FK_Conversation_Sender] FOREIGN KEY ([SenderUserID]) REFERENCES [Security].[Users]([UserID]),
CONSTRAINT [FK_Conversation_ShopOwner] FOREIGN KEY ([ReceiverUserID]) REFERENCES [Security].[Users]([UserID])
);
