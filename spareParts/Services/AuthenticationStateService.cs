using System.ComponentModel;

namespace spareParts.Services
{
    public class AuthenticationStateService : INotifyPropertyChanged
    {
        private static AuthenticationStateService _instance;
        public static AuthenticationStateService Instance => _instance ??= new AuthenticationStateService();

        private bool _isAuthenticated;
        private string _currentUserName = string.Empty;
        private string _currentUserEmail = string.Empty;
        private Guid _currentUserID = Guid.Empty;

        private const string IsAuthenticatedKey = "IsAuthenticated";
        private const string UserNameKey = "CurrentUserName";
        private const string UserEmailKey = "CurrentUserEmail";
        private const string UserIDKey = "CurrentUserID";

        private AuthenticationStateService()
        {
            // Note: We cannot await in a constructor. 
            // We call an async initialization method.
            _ = InitializeAsync();
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

        public Guid CurrentUserID
        {
            get => CurrentUserID;
            private set
            {
                if (CurrentUserID != value)
                {
                    CurrentUserID = value;
                    OnPropertyChanged(nameof(CurrentUserID));
                }
            }
        }

        public async Task InitializeAsync()
        {
            try
            {
                // SecureStorage only stores strings, so we convert "true"/"false" back to bool
                var authStr = await SecureStorage.Default.GetAsync(IsAuthenticatedKey);
                IsAuthenticated = authStr == "true";
                
                CurrentUserName = await SecureStorage.Default.GetAsync(UserNameKey) ?? string.Empty;
                CurrentUserEmail = await SecureStorage.Default.GetAsync(UserEmailKey) ?? string.Empty;
                var useridString = await SecureStorage.Default.GetAsync(UserIDKey);
                CurrentUserID = Guid.Parse(useridString);
            }
            catch (Exception)
            {
                // Possible if device doesn't support SecureStorage or user disabled screen lock
                await Logout();
            }
        }

        public async Task Login(string userName, string email, Guid userID)
        {
            CurrentUserName = userName;
            CurrentUserEmail = email;
            IsAuthenticated = true;
            CurrentUserID = userID;
            await SaveAuthenticationState();
        }

        public async Task Logout()
        {
            CurrentUserName = string.Empty;
            CurrentUserEmail = string.Empty;
            IsAuthenticated = false;
            CurrentUserID = Guid.Empty;
            
            // It is safer to RemoveAll or remove specific keys on logout
            SecureStorage.Default.Remove(IsAuthenticatedKey);
            SecureStorage.Default.Remove(UserNameKey);
            SecureStorage.Default.Remove(UserEmailKey);
            SecureStorage.Default.Remove(UserIDKey);
        }

        private async Task SaveAuthenticationState()
        {
            try
            {
                await SecureStorage.Default.SetAsync(IsAuthenticatedKey, IsAuthenticated ? "true" : "false");
                await SecureStorage.Default.SetAsync(UserNameKey, CurrentUserName);
                await SecureStorage.Default.SetAsync(UserEmailKey, CurrentUserEmail);
                await SecureStorage.Default.SetAsync(UserIDKey, CurrentUserID.ToString());
            }
            catch (Exception)
            {
                // Handle potential errors (e.g., storage full or hardware issues)
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}