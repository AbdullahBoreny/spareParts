using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using spareParts.Models;
using spareParts.Services;
using System.Collections.ObjectModel;
using spareParts.PageViews.Customer;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;
namespace spareParts.ViewModels.Customer;

[QueryProperty(nameof(ChatInfo), "ChatData")]
public partial class ChatViewModel : ObservableObject
{
    [ObservableProperty]
    public ChatNav chatInfo;
    public ApiService apiService = new ApiService();

    public string UserID = string.Empty;
    public AuthenticationStateService AuthService {get; set;}
    [ObservableProperty]
    public string shopName;

    [ObservableProperty]
    public string newMessageText;
    
    public string OtherPartyUserID = string.Empty;

    public ObservableCollection<Chat> Messages { get; set;}

    public AuthenticationStateService _authService;
    public HubConnection hubConnection;

    public ChatViewModel()
    {
        AuthService = AuthenticationStateService.Instance;

        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await AuthenticationStateService.Instance.InitializeAsync();
            UserID = AuthService.CurrentUserID;


        hubConnection = new HubConnectionBuilder()
            .WithUrl($"{apiService._hubUrl}/ChatHub", options =>
            {
                options.Headers.Add("ClientId", UserID);
            })
            .WithAutomaticReconnect()
            .Build();
            await loadMessages();
        });
    }
    partial void OnChatInfoChanged(ChatNav value)
    {
        if (value != null)
        {
            OtherPartyUserID = value.ContactUserID;
            shopName = value.ContactName;
        }
    }
    public async Task loadMessages()
    {
        string responseString = await apiService.PostAsync("GetConversation", new {MyUserID = UserID, OtherPartyUserID = OtherPartyUserID});
        var response = System.Text.Json.JsonSerializer.Deserialize<ChatResponse>(responseString);
        if (response.Success)
        {
            Messages = response.chats;
        }
        else
        {
            if (string.IsNullOrEmpty(response.Message))
            {
                await Shell.Current.DisplayAlert("Error", response.Message, "Ok");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "An issue occurred retirieving your messages", "Ok");
            }
        }
    }

    [RelayCommand]
    private async Task Send()
    {
        if (string.IsNullOrWhiteSpace(NewMessageText)) return;
        try
        {
            await hubConnection.InvokeAsync("sendMessage", OtherPartyUserID, NewMessageText);
            Chat chat = new Chat 
            { 
                MessageContent = NewMessageText, 
                IsIncoming = false, 
                MessageCreationDate = DateTime.Now,
                IsRead = false,
                SenderID = UserID
            };
            Messages.Add(chat);

            apiService.PostAsync("SendMessage", chat);

            NewMessageText = string.Empty;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to send message.", "OK");
        }
    }

    public class ChatResponse
    {
        public bool Success {get; set;}
        public string Message {get; set;}
        public ObservableCollection<Chat> chats {get; set;}
    }

    public class Chat
    {
        public string SenderID {get; set;}
        public string MessageContent { get; set; }
        public DateTime MessageCreationDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsIncoming {get; set;}
    }

    public class ChatNav
    {
        public string ContactUserID {get; set;}
        public string ContactName {get; set;}
    }
}
