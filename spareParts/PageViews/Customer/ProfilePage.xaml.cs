using Microsoft.Maui.Controls;
using spareParts.Services;
using spareParts.Models;
using System.Collections.ObjectModel;
using spareParts.PageViews.Authentication;
using spareParts.ViewModels.Customer;

namespace spareParts.PageViews.Customer
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel();
        }
    }
}
