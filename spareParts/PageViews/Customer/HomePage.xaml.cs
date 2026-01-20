using spareParts.ViewModels.Customer;

namespace spareParts.PageViews.Customer
{
    public partial class HomePage : ContentPage
    {        
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }
    }
}
