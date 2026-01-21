using System;
using CommunityToolkit.Mvvm.ComponentModel;
using spareParts.Services;
using System.Collections.ObjectModel;
using spareParts.Models;
using CommunityToolkit.Mvvm.Input;

namespace spareParts.ViewModels.Customer;

public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string CurrentUserName {get; set;}
    [ObservableProperty]
    public partial string CurrentUserEmail {get; set;}
    private AuthenticationStateService _authService;
    public ObservableCollection<Order> Orders { get; set; }

    public ProfileViewModel()
    {
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await LoadUserData();
        });
    }

    public async Task LoadUserData()
    {
        await AuthenticationStateService.Instance.InitializeAsync();
        _authService = AuthenticationStateService.Instance;
        CurrentUserName = _authService.CurrentUserName;
        CurrentUserEmail = _authService.CurrentUserEmail;
    }
    [RelayCommand]
    public async Task Logout()
    {
        await _authService.Logout();
        Shell.Current.DisplayAlert("Signed Out", "You have been successfully signed out.", "OK");
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Application.Current.MainPage = new AppShell();
        });
    }
}
