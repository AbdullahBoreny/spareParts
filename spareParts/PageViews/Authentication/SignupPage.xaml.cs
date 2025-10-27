using Microsoft.Maui.Controls;
using spareParts.ViewModels.Authentication;

namespace spareParts.PageViews.Authentication
{
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
            BindingContext = new SignupViewModel();
        }
    }
}
