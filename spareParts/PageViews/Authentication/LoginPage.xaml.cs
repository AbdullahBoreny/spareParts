using Microsoft.Maui.Controls;
using spareParts.ViewModels.Authentication;

namespace spareParts.PageViews.Authentication
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}
