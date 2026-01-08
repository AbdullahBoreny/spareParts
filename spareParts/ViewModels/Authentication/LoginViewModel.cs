using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using spareParts.PageViews;
using spareParts.PageViews.Authentication;
using spareParts.Utilities_network;
using spareParts.Services;

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
            NavigateToSignupCommand = new Command(() => NavigateToSignupAsync());
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
                loginRequest loginRequest = new loginRequest() { Email = Mail, Password = Password };
                var response = await apiService.PostAsync<loginRespone>("sync/Login", loginRequest);

                if (response.Success)
                {
                    var authStateService = AuthenticationStateService.Instance;
                    authStateService.Login("Demo User", Mail);

                    await Shell.Current.DisplayAlert("Success", "Login successful!", "OK");

                    if (Application.Current is App app)
                    {
                        NavigationService.SetRoot(new AppShellWithBottomTabs());
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

        private void NavigateToSignupAsync()
        {
            NavigationService.SetRoot(new SignupPage());
        }
        private class loginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        private class loginRespone
        {
            public bool Success { get; set; }

            public string Username { get; set; }

            public string Email { get; set; }
        }
    }
}