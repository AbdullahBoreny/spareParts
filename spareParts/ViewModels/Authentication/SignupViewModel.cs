using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel.Communication;
using spareParts.PageViews;
using spareParts.PageViews.Authentication;
using spareParts.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace spareParts.ViewModels.Authentication
{
    public partial class SignupViewModel : ObservableObject
    {
        private readonly ApiService apiService = new ApiService();

        [ObservableProperty]
        public partial string Name { get; set; }

        [ObservableProperty]
        public partial string UserEmail { get; set; }

        [ObservableProperty]
        public partial string Password { get; set; }

        [ObservableProperty]
        public partial string ConfirmPassword { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsConsumer))]
        public partial bool IsShopOwner { get; set; } = false;

        public bool IsConsumer
        {
            get => !IsShopOwner;
            set => IsShopOwner = !value;
        }

        
        [RelayCommand]
        public async Task Signup()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(UserEmail) || 
                string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            if (Password.Length < 6)
            {
                await Shell.Current.DisplayAlert("Error", "Password must be at least 6 characters long", "OK");
                return;
            }
            try
            {
                SignupRequest signupRequest = new SignupRequest { FullName = Name, Email = UserEmail, Password = Password,IsShopOwner = IsShopOwner };
                string responseString = await apiService.PostAsync("Signup", signupRequest);
                var response = System.Text.Json.JsonSerializer.Deserialize<SignupResponse>(responseString);
                if (response.Success)
                {
                    var authStateService = AuthenticationStateService.Instance;
                    await authStateService.Login(Name, UserEmail, response.UserID);
                    
                    Shell.Current.DisplayAlert("Success", "Account created successfully!", "OK");
                    
                    if (Application.Current is App app)
                    {
                        Application.Current.MainPage = new AppShellWithBottomTabs();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(response.Message))
                    {
                        await Shell.Current.DisplayAlert("Error", response.Message, "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Registration failed. Please try again.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Registration failed: {ex.Message}", "OK");
            }
        }
        [RelayCommand]
        public async Task NavigateToLogin()
        {
            await NavigationService.SetRoot("LoginPage");
        }
    }

    public class SignupResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string UserID {get; set;}
    }

    public class SignupRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public bool IsShopOwner { get; set; }
    }
}