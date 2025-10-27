using Microsoft.Maui.Controls;
using spareParts.Services;

namespace spareParts.PageViews
{
    public partial class AppShellWithBottomTabs : Shell
    {
        private readonly AuthenticationStateService _authService;

        public AppShellWithBottomTabs()
        {
            InitializeComponent();
            _authService = AuthenticationStateService.Instance;
            
            // Apply responsive styling
            ApplyResponsiveStyling();
            
            // Subscribe to authentication state changes
            _authService.PropertyChanged += OnAuthenticationStateChanged;
            
            // Check initial authentication state
            UpdateTabsBasedOnAuthState();
        }

        private void ApplyResponsiveStyling()
        {
            // Set tab bar to bottom for all platforms
            Shell.SetTabBarIsVisible(this, true);
            
            // Platform-specific styling
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // iOS specific styling
                Shell.SetTabBarBackgroundColor(this, Color.FromArgb("#F8F9FA"));
                Shell.SetTabBarForegroundColor(this, Color.FromArgb("#3498DB"));
                Shell.SetTabBarUnselectedColor(this, Color.FromArgb("#ADB5BD"));
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                // Android specific styling
                Shell.SetTabBarBackgroundColor(this, Color.FromArgb("#F8F9FA"));
                Shell.SetTabBarForegroundColor(this, Color.FromArgb("#3498DB"));
                Shell.SetTabBarUnselectedColor(this, Color.FromArgb("#ADB5BD"));
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                // Windows specific styling - force bottom tabs
                Shell.SetTabBarBackgroundColor(this, Color.FromArgb("#F8F9FA"));
                Shell.SetTabBarForegroundColor(this, Color.FromArgb("#3498DB"));
                Shell.SetTabBarUnselectedColor(this, Color.FromArgb("#ADB5BD"));
            }

            // Apply responsive icon sizing based on screen size
            ApplyResponsiveIconSizing();

            // Handle orientation changes
            DeviceDisplay.MainDisplayInfoChanged += OnDisplayInfoChanged;
        }

        private void ApplyResponsiveIconSizing()
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            var screenWidth = displayInfo.Width / displayInfo.Density;
            
            // Adjust icon sizes based on screen width
            double iconSize = 24; // Default size
            
            if (screenWidth < 400) // Small screens (phones in portrait)
            {
                iconSize = 20;
            }
            else if (screenWidth > 800) // Large screens (tablets, desktop)
            {
                iconSize = 28;
            }
            
            // Apply the calculated icon size to all tab icons
            foreach (var item in Items)
            {
                if (item is TabBar tabBar)
                {
                    foreach (var shellContent in tabBar.Items)
                    {
                        if (shellContent.Icon is FontImageSource fontIcon)
                        {
                            fontIcon.Size = iconSize;
                        }
                    }
                }
            }
        }

        private void OnDisplayInfoChanged(object? sender, DisplayInfoChangedEventArgs e)
        {
            // Reapply responsive icon sizing when display info changes
            ApplyResponsiveIconSizing();
        }

        private void OnAuthenticationStateChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AuthenticationStateService.IsAuthenticated))
            {
                UpdateTabsBasedOnAuthState();
            }
        }

        private void UpdateTabsBasedOnAuthState()
        {
            // Get the TabBar
            if (Items.FirstOrDefault() is TabBar tabBar)
            {
                // Show/hide tabs based on authentication state
                if (!_authService.IsAuthenticated)
                {
                    // Hide Profile and Cart tabs when not authenticated
                    if (tabBar.Items.Count > 1)
                    {
                        for (int i = 1; i < tabBar.Items.Count; i++)
                        {
                            tabBar.Items[i].IsVisible = false;
                        }
                    }
                }
                else
                {
                    // Show all tabs when authenticated
                    foreach (var item in tabBar.Items)
                    {
                        item.IsVisible = true;
                    }
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Unsubscribe from events to prevent memory leaks
            _authService.PropertyChanged -= OnAuthenticationStateChanged;
            DeviceDisplay.MainDisplayInfoChanged -= OnDisplayInfoChanged;
        }
    }
}
