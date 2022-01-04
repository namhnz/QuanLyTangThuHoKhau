using System.Windows;
using CustomMVVMDialogs;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using QuanLyTangThuHoKhau.Core;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices;
using QuanLyTangThuHoKhau.Core.Settings;
using QuanLyTangThuHoKhau.KhoiTaoDuLieuBanDau.Views;
using QuanLyTangThuHoKhau.MenuPart;
using QuanLyTangThuHoKhau.QuanLyTapHSCT;
using QuanLyTangThuHoKhau.QuanLyThaoTacDuLieu.Views;
using QuanLyTangThuHoKhau.QuanLyThonXom;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services;

namespace QuanLyTangThuHoKhau
{
    public class Bootstrapper : PrismBootstrapper
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
            //
            // containerRegistry.RegisterSingleton<IBusyMonitor, BusyMonitor>();

            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterSingleton<ISettingsManager, SettingsManager>();
            // containerRegistry.RegisterSingleton<IDonViHanhChinhService, DonViHanhChinhService>();

            //Khoi tao cac view de dieu huong
            containerRegistry.RegisterForNavigation<KhoiTaoDuLieuBanDauRootView>();
            containerRegistry.RegisterForNavigation<QuanLyThaoTacDuLieuRootView>();

            containerRegistry.RegisterForNavigation<GioiThieuKhoiTaoDuLieuBanDauView>();

            // containerRegistry.RegisterSingleton<ITuiHSCTCRUDService, TuiHSCTCRUDServiceSampleData>();

        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<QuanLyThonXomModule>();
            moduleCatalog.AddModule<QuanLyTapHSCTModule>();
            moduleCatalog.AddModule<QuanLyTuiHSCTModule>();
            moduleCatalog.AddModule<MenuPartModule>();
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