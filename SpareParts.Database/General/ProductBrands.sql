CREATE TABLE [General].[ProductBrands]
(
	[BrandID]			INT NOT NULL,
	[BrandDesc]			NVARCHAR(200) NOT NULL,
	[BrandCategoryID]	INT NOT NULL,
	[ModifiedOn]		DATETIME NOT NULL,

CONSTRAINT [PK_ProductBrands] PRIMARY KEY ([BrandID] ASC, [BrandCategoryID] ASC),
CONSTRAINT [FK_ProductBrands_Categories] FOREIGN KEY ([BrandCategoryID]) REFERENCES [General].[Categories] ([CategoryID])
)
