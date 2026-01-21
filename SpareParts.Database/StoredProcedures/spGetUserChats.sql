CREATE PROCEDURE [dbo].[spGetUserChats] 
(
    @UserID UNIQUEIDENTIFIER
)
AS
BEGIN
    SET NOCOUNT ON;
    
    WITH UserConversations AS (
        SELECT 
            [Communication].[Conversations].ConversationID,
            CASE 
                WHEN [Communication].[Conversations].SenderUserID = @UserID 
                THEN [Communication].[Conversations].ReceiverUserID 
                ELSE [Communication].[Conversations].SenderUserID 
            END AS OtherUserID
        FROM [Communication].[Conversations]
        WHERE [Communication].[Conversations].SenderUserID = @UserID 
           OR [Communication].[Conversations].ReceiverUserID = @UserID
    ),
    RankedMessages AS (
        SELECT 
            [Communication].[Messages].ConversationID,
            [Communication].[Messages].MessageContent,
            [Communication].[Messages].MessageCreationDate,
            ROW_NUMBER() OVER (
                PARTITION BY [Communication].[Messages].ConversationID 
                ORDER BY [Communication].[Messages].MessageCreationDate DESC
            ) as MessageRank
        FROM [Communication].[Messages]
        INNER JOIN UserConversations 
            ON [Communication].[Messages].ConversationID = UserConversations.ConversationID
    )
    SELECT 
        [Security].[Users].UserName AS ContactName,
        [Security].[Users].UserID AS ContactID,
        RankedMessages.MessageContent AS LastMessage,
        RankedMessages.MessageCreationDate AS LastMessageTime,
        (SELECT COUNT(*) 
         FROM [Communication].[Messages] 
         WHERE [Communication].[Messages].ConversationID = UserConversations.ConversationID 
           AND [Communication].[Messages].IsRead = 0 
           AND [Communication].[Messages].SenderUserID = UserConversations.OtherUserID) AS UnreadCount
    FROM UserConversations
    INNER JOIN [Security].[Users] 
        ON UserConversations.OtherUserID = [Security].[Users].UserID
    INNER JOIN RankedMessages 
        ON UserConversations.ConversationID = RankedMessages.ConversationID
    WHERE RankedMessages.MessageRank = 1
    ORDER BY RankedMessages.MessageCreationDate DESC;
    
END