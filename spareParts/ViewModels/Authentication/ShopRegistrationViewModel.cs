using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace spareParts.ViewModels.Authentication;

public partial class ShopRegistrationViewModel : ObservableObject
{
    [ObservableProperty]
    public partial SignupRequest _basicInfo {get; set;}

    [ObservableProperty]
    public partial string ShopName {get; set;}

    [ObservableProperty]
    public partial string ShopAddress {get; set;}

    [ObservableProperty]
    public  partial string ShopDescription {get; set;}

    public ShopRegistrationViewModel(Dictionary<string, object> keyValuePairs) 
    {
        
    }

    [RelayCommand]
    public async Task CompleteRegistration()
    {
        
    }
}
