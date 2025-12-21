CREATE TABLE [Communication].[Conversation]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [CustomerId] UNIQUEIDENTIFIER NOT NULL,
    [ShopOwnerId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT [PK_Conversation] PRIMARY KEY ([Id]),

    CONSTRAINT [FK_Conversation_Customer]
        FOREIGN KEY ([CustomerId])
        REFERENCES [Security].[Users]([UserID]),

    CONSTRAINT [FK_Conversation_ShopOwner]
        FOREIGN KEY ([ShopOwnerId])
        REFERENCES [Security].[Users]([UserID])
);
