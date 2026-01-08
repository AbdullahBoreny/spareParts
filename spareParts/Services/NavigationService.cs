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
        else
        {
            var window = Application.Current?.Windows.FirstOrDefault();
            if (window?.Page != null)
            {
                if (route == "SignUp")
                {
                    await window.Page.Navigation.PushAsync(new PageViews.Authentication.SignupPage());
                }
            }
            else 
            {
                System.Diagnostics.Debug.WriteLine("Navigation failed: No Window or Shell found.");
            }
        }
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
