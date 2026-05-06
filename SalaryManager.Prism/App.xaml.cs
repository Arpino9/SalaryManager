using Prism.Ioc;
using SalaryManager.Prism.Views;
using System.Windows;
using Career = SalaryManager.Prism.Views.Career;

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
        containerRegistry.RegisterForNavigation<Career>();
    }
}
