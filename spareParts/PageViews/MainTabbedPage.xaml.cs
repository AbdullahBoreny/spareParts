using Microsoft.Maui.Controls;
using spareParts.Services;

namespace spareParts.PageViews
{
    public partial class MainTabbedPage : TabbedPage
    {
        private readonly AuthenticationStateService _authService;

        public MainTabbedPage()
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
            // Platform-specific tab bar adjustments
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // iOS tabs are at bottom by default
                // iOS specific styling
                this.SetValue(TabbedPage.BarBackgroundColorProperty, Color.FromArgb("#F8F9FA"));
                
                // Add safe area handling for iOS
                if (DeviceInfo.Version.Major >= 11) // iOS 11+ with notch support
                {
                    // iOS handles safe areas automatically for tab bars
                }
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                // Android tabs are at bottom by default
                // Android specific styling
                this.SetValue(TabbedPage.BarBackgroundColorProperty, Color.FromArgb("#F8F9FA"));
                
                // Handle different screen densities
                var density = DeviceDisplay.MainDisplayInfo.Density;
                if (density >= 3.0) // XXHDPI and above
                {
                    // Adjust for high-density screens
                    foreach (var child in Children)
                    {
                        if (child is NavigationPage navPage && navPage.IconImageSource is FontImageSource fontIcon)
                        {
                            fontIcon.Size = 28; // Larger icons for high-density screens
                        }
                    }
                }
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                // Windows tabs are at top by default, but we can't easily move them to bottom
                // Windows specific styling
                this.SetValue(TabbedPage.BarBackgroundColorProperty, Color.FromArgb("#F8F9FA"));
                
                // Adjust for Windows UI guidelines
                foreach (var child in Children)
                {
                    if (child is NavigationPage navPage && navPage.IconImageSource is FontImageSource fontIcon)
                    {
                        fontIcon.Size = 20; // Smaller icons for Windows
                    }
                }
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
            
            // Apply the calculated icon size
            foreach (var child in Children)
            {
                if (child is NavigationPage navPage && navPage.IconImageSource is FontImageSource fontIcon)
                {
                    fontIcon.Size = iconSize;
                }
            }
        }

        private void OnDisplayInfoChanged(object? sender, DisplayInfoChangedEventArgs e)
        {
            // Adjust layout based on orientation and screen size changes
            var orientation = e.DisplayInfo.Orientation;
            
            // Reapply responsive icon sizing when display info changes
            ApplyResponsiveIconSizing();
            
            if (orientation == DisplayOrientation.Landscape)
            {
                // In landscape, we might want to adjust tab spacing or size
                // This is handled automatically by the platform, but we can add custom logic here
            }
            else if (orientation == DisplayOrientation.Portrait)
            {
                // Portrait mode adjustments
                // Platform handles this automatically
            }
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
            // If user is not authenticated, show only Home tab
            // The Home tab will handle showing login/signup options
            if (!_authService.IsAuthenticated)
            {
                // Hide Profile and Cart tabs when not authenticated
                if (Children.Count > 1)
                {
                    for (int i = Children.Count - 1; i > 0; i--)
                    {
                        Children[i].IsVisible = false;
                    }
                }
            }
            else
            {
                // Show all tabs when authenticated
                foreach (var child in Children)
                {
                    child.IsVisible = true;
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
