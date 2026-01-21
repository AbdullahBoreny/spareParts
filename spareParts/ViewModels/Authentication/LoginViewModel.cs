using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using spareParts.PageViews;
using spareParts.PageViews.Authentication;
using spareParts.Utilities_network;
using spareParts.Services;
using System.Threading.Tasks;

namespace spareParts.ViewModels.Authentication
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial string Mail { get; set; }

        [ObservableProperty]
        public partial string Password { get; set; }

        [ObservableProperty]
        public partial bool IsLoading { get; set; }

        public ApiService apiService = new ApiService();



        public ICommand SignupCommand { get; }
        public ICommand NavigateToSignupCommand { get; }

        public ICommand LoginAsyncCommand { get; }

        public LoginViewModel()
        {
            NavigateToSignupCommand = new Command(async () =>  await NavigateToSignupAsync());
            LoginAsyncCommand = new Command(() => LoginAsync());
        }

        private async void LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Mail) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill in all fields", "OK");

                IsLoading = false;
                return;
            }
            if (NetworkCheck.IsConnected())
            {
                LoginRequest loginRequest = new LoginRequest() { Email = Mail, Password = Password };
                string responseString = await apiService.PostAsync("Login", loginRequest);
                var response = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(responseString);

                if (response.Success)
                {
                    var authStateService = AuthenticationStateService.Instance;
                    await authStateService.Login(response.Username, Mail, response.UserID);

                    if (Application.Current is App app)
                    {
                        Application.Current.MainPage = new AppShellWithBottomTabs();
                    }

                    IsLoading = false;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Invalid email or password", "OK");

                    IsLoading = false;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Oops!", "Failed to connect to the internet", "OK");

                IsLoading = false;
            }
        }

        private async Task NavigateToSignupAsync()
        {
            await NavigationService.SetRoot("SignupPage");
        }
        public class LoginRequest
        {            
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class LoginResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public string Username { get; set; }
            public string UserID {get; set;}
        }
    }
}