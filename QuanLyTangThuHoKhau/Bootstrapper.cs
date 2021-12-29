using System.Windows;
using CustomMVVMDialogs;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using QuanLyTangThuHoKhau.Core;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices;
using QuanLyTangThuHoKhau.Core.Settings;
using QuanLyTangThuHoKhau.KhoiTaoDuLieuBanDau.Views;
using QuanLyTangThuHoKhau.QuanLyDuLieu.Views;
using QuanLyTangThuHoKhau.QuanLyTapHSCT;
using QuanLyTangThuHoKhau.QuanLyThonXom;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services;

namespace QuanLyTangThuHoKhau
{
    public class Bootstrapper : PrismBootstrapper
    {
        override protected void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
            //
            // containerRegistry.RegisterSingleton<IBusyMonitor, BusyMonitor>();

            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterSingleton<ISettingsManager, SettingsManager>();
            containerRegistry.RegisterSingleton<IDonViHanhChinhService, DonViHanhChinhService>();

            //Khoi tao cac view de dieu huong
            containerRegistry.RegisterForNavigation<KhoiTaoDuLieuBanDauRootView>();
            containerRegistry.RegisterForNavigation<QuanLyDuLieuRootView>();

            containerRegistry.RegisterSingleton<ITuiHSCTCRUDService, TuiHSCTCRUDServiceSampleData>();

        }

        override protected DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        override protected void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<QuanLyThonXomModule>();
            moduleCatalog.AddModule<QuanLyTapHSCTModule>();
            moduleCatalog.AddModule<QuanLyTuiHSCTModule>();
            // moduleCatalog.AddModule<QuetTheModule>();
            // moduleCatalog.AddModule<NhanTheModule>();
            // moduleCatalog.AddModule<CaiDatModule>();
            
        }

        // override protected void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        // {
        //     base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        //     regionAdapterMappings.RegisterMapping(typeof(HamburgerMenuItemCollection),
        //         Container.Resolve<HamburgerMenuItemCollectionRegionAdapter>());
        // }

        // protected override void InitializeModules()
        // {
        //     var splashScreen = new AppSplashScreen();
        //     splashScreen.Show();
        //     try
        //     {
        //         base.InitializeModules();
        //     }
        //     finally
        //     {
        //         splashScreen.CloseSplashScreen();
        //     }
        // }
    }
}