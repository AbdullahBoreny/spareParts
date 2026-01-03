using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.PlatformConfiguration;
using spareParts.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace spareParts.ViewModels.Authentication
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial string Email { get; set; }

        [ObservableProperty]
        public partial string Password { get; set; }

        private bool IsLoading = false;


        [RelayCommand]
        private async Task LoginAsync()
        {
            var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await currentPage.DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            try
            {
                IsLoading = true;
                var success = true;
                
                if (success)
                {
                    // Set authentication state
                    var authStateService = AuthenticationStateService.Instance;
                    authStateService.Login("Demo User", Email);
                    
                    await currentPage.DisplayAlert("Success", "Login successful!", "OK");
                    
                    // Navigate to main app
                    if (Application.Current is App app)
                    {
                        app.NavigateToMainApp();
                    }
                }
                else
                {
                    await currentPage.DisplayAlert("Error", "Invalid email or password", "OK");
                }
            }
            catch (Exception ex)
            {
                await currentPage.DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task NavigateToSignupAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PageViews.Authentication.SignupPage());
        }
}