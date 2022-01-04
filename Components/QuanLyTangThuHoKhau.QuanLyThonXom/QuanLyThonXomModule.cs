using Prism.Ioc;
using Prism.Modularity;
using QuanLyTangThuHoKhau.QuanLyThonXom.KhoiTaoCacThonXom.Views;
using QuanLyTangThuHoKhau.QuanLyThonXom.Services;

namespace QuanLyTangThuHoKhau.QuanLyThonXom
{
    public class QuanLyThonXomModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // containerRegistry.Register<QRCodeStringToTheCCCDConverter>();
            containerRegistry.RegisterSingleton<IThonXomCRUDService, ThonXomCRUDService>();

            // containerRegistry.RegisterScoped<IXuLyThongTinTheModuleCommands, XuLyThongTinTheModuleCommands>();

            containerRegistry.RegisterForNavigation<KhoiTaoDanhSachThonXomView>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // var regionManager = containerProvider.Resolve<IRegionManager>();
            // regionManager.RegisterViewWithRegion(MainWindowRegionNames.MAIN_MENU_REGION, typeof(OptionTheCCCDMenuItem));
            //
            // //Khoi tao view dau tien cho Main content view region
            // regionManager.RequestNavigate(MainWindowRegionNames.MAIN_VIEW_CONTENT_REGION,
            //     nameof(XuLyThongTinTheMainView));
        }
    }
}