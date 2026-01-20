using Microsoft.Maui.Controls;
using spareParts.ViewModels.Customer;
using spareParts.Models;

namespace spareParts.PageViews.Customer
{
    public partial class ShopDetailsPage : ContentPage
    {
        public ShopDetailsPage()
        {
            InitializeComponent();
        }

        public ShopDetailsPage(ShopWithProducts shop) : this()
        {
            BindingContext = shop;
        }
    }
}
