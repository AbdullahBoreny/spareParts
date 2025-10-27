using Microsoft.Maui.Controls;
using spareParts.Services;
using spareParts.Models;
using System.Collections.ObjectModel;

namespace spareParts.PageViews.Customer
{
    public partial class ProfilePage : ContentPage
    {
        private readonly AuthenticationStateService _authService;
        public ObservableCollection<Order> Orders { get; set; }

        public ProfilePage()
        {
            InitializeComponent();
            _authService = AuthenticationStateService.Instance;
            Orders = new ObservableCollection<Order>();
            
            // Set up event handlers
            SignOutButton.Clicked += OnSignOutClicked;
            GoToHomeButton.Clicked += OnGoToHomeClicked;
            
            // Subscribe to authentication state changes
            _authService.PropertyChanged += OnAuthenticationStateChanged;
            
            LoadUserData();
            LoadSampleOrders();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadUserData();
        }

        private void OnAuthenticationStateChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AuthenticationStateService.IsAuthenticated) ||
                e.PropertyName == nameof(AuthenticationStateService.CurrentUserName) ||
                e.PropertyName == nameof(AuthenticationStateService.CurrentUserEmail))
            {
                LoadUserData();
            }
        }

        private void LoadUserData()
        {
            if (_authService.IsAuthenticated)
            {
                NameEntry.Text = _authService.CurrentUserName;
                EmailEntry.Text = _authService.CurrentUserEmail;
                PhoneEntry.Text = "555-0123"; // Sample data
                AddressEntry.Text = "123 Main St, Anytown, USA"; // Sample data
            }
            else
            {
                NameEntry.Text = "";
                EmailEntry.Text = "";
                PhoneEntry.Text = "";
                AddressEntry.Text = "";
            }
        }

        private void LoadSampleOrders()
        {
            Orders.Clear();
            if (_authService.IsAuthenticated)
            {
                Orders.Add(new Order
                {
                    Id = 1,
                    OrderNumber = "ORD-001",
                    Total = 125.99M,
                    OrderDate = DateTime.Now.AddDays(-5),
                    Status = "Delivered"
                });

                Orders.Add(new Order
                {
                    Id = 2,
                    OrderNumber = "ORD-002",
                    Total = 89.50M,
                    OrderDate = DateTime.Now.AddDays(-12),
                    Status = "Delivered"
                });

                Orders.Add(new Order
                {
                    Id = 3,
                    OrderNumber = "ORD-003",
                    Total = 245.75M,
                    OrderDate = DateTime.Now.AddDays(-20),
                    Status = "Delivered"
                });
            }

            OrdersCollection.ItemsSource = Orders;
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            _authService.Logout();
            await DisplayAlert("Signed Out", "You have been successfully signed out.", "OK");
            
            // Navigate back to login page
            if (Application.Current is App app)
            {
                app.SetInitialPage();
            }
        }

        private async void OnGoToHomeClicked(object sender, EventArgs e)
        {
            // Navigate to Home tab using Shell
            await Shell.Current.GoToAsync("//Home");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Unsubscribe from events to prevent memory leaks
            _authService.PropertyChanged -= OnAuthenticationStateChanged;
        }
    }
}
