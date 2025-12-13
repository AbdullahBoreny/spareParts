CREATE TABLE [General].[Categories]
(
	[CategoryID]			INT NOT NULL,
	[CategoryDesc]			NVARCHAR(200) NOT NULL,
	[CategoryCreationDate]	DATETIME NOT NULL DEFAULT GETDATE(),
	
CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryID] ASC)
)
