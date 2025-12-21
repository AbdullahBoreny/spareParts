CREATE TABLE [Communication].[Conversations]
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,

    User1Id INT NOT NULL,
    User2Id INT NOT NULL,

    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CHK_Users_Not_Same CHECK (User1Id <> User2Id),
    CONSTRAINT UQ_User_Pair UNIQUE (User1Id, User2Id)
);