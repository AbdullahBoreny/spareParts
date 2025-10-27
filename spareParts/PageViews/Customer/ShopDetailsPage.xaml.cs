using Microsoft.Maui.Controls;

namespace spareParts.PageViews.Customer
{
    public partial class ShopDetailsPage : ContentPage
    {
        private ShopWithProducts? _currentShop;

        public ShopDetailsPage()
        {
            InitializeComponent();
        }

        public ShopDetailsPage(ShopWithProducts shop) : this()
        {
            _currentShop = shop;
            BindingContext = shop;
        }

        private async void OnCallClicked(object sender, EventArgs e)
        {
            if (_currentShop != null)
            {
                await DisplayAlert("Call Shop", $"Calling {_currentShop.Phone}", "OK");
            }
        }

        private async void OnEmailClicked(object sender, EventArgs e)
        {
            if (_currentShop != null)
            {
                await DisplayAlert("Email Shop", $"Opening email to {_currentShop.Email}", "OK");
            }
        }

        private async void OnMapClicked(object sender, EventArgs e)
        {
            if (_currentShop != null)
            {
                await DisplayAlert("Directions", $"Opening map to: {_currentShop.Address}", "OK");
            }
        }

        private async void OnBrowseProductsClicked(object sender, EventArgs e)
        {
            if (_currentShop != null)
            {
                await DisplayAlert("Browse Products", $"Browsing all products from {_currentShop.Name}", "OK");
            }
        }

        private async void OnVisitShopClicked(object sender, EventArgs e)
        {
            if (_currentShop != null)
            {
                await DisplayAlert("Visit Shop", $"Visiting {_currentShop.Name} at {_currentShop.Address}", "OK");
            }
        }
    }
}
