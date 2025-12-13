CREATE TABLE [General].[Conditions]
(
	[ConditionID]			INT NOT NULL,
	[ConditionDesc]			NVARCHAR(200) NOT NULL,
	[ModifiedOn]			DATETIME NOT NULL DEFAULT GETDATE(),


CONSTRAINT [PK_Conditions] PRIMARY KEY ([ConditionID] ASC)
)
