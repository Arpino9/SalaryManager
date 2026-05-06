using Prism.Ioc;
using SalaryManager.Prism.Views;
using System.Windows;

namespace SalaryManager.Prism;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<Prism.Views.Career>();
        containerRegistry.RegisterForNavigation<Prism.Views.Company>();
        containerRegistry.RegisterForNavigation<Prism.Views.Holiday>();
        containerRegistry.RegisterForNavigation<Prism.Views.Home>();
    }
}
