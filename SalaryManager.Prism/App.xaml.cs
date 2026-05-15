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
        containerRegistry.RegisterDialogWindow<Prism.Views.MetroDialogWindow>();
        containerRegistry.RegisterDialog<Prism.Views.Career, Prism.ViewModels.CareerViewModel>();
        containerRegistry.RegisterDialog<Prism.Views.Company, Prism.ViewModels.CompanyViewModel>();
        containerRegistry.RegisterDialog<Prism.Views.Holiday, Prism.ViewModels.HolidayViewModel>();
        containerRegistry.RegisterDialog<Prism.Views.Home, Prism.ViewModels.HomeViewModel>();
        containerRegistry.RegisterDialog<Prism.Views.FileStorage, Prism.ViewModels.FileStorageViewModel>();
        containerRegistry.RegisterDialog<Prism.Views.WorkingPlace, Prism.ViewModels.WorkingPlaceViewModel>();
        containerRegistry.RegisterDialog<Prism.Views.Option, Prism.ViewModels.OptionViewModel>();
    }
}
