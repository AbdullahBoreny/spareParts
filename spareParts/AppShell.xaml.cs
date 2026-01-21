using spareParts.PageViews;
using spareParts.PageViews.Authentication;
using spareParts.PageViews.Customer;
using spareParts.PageViews.Shop;
using spareParts.Services;

namespace spareParts;

public partial class AppShell : Shell
{
    private readonly AuthenticationStateService _authService = AuthenticationStateService.Instance;
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("AddProductPage", typeof(AddProductPage));
        Routing.RegisterRoute("OrdersPage", typeof(OrdersPage));
        Routing.RegisterRoute("ShopDashboardPage", typeof(ShopDashboardPage));
        Routing.RegisterRoute("ShopDetailsPage", typeof(ShopDetailsPage));
        Routing.RegisterRoute("MainTabbedPage", typeof(MainTabbedPage));
        Routing.RegisterRoute("CartPage", typeof(CartPage));
        Routing.RegisterRoute("HomePage", typeof(HomePage));
        Routing.RegisterRoute("ProductDetails", typeof(ProductDetails));
        Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
        Routing.RegisterRoute("ShopRegistrationPage", typeof(ShopRegistrationPage));

        CheckAuthentication();
    }

    private async void CheckAuthentication()
    {
        await AuthenticationStateService.Instance.InitializeAsync();
        if (_authService.IsAuthenticated)
        {
            Application.Current.MainPage = new AppShellWithBottomTabs();
        }
        else
        {
            await NavigationService.SetRoot("LoginPage");
        }
    }
}
