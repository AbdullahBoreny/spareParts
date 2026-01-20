using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace spareParts.ViewModels.Authentication;

public partial class ShopRegistrationViewModel : ObservableObject
{
    [ObservableProperty]
    private SignupRequest _basicInfo;

    [ObservableProperty]
    private string _shopName;

    [ObservableProperty]
    private string _shopAddress;

    [ObservableProperty]
    private string _shopDescription;

    [RelayCommand]
    private async Task CompleteRegistration()
    {
        
    }
}
