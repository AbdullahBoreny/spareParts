using Microsoft.AspNetCore.SignalR.Client;
using spareParts.Services;

namespace spareParts.ViewModels.Customer
{
    internal class ChatsViewModel
    {
        public AuthenticationStateService _authService;
        private ApiService apiService;
        private HubConnection hubConnection;
        public float  TotalPrice = 0;

        public ChatsViewModel()
        {
            _authService =  AuthenticationStateService.Instance;
                hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{apiService._hubUrl}/ChatHub", options =>
                    {
                        options.Headers.Add("ClientId", _authService.CurrentUserID.ToString());
                    })
                    .WithAutomaticReconnect()
                    .Build();
        }
    }
}