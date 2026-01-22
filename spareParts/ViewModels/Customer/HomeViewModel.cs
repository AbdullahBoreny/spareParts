using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using spareParts.Models;
using spareParts.Services;
using System.Collections.ObjectModel;
using spareParts.PageViews.Customer;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;

namespace spareParts.ViewModels.Customer
{
    public partial class HomeViewModel : ObservableObject
    {

        public HubConnection hubConnection;
        public AuthenticationStateService _authService;

        [ObservableProperty]
        public partial ObservableCollection<ShopWithProducts> Shops {get; set;}

        [ObservableProperty]
        public partial string WelcomeMessage {get; set;}

        [ObservableProperty]
        public partial string UserEmail {get; set;}

        [ObservableProperty]
        public partial bool IsAuthenticated {get; set;}

        public ApiService apiService = new ApiService();

        public string UserID {get; set;}

        public HomeViewModel()
        {
            _authService = AuthenticationStateService.Instance;
            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await UpdateAuthState();
                LoadSampleShops();
                // await getShops();

            });
        }

        public async Task UpdateAuthState()
        {
            await AuthenticationStateService.Instance.InitializeAsync();
            WelcomeMessage = _authService.CurrentUserName + " !";
            UserEmail = _authService.CurrentUserEmail;
            UserID = _authService.CurrentUserID;
        }

        [RelayCommand]
        public async Task Logout()
        {
            bool answer = await Shell.Current.DisplayAlert("Logout", "Are you sure?", "Yes", "No");
            if (answer)
            {
                await _authService.Logout();
            }
        }
        [RelayCommand]
        public async Task ViewShop(ShopWithProducts shop)
        {
            if (shop == null) return;
            await Shell.Current.Navigation.PushAsync(new ShopDetailsPage(shop));
        }
        public void LoadSampleShops()
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
                    UserID = "72E0941C-17FB-4EDD-81D2-B2FED83004FA",
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
                    UserID = "C060063D-2620-4851-B2CB-D23BEF0ED127",
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
                    UserID = "CF070548-7E32-49ED-A0B2-94CCDBF0927F",
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
                    UserID = "EF0D8BA9-6F1A-490B-B348-6DB8834984EF",
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
                    UserID = "ECA712CC-6E41-4E84-885D-35BEEEAB798C",
                    FeaturedProducts = new List<Product>
                    {
                        new Product { Name = "Propeller", Price = 199.99M },
                        new Product { Name = "Bilge Pump", Price = 79.99M },
                        new Product { Name = "GPS System", Price = 399.99M }
                    }
                }
            };
        }

        // public async Task getShops()
        // {
        //     GetShopsRequest getShopsRequest = new GetShopsRequest()
        //     {
        //         UserID = UserID
        //     };
        //     string responseString = await apiService.PostAsync("GetShops", getShopsRequest);
        //     ShopsResponse response = System.Text.Json.JsonSerializer.Deserialize<ShopsResponse>(responseString);
        // }

        public async void OnContactShopClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ShopWithProducts shop)
            {
                var action = await Shell.Current.DisplayActionSheet(
                    "Contact Shop",
                    "Cancel",
                    null,
                    "Call",
                    "Email",
                    "Get Directions");

                switch (action)
                {
                    case "Call":
                        await Shell.Current.DisplayAlert("Call Shop", $"Calling {shop.Phone}", "OK");
                        break;
                    case "Email":
                        await Shell.Current.DisplayAlert("Email Shop", $"Email: {shop.Email}", "OK");
                        break;
                    case "Get Directions":
                        await Shell.Current.DisplayAlert("Directions", $"Navigate to: {shop.Address}", "OK");
                        break;
                }
            }
        }
        [RelayCommand]
        public async Task ContactShop(ShopWithProducts shopWithProducts)
        {
            string ShopUserID = shopWithProducts.UserID;
            string shopName = shopWithProducts.Name;
            ChatNav chatNav = new ChatNav()
            {
                ContactUserID = ShopUserID,
                ContactName = shopName
            };
            var parameters = new Dictionary<string, object>
            {
                {"ChatData" , chatNav}
            };
            await Shell.Current.GoToAsync("ChatPage", parameters);
        }
    }
    public class GetShopsRequest
    {
        public string UserID {get; set;}
    }

    public class ShopsResponse
    {
        public bool Success {get; set;}
        public string Message {get; set;}

        public ObservableCollection<Models.Shop> ShopsList {get; set;}
    }
    public class ChatNav
    {
        public string ContactUserID {get; set;}
        public string ContactName {get; set;}
    }
}