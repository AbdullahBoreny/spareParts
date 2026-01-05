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

        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("SignupPage", typeof(SignupPage));
        Routing.RegisterRoute("AddProductPage", typeof(AddProductPage));
        Routing.RegisterRoute("OrdersPage", typeof(OrdersPage));
        Routing.RegisterRoute("ShopDashboardPage", typeof(ShopDashboardPage));
        Routing.RegisterRoute("ShopDetailsPage", typeof(ShopDetailsPage));
        Routing.RegisterRoute("AppShellWithBottomTabs", typeof(AppShellWithBottomTabs));
        Routing.RegisterRoute("MainTabbedPage", typeof(MainTabbedPage));
        Routing.RegisterRoute("CartPage", typeof(CartPage));
        Routing.RegisterRoute("HomePage", typeof(HomePage));
        Routing.RegisterRoute("ProductDetails", typeof(ProductDetails));
        Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));

        CheckAuthentication();
    }

    private async void CheckAuthentication()
    {
        if (_authService.IsAuthenticated)
        {
            await NavigationService.GoTo("HomePage");
        }
        else
        {
            await NavigationService.GoTo("LoginPage");
        }
    }

    protected override void OnParentSet()
    {
        base.OnHandlerChanged();
    }
}
