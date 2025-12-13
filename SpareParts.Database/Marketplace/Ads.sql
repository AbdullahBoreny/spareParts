CREATE TABLE [Marketplace].[Ads]
(
    [AdID] UNIQUEIDENTIFIER DEFAULT NEWID(),
    [SellerId] UNIQUEIDENTIFIER NOT NULL,
    [AdTitle] NVARCHAR(200) NOT NULL,
    [AdDescription] NVARCHAR(MAX) NOT NULL,
    [AdPrice] DECIMAL(18,2) NULL,
    [AdCategory] NVARCHAR(100),
    [ConditionID] INT NULL,
    [AdLocation] NVARCHAR(200),
    [AdCreationDate] DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    [AdExpirayDate] DATETIME2 NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,

CONSTRAINT [PK_Ads] PRIMARY KEY CLUSTERED ([AdID] ASC),
CONSTRAINT [FK_Ads_Users] FOREIGN KEY (SellerId) REFERENCES [Security].[Users] (UserID),
CONSTRAINT [FK_Ads_Conditions] FOREIGN KEY ([ConditionID]) REFERENCES [General].[Conditions] ([ConditionID])
)
