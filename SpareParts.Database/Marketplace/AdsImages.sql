CREATE TABLE [Marketplace].[AdsImages]
(
    [AdImageID]           UNIQUEIDENTIFIER DEFAULT NEWID(),
    [AdID]                UNIQUEIDENTIFIER NOT NULL,
    [ImageUrl]            NVARCHAR(500) NOT NULL,


CONSTRAINT [PK_AdsImages] PRIMARY KEY CLUSTERED ([AdImageID] ASC),
CONSTRAINT [FK_AdsImages_Ads] FOREIGN KEY (AdID) REFERENCES Marketplace.Ads(AdID)
)
