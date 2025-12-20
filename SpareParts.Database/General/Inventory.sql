CREATE TABLE [General].[Inventory]
(
	[ShopID]						UNIQUEIDENTIFIER NOT NULL,
	[ProductID]						UNIQUEIDENTIFIER NOT NULL,
	[ProductQty]					INT NOT NULL,
	[ProductConditionID]			INT NOT NULL,

CONSTRAINT [PK_Inventory] PRIMARY KEY ([ShopID] ASC, [ProductID] ASC),
CONSTRAINT [FK_Inventory_Shops] FOREIGN KEY ([ShopID]) REFERENCES [General].[Shops]([ShopID]),
CONSTRAINT [FK_Inventory_Products] FOREIGN KEY (ProductID) REFERENCES [General].[Products]([ProductID])

)
