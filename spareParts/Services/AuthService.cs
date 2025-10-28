using System.Text.Json;

namespace spareParts.Services
{
    public class AuthService
    {
        private const string UsersKey = "RegisteredUsers";

        private async Task<Dictionary<string, string>> GetUsersAsync()
        {
            // Get saved users from Preferences
            if (Preferences.ContainsKey(UsersKey))
            {
                var json = Preferences.Get(UsersKey, string.Empty);
                if (!string.IsNullOrEmpty(json))
                    return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }

            return new Dictionary<string, string>();
        }

        private async Task SaveUsersAsync(Dictionary<string, string> users)
        {
            var json = JsonSerializer.Serialize(users);
            Preferences.Set(UsersKey, json);
        }

        // Register a new user
        public async Task<bool> RegisterAsync(string email, string password, string name)
        {
            var users = await GetUsersAsync();

            // Block duplicate registration
            if (users.ContainsKey(email))
                return false;

            users[email] = password;
            await SaveUsersAsync(users);
            return true;
        }

        // Login validation
        public async Task<bool> LoginAsync(string email, string password)
        {
            var users = await GetUsersAsync();

            // Only allow login if user exists and password matches
            if (users.ContainsKey(email) && users[email] == password)
                return true;

            return false;
        }

        // Optional: clear all users (for testing)
        public void ClearAllUsers()
        {
            Preferences.Remove(UsersKey);
        }
    }
}
