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

        private AuthenticationStateService()
        {
            // Initialize as not authenticated
            IsAuthenticated = false;
        }

        public void Login(string userName, string email)
        {
            CurrentUserName = userName;
            CurrentUserEmail = email;
            IsAuthenticated = true;
        }

        public void Logout()
        {
            CurrentUserName = string.Empty;
            CurrentUserEmail = string.Empty;
            IsAuthenticated = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
