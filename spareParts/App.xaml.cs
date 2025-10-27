
using spareParts.PageViews;
using spareParts.PageViews.Authentication;
using spareParts.Services;

namespace spareParts;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        SetInitialPage();
    }

    public void SetInitialPage()
    {
        var authService = AuthenticationStateService.Instance;
        
        if (authService.IsAuthenticated)
        {
            // User is already logged in, show the main app with bottom tabs
            MainPage = new AppShellWithBottomTabs();
        }
        else
        {
            // User is not logged in, show login page
            MainPage = new NavigationPage(new LoginPage());
        }
    }

    public void NavigateToMainApp()
    {
        // Called after successful login to navigate to the main app
        MainPage = new AppShellWithBottomTabs();
    }
}
