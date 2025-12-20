CREATE TABLE [General].[Products]
(
	[ProductID]					UNIQUEIDENTIFIER NOT NULL,
	[ProductName]				NVARCHAR(100) NOT NULL,
	[ProductNumber]				NVARCHAR(200) NULL,
	[ProductDescription]		NVARCHAR(MAX) NOT NULL,
	[ProductBrandID]			INT NOT NULL,
	[ProductCreationDate]		DATETIME NOT NULL,

CONSTRAINT [PK_Products] PRIMARY KEY ([ProductID] ASC)
)
