using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using spareParts.Services;

namespace spareParts.ViewModels.Authentication
{
    public class SignupViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private bool _isLoading = false;

        public SignupViewModel()
        {
            _authService = new AuthService();
            SignupCommand = new Command(async () => await SignupAsync());
            NavigateToLoginCommand = new Command(async () => await NavigateToLoginAsync());
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
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

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
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

        public ICommand SignupCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        private async Task SignupAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email) || 
                string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            if (Password.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password must be at least 6 characters long", "OK");
                return;
            }

            try
            {
                IsLoading = true;
                var success = await _authService.RegisterAsync(Email, Password, Name);
                
                if (success)
                {
                    // Set authentication state
                    var authStateService = spareParts.Services.AuthenticationStateService.Instance;
                    authStateService.Login(Name, Email);
                    
                    await Application.Current.MainPage.DisplayAlert("Success", "Account created successfully!", "OK");
                    
                    // Navigate to main app
                    if (Application.Current is App app)
                    {
                        app.NavigateToMainApp();
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Registration failed. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Registration failed: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task NavigateToLoginAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}