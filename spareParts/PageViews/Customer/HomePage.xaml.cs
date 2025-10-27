using Microsoft.Maui.Controls;
using spareParts.PageViews.Authentication;
using spareParts.Models;
using spareParts.Services;
using System.Collections.ObjectModel;

namespace spareParts.PageViews.Customer
{
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<ShopWithProducts> Shops { get; set; }
        private readonly AuthenticationStateService _authService;

        public HomePage()
        {
            InitializeComponent();
            
            _authService = AuthenticationStateService.Instance;
            
            // Set up button click events
            LoginButton.Clicked += async (s, e) => await Navigation.PushAsync(new LoginPage());
            SignupButton.Clicked += async (s, e) => await Navigation.PushAsync(new SignupPage());
            LogoutButton.Clicked += OnLogoutClicked;
            
            // Load sample shop data
            LoadSampleShops();
            
            // Subscribe to authentication state changes
            _authService.PropertyChanged += OnAuthenticationStateChanged;
            
            // Update UI based on current authentication state
            UpdateUIBasedOnAuthState();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Update UI when page appears (in case auth state changed while away)
            UpdateUIBasedOnAuthState();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Unsubscribe from events to prevent memory leaks
            _authService.PropertyChanged -= OnAuthenticationStateChanged;
        }

        private void OnAuthenticationStateChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AuthenticationStateService.IsAuthenticated) ||
                e.PropertyName == nameof(AuthenticationStateService.CurrentUserName) ||
                e.PropertyName == nameof(AuthenticationStateService.CurrentUserEmail))
            {
                MainThread.BeginInvokeOnMainThread(UpdateUIBasedOnAuthState);
            }
        }

        private void UpdateUIBasedOnAuthState()
        {
            bool isAuthenticated = _authService.IsAuthenticated;
            
            // Show/hide sections based on authentication state
            AuthenticationSection.IsVisible = !isAuthenticated;
            UserInfoSection.IsVisible = isAuthenticated;
            ShopsSection.IsVisible = isAuthenticated;
            
            if (isAuthenticated)
            {
                // Update user info
                WelcomeLabel.Text = $"Welcome back, {_authService.CurrentUserName}!";
                EmailLabel.Text = _authService.CurrentUserEmail;
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (result)
            {
                _authService.Logout();
            }
        }

        private async void OnViewShopClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ShopWithProducts shop)
            {
                await Navigation.PushAsync(new ShopDetailsPage(shop));
            }
        }

        private async void OnContactShopClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ShopWithProducts shop)
            {
                var action = await Application.Current.MainPage.DisplayActionSheet(
                    "Contact Shop", 
                    "Cancel", 
                    null, 
                    "Call", 
                    "Email", 
                    "Get Directions");
                
                switch (action)
                {
                    case "Call":
                        await Application.Current.MainPage.DisplayAlert("Call Shop", $"Calling {shop.Phone}", "OK");
                        break;
                    case "Email":
                        await Application.Current.MainPage.DisplayAlert("Email Shop", $"Email: {shop.Email}", "OK");
                        break;
                    case "Get Directions":
                        await Application.Current.MainPage.DisplayAlert("Directions", $"Navigate to: {shop.Address}", "OK");
                        break;
                }
            }
        }

        private void LoadSampleShops()
        {
            Shops = new ObservableCollection<ShopWithProducts>
            {
                new ShopWithProducts
                {
                    Id = 1,
                    Name = "Auto Parts Central",
                    Description = "Your one-stop shop for all automotive spare parts. We specialize in engine components, brakes, and electrical parts.",
                    Address = "123 Main Street, Downtown",
                    Phone = "(555) 123-4567",
                    Email = "info@autopartscentral.com",
                    IsActive = true,
                    ProductCount = 1250,
                    Rating = 4.8,
                    Distance = 2.3,
                    FeaturedProducts = new List<Product>
                    {
                        new Product { Name = "Brake Pads", Price = 45.99M },
                        new Product { Name = "Oil Filter", Price = 12.50M },
                        new Product { Name = "Spark Plugs", Price = 8.99M }
                    }
                },
                new ShopWithProducts
                {
                    Id = 2,
                    Name = "Motorcycle Masters",
                    Description = "Premium motorcycle parts and accessories. From sport bikes to cruisers, we have everything you need.",
                    Address = "456 Bike Lane, Westside",
                    Phone = "(555) 987-6543",
                    Email = "sales@motorcyclemasters.com",
                    IsActive = true,
                    ProductCount = 890,
                    Rating = 4.6,
                    Distance = 5.1,
                    FeaturedProducts = new List<Product>
                    {
                        new Product { Name = "Chain Set", Price = 89.99M },
                        new Product { Name = "Helmet", Price = 199.99M },
                        new Product { Name = "Tires", Price = 149.99M }
                    }
                },
                new ShopWithProducts
                {
                    Id = 3,
                    Name = "Truck & Heavy Equipment",
                    Description = "Commercial vehicle parts and heavy machinery components. Serving construction and logistics companies.",
                    Address = "789 Industrial Blvd, Eastside",
                    Phone = "(555) 456-7890",
                    Email = "orders@truckparts.com",
                    IsActive = true,
                    ProductCount = 2100,
                    Rating = 4.9,
                    Distance = 8.7,
                    FeaturedProducts = new List<Product>
                    {
                        new Product { Name = "Hydraulic Pump", Price = 450.00M },
                        new Product { Name = "Transmission", Price = 1200.00M },
                        new Product { Name = "Axle Assembly", Price = 850.00M }
                    }
                },
                new ShopWithProducts
                {
                    Id = 4,
                    Name = "Classic Car Restoration",
                    Description = "Specialized parts for vintage and classic cars. Authentic restoration components and rare finds.",
                    Address = "321 Vintage Road, Old Town",
                    Phone = "(555) 321-0987",
                    Email = "classic@restoration.com",
                    IsActive = true,
                    ProductCount = 650,
                    Rating = 4.7,
                    Distance = 12.4,
                    FeaturedProducts = new List<Product>
                    {
                        new Product { Name = "Chrome Bumper", Price = 299.99M },
                        new Product { Name = "Carburetor", Price = 189.99M },
                        new Product { Name = "Vintage Tires", Price = 89.99M }
                    }
                },
                new ShopWithProducts
                {
                    Id = 5,
                    Name = "Marine & Boat Parts",
                    Description = "Everything for your boat and marine equipment. From engines to navigation systems.",
                    Address = "654 Harbor View, Waterfront",
                    Phone = "(555) 654-3210",
                    Email = "marine@boatparts.com",
                    IsActive = false,
                    ProductCount = 420,
                    Rating = 4.4,
                    Distance = 15.2,
                    FeaturedProducts = new List<Product>
                    {
                        new Product { Name = "Propeller", Price = 199.99M },
                        new Product { Name = "Bilge Pump", Price = 79.99M },
                        new Product { Name = "GPS System", Price = 399.99M }
                    }
                }
            };

            ShopsCollection.ItemsSource = Shops;
        }
    }

    // Helper class to combine Shop with its products
    public class ShopWithProducts : spareParts.Models.Shop
    {
        public int ProductCount { get; set; }
        public double Rating { get; set; }
        public double Distance { get; set; }
        public List<Product> FeaturedProducts { get; set; } = new List<Product>();
    }
}
