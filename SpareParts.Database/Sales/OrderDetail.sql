CREATE TABLE [Sales].[OrderDetail]
(
	[ShopID]						UNIQUEIDENTIFIER NOT NULL,
	[UserID]						UNIQUEIDENTIFIER NOT NULL,
	[OrderID]						UNIQUEIDENTIFIER NOT NULL,
	[OrderNumber]					INT NOT NULL,
	[OrderProductID]				UNIQUEIDENTIFIER NOT NULL,
	[OrderProductQty]				INT NOT NULL,
	[OrderProductTotalQty]			INT NOT NULL,
	[OrderProductVal]				NUMERIC(21,13) NOT NULL,
	[OrderProductTotalVal]			NUMERIC(21,13) NOT NULL,
	[OrderDate]						DATETIME NOT NULL,
	[OrderProductNote]				NVARCHAR(MAX) NULL,

CONSTRAINT [PK_OrderDetail]	PRIMARY KEY ([ShopID] ASC, [UserID] ASC, [OrderID] ASC, [OrderNumber] ASC, [OrderProductID] ASC),
CONSTRAINT [FK_OrderDetail_OrderHeader] FOREIGN KEY (ShopID, UserID,OrderID, OrderNumber) REFERENCES [Sales].[OrderHeader](ShopID, UserID,OrderID, OrderNumber)
)
