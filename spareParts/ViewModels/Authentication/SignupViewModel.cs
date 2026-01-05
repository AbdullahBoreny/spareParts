using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using spareParts.Services;
using spareParts.PageViews;
using spareParts.PageViews.Authentication;

namespace spareParts.ViewModels.Authentication
{
    public class SignupViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private bool _isLoading = false;

        public SignupViewModel()
        {
            _apiService = new ApiService();
            SignupCommand = new Command(async () => await SignupAsync());
            NavigateToLoginCommand = new Command(() => NavigateToLoginAsync());
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
                IsLoading = true;
                var success = await _apiService.RegisterAsync(Email, Password, Name);
                
                if (success)
                {
                    // Set authentication state
                    var authStateService = spareParts.Services.AuthenticationStateService.Instance;
                    authStateService.Login(Name, Email);
                    
                    await Shell.Current.DisplayAlert("Success", "Account created successfully!", "OK");
                    
                    // Navigate to main app
                    if (Application.Current is App app)
                    {
                        await NavigationService.GoTo("AppShellWithBottomTabs");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Registration failed. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Registration failed: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void NavigateToLoginAsync()
        {
             NavigationService.SetRoot(new LoginPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}