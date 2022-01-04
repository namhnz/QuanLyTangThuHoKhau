using Prism.Ioc;
using Prism.Modularity;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices;
using QuanLyTangThuHoKhau.Core.DbDataSerivces;
using QuanLyTangThuHoKhau.Core.Settings;

namespace QuanLyTangThuHoKhau.Core
{
    public class CoreModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // containerRegistry.RegisterSingleton<ISettingsManager, SettingsManager>();
            containerRegistry.RegisterSingleton<ILiteDbDataService, LiteDbDataService>();
            // containerRegistry.RegisterSingleton<IQRCodeScanner, ZebraQRCodeScanner>();
            //
            // containerRegistry.RegisterSingleton<IDonViHanhChinhService, DonViHanhChinhService>();
            // containerRegistry.RegisterSingleton<IDefaultDiaGioiContainer, DefaultDiaGioiContainer>();

            // containerRegistry.RegisterSingleton<ISettingsManager, SettingsManager>();
            containerRegistry.RegisterSingleton<IDonViHanhChinhService, DonViHanhChinhService>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }
    }
}