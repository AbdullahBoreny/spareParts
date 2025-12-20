CREATE TABLE [General].[ShopCategories]
(
	[ShopID]			UNIQUEIDENTIFIER NOT NULL,
	[CategoryID]		INT NOT NULL,
	[ModifiedOn]		DATETIME NOT NULL,

CONSTRAINT [PK_ShopCategories] PRIMARY KEY ([ShopID] ASC, [CategoryID] ASC),
CONSTRAINT [FK_ShopCategories_Shops] FOREIGN KEY (ShopID) REFERENCES [General].[Shops]([ShopID]),
CONSTRAINT [FK_ShopCategories_Categories] FOREIGN KEY (CategoryID) REFERENCES [General].[Categories] ([CategoryID])
)
