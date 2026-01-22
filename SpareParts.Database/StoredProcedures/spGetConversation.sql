CREATE PROCEDURE [dbo].[spGetConversation] 
(
    @CurrentUserID UNIQUEIDENTIFIER,
    @OtherUserID UNIQUEIDENTIFIER
)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ConversationID INT  = (SELECT ConversationID from Communication.Conversation WHERE (SenderUserID = @CurrentUserID OR ReceiverUserID = @CurrentUserID) AND (SenderUserID = @OtherUserID OR ReceiverUserID = @OtherUserID))
    
    
    SELECT SenderUserID
        , MessageContent
        , MessageCreationDate
        , IsRead
    FROM Communication.Messages
    WHERE ConversationID = @ConversationID
    
END