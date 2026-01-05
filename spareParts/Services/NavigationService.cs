using System;

namespace spareParts.Services;

public partial class NavigationService
{
    public static async Task GoTo(string route)
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync(route);
        }
        // 2. Fallback: If Shell is null, we are likely on the Login/Signup screen
        else
        {
            // Get the current window safely
            var window = Application.Current?.Windows.FirstOrDefault();
            if (window?.Page != null)
            {
                if (route == "SignUp")
                {
                    // This pushes the Signup page onto the Login stack
                    await window.Page.Navigation.PushAsync(new PageViews.Authentication.SignupPage());
                }
            }
            else 
            {
                // This is a last resort to catch errors during startup
                System.Diagnostics.Debug.WriteLine("Navigation failed: No Window or Shell found.");
            }
        }
        //await Shell.Current.GoToAsync(route);
    }

    public static async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    public static void SetRoot(Page name)
    {
        var window = Application.Current?.Windows.FirstOrDefault();
        if (window != null)
        {
             window.Page = name;
        }
    }
}
