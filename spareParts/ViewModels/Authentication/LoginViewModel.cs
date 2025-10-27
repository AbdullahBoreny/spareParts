using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using spareParts.Services;

namespace spareParts.ViewModels.Authentication
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private bool _isLoading = false;

        public LoginViewModel()
        {
            _authService = new AuthService();
            LoginCommand = new Command(async () => await LoginAsync());
            NavigateToSignupCommand = new Command(async () => await NavigateToSignupAsync());
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToSignupCommand { get; }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            try
            {
                IsLoading = true;
                var success = await _authService.LoginAsync(Email, Password);
                
                if (success)
                {
                    // Set authentication state
                    var authStateService = spareParts.Services.AuthenticationStateService.Instance;
                    authStateService.Login("Demo User", Email);
                    
                    await Application.Current.MainPage.DisplayAlert("Success", "Login successful!", "OK");
                    
                    // Navigate back to home page
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid email or password", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}