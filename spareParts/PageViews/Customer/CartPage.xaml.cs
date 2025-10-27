using Microsoft.Maui.Controls;
using spareParts.Services;
using spareParts.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace spareParts.PageViews.Customer
{
    public partial class CartPage : ContentPage, INotifyPropertyChanged
    {
        private readonly AuthenticationStateService _authService;
        public ObservableCollection<CartItem> CartItems { get; set; }
        
        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public CartPage()
        {
            InitializeComponent();
            _authService = AuthenticationStateService.Instance;
            CartItems = new ObservableCollection<CartItem>();
            
            BindingContext = this;
            CartItemsCollection.ItemsSource = CartItems;
            
            // Set up event handlers
            GoToHomeButton.Clicked += OnGoToHomeClicked;
            
            // Subscribe to authentication state changes
            _authService.PropertyChanged += OnAuthenticationStateChanged;
            
            LoadSampleCartItems();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadSampleCartItems();
        }

        private void OnAuthenticationStateChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AuthenticationStateService.IsAuthenticated))
            {
                LoadSampleCartItems();
            }
        }

        private void LoadSampleCartItems()
        {
            CartItems.Clear();
            
            if (_authService.IsAuthenticated)
            {
                CartItems.Add(new CartItem
                {
                    Id = 1,
                    ProductName = "Brake Pads",
                    Price = 45.99M,
                    Quantity = 2
                });

                CartItems.Add(new CartItem
                {
                    ProductName = "Oil Filter",
                    Price = 12.50M,
                    Quantity = 1
                });

                CartItems.Add(new CartItem
                {
                    ProductName = "Spark Plugs",
                    Price = 8.99M,
                    Quantity = 4
                });
            }

            CalculateTotal();
        }

        private void CalculateTotal()
        {
            TotalPrice = CartItems.Sum(item => item.Price * item.Quantity);
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

        public new event PropertyChangedEventHandler? PropertyChanged;

        protected new virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Helper class for cart items
    public class CartItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
