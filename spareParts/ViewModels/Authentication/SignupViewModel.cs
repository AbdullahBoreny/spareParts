using CommunityToolkit.Mvvm.ComponentModel;
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
        private readonly ApiService apiService;

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
        public SignupViewModel()
        {
            apiService = new ApiService();
            SignupCommand = new Command(async () => await SignupAsync());
            NavigateToLoginCommand = new Command(async () => await NavigateToLoginAsync());
        }

        public ICommand SignupCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        private async Task SignupAsync()
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
            if (IsShopOwner)
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "BasicInfo", new SignupRequest { 
                        Name = Name, 
                        UserEmail = UserEmail, 
                        Password = Password, 
                        IsShopOwner = true 
                    }}
                };

                await Shell.Current.GoToAsync("ShopRegistrationPage", navigationParameter);
            }
            else
            {
                await PerformRegistration(new SignupRequest {
                    Name = Name, 
                    UserEmail = UserEmail, 
                    Password = Password, 
                    IsShopOwner = false 
                });
            }
        }

        private async Task NavigateToLoginAsync()
        {
            await NavigationService.SetRoot("LoginPage");
        }

        private async Task PerformRegistration(SignupRequest signupRequest)
        {
            try
            {
                string responseString = await apiService.PostAsync("Signup", signupRequest);
                var response = System.Text.Json.JsonSerializer.Deserialize<SignupResponse>(responseString);
                if (response.Success)
                {
                    var authStateService = AuthenticationStateService.Instance;
                    await authStateService.Login(Name, UserEmail);
                    
                    await Shell.Current.DisplayAlert("Success", "Account created successfully!", "OK");
                    
                    if (Application.Current is App app)
                    {
                        await NavigationService.SetRoot("AppShellWithBottomTabs");
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
    }

    public class SignupResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }

    public class SignupRequest
    {
        public string UserEmail { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public bool IsShopOwner { get; set; }
    }
}