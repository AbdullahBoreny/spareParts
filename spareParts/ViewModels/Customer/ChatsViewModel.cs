using Microsoft.AspNetCore.SignalR.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using spareParts.Services;

namespace spareParts.ViewModels.Customer
{
    internal partial class ChatsViewModel : ObservableObject
    {
        public AuthenticationStateService _authService;
        private ApiService apiService = new ApiService();
        private HubConnection hubConnection;

        public ObservableCollection<ChatRecord> ChatList = new ObservableCollection<ChatRecord>();
        public ChatsViewModel()
        {
            _authService =  AuthenticationStateService.Instance;
            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{apiService._hubUrl}/ChatHub", options =>
                {
                    options.Headers.Add("ClientId", _authService.CurrentUserID);
                })
                .WithAutomaticReconnect()
                .Build();
            preparePage();
        }

        public async void preparePage()
        {
            ChatRequest chatRequest = new ChatRequest
            {
                UserID = _authService.CurrentUserID
            };
            string responseString = await apiService.PostAsync("GetChats", chatRequest);
            var response = System.Text.Json.JsonSerializer.Deserialize<ChatResponse>(responseString);

            if (response.Success)
            {
                ChatList = response.Chats;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", string.IsNullOrEmpty(response.Message) ? "Error Occurrd getting your conversations" : response.Message, "Ok");
            }
            
        }

        [RelayCommand]
        private async Task OpenChat(ChatRecord selectedChat)
        {
            
        }
    }

    public class ChatRecord
    {
        public Guid ContactID {get; set;}
        public bool IsOnline {get; set;}
        public string ContactName {get; set;}
        public string LastMessage {get; set;}
        public DateTime LastMessageTime {get; set;}
        public int UnreadCount {get; set;}
    }

    public class ChatRequest
    {
        public string UserID {get; set;}
    }
    public class ChatResponse
    {
        public bool Success {get; set;}

        public string Message {get ;set;}

        public ObservableCollection<ChatRecord> Chats {get; set;}
    }
}