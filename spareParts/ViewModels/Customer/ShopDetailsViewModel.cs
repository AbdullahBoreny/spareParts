using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using spareParts.Models;

namespace spareParts.ViewModels.Customer;

public partial class ShopDetailsViewModel : ObservableObject
{
        private ShopWithProducts _currentShop;


        public ShopDetailsViewModel(ShopWithProducts shop)
        {
            _currentShop = shop;
        }

        [RelayCommand]
        private async Task ShowPhone()
        {
            if (_currentShop != null)
            {
                await Shell.Current.DisplayAlert("Call Shop", $"Calling {_currentShop.Phone}", "OK");
            }
        }

        [RelayCommand]
        private async Task ShowEmail()
        {
            if (_currentShop != null)
            {
                await Shell.Current.DisplayAlert("Email Shop", $"Opening email to {_currentShop.Email}", "OK");
            }
        }

        [RelayCommand]
        private async Task ShowMap()
        {
            if (_currentShop != null)
            {
                await Shell.Current.DisplayAlert("Directions", $"Opening map to: {_currentShop.Address}", "OK");
            }
        }

        [RelayCommand]
        private async Task BrowseProducts()
        {
            if (_currentShop != null)
            {
                await Shell.Current.DisplayAlert("Browse Products", $"Browsing all products from {_currentShop.Name}", "OK");
            }
        }

        [RelayCommand]
        private async Task VisitShop()
        {
            if (_currentShop != null)
            {
                await Shell.Current.DisplayAlert("Visit Shop", $"Visiting {_currentShop.Name} at {_currentShop.Address}", "OK");
            }
        }
}
