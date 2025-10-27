using System.ComponentModel;

namespace spareParts.Services
{
    public class AuthenticationStateService : INotifyPropertyChanged
    {
        private static AuthenticationStateService? _instance;
        public static AuthenticationStateService Instance => _instance ??= new AuthenticationStateService();

        private bool _isAuthenticated;
        private string _currentUserName = string.Empty;
        private string _currentUserEmail = string.Empty;

        // Keys for storing authentication data
        private const string IsAuthenticatedKey = "IsAuthenticated";
        private const string UserNameKey = "CurrentUserName";
        private const string UserEmailKey = "CurrentUserEmail";

        private AuthenticationStateService()
        {
            // Load authentication state from preferences on startup
            LoadAuthenticationState();
        }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            private set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    OnPropertyChanged(nameof(IsAuthenticated));
                }
            }
        }

        public string CurrentUserName
        {
            get => _currentUserName;
            private set
            {
                if (_currentUserName != value)
                {
                    _currentUserName = value;
                    OnPropertyChanged(nameof(CurrentUserName));
                }
            }
        }

        public string CurrentUserEmail
        {
            get => _currentUserEmail;
            private set
            {
                if (_currentUserEmail != value)
                {
                    _currentUserEmail = value;
                    OnPropertyChanged(nameof(CurrentUserEmail));
                }
            }
        }

        public void Login(string userName, string email)
        {
            CurrentUserName = userName;
            CurrentUserEmail = email;
            IsAuthenticated = true;
            SaveAuthenticationState();
        }

        public void Logout()
        {
            CurrentUserName = string.Empty;
            CurrentUserEmail = string.Empty;
            IsAuthenticated = false;
            SaveAuthenticationState();
        }

        private void LoadAuthenticationState()
        {
            try
            {
                _isAuthenticated = Preferences.Get(IsAuthenticatedKey, false);
                _currentUserName = Preferences.Get(UserNameKey, string.Empty);
                _currentUserEmail = Preferences.Get(UserEmailKey, string.Empty);
            }
            catch (Exception)
            {
                // If there's any error loading preferences, default to not authenticated
                _isAuthenticated = false;
                _currentUserName = string.Empty;
                _currentUserEmail = string.Empty;
            }
        }

        private void SaveAuthenticationState()
        {
            try
            {
                Preferences.Set(IsAuthenticatedKey, _isAuthenticated);
                Preferences.Set(UserNameKey, _currentUserName);
                Preferences.Set(UserEmailKey, _currentUserEmail);
            }
            catch (Exception)
            {
                // Handle any errors saving preferences silently
                // In a production app, you might want to log this
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}