
using spareParts.PageViews;
using spareParts.PageViews.Authentication;
using spareParts.Services;

namespace spareParts;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }
    protected override Window CreateWindow(IActivationState activationState)
    {
        return new Window(new AppShell());
    }
}