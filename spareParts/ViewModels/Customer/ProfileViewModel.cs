using System;
using CommunityToolkit.Mvvm.ComponentModel;
using spareParts.Services;
using System.Collections.ObjectModel;
using spareParts.Models;

namespace spareParts.ViewModels.Customer;

public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string CurrentUserName {get; set;}
    [ObservableProperty]
    public partial string CurrentUserEmail {get; set;}
    // [ObservableProperty]
    // public partial string CurrentUserName {get; set;}
    // [ObservableProperty]
    // public partial string CurrentUserName {get; set;}
    // [ObservableProperty]
    // public partial string CurrentUserName {get; set;}
    // [ObservableProperty]
    // public partial string CurrentUserName {get; set;}
    private AuthenticationStateService _authService;
    public ObservableCollection<Order> Orders { get; set; }

    public ProfileViewModel()
    {
        Orders = new ObservableCollection<Order>();
        
        // LoadUserData();
        // LoadSampleOrders();
    }



    // protected override void OnAppearing()
    // {
    //     base.OnAppearing();
    //     LoadUserData();
    // }

    // private async void LoadUserData()
    // {
    //     await AuthenticationStateService.Instance.InitializeAsync();
    //     _authService = AuthenticationStateService.Instance;
    //     if (_authService.IsAuthenticated)
    //     {
    //         NameEntry.Text = _authService.CurrentUserName;
    //         EmailEntry.Text = _authService.CurrentUserEmail;
    //         PhoneEntry.Text = "555-0123"; // Sample data
    //         AddressEntry.Text = "123 Main St, Anytown, USA"; // Sample data
    //     }
    //     else
    //     {
    //         NameEntry.Text = "";
    //         EmailEntry.Text = "";
    //         PhoneEntry.Text = "";
    //         AddressEntry.Text = "";
    //     }
    // }

    // private void LoadSampleOrders()
    // {
    //     Orders.Clear();
    //     if (_authService.IsAuthenticated)
    //     {
    //         Orders.Add(new Order
    //         {
    //             Id = 1,
    //             OrderNumber = "ORD-001",
    //             Total = 125.99M,
    //             OrderDate = DateTime.Now.AddDays(-5),
    //             Status = "Delivered"
    //         });

    //         Orders.Add(new Order
    //         {
    //             Id = 2,
    //             OrderNumber = "ORD-002",
    //             Total = 89.50M,
    //             OrderDate = DateTime.Now.AddDays(-12),
    //             Status = "Delivered"
    //         });

    //         Orders.Add(new Order
    //         {
    //             Id = 3,
    //             OrderNumber = "ORD-003",
    //             Total = 245.75M,
    //             OrderDate = DateTime.Now.AddDays(-20),
    //             Status = "Delivered"
    //         });
    //     }

    //     OrdersCollection.ItemsSource = Orders;
    // }

    // private async void OnSignOutClicked(object sender, EventArgs e)
    // {
    //     _authService.Logout();
    //     await DisplayAlert("Signed Out", "You have been successfully signed out.", "OK");
    //     await NavigationService.GoTo("LoginPage");
    // }
}
