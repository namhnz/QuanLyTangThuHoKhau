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
            // containerRegistry.Register<ICacTheCCCDsToExcelFileExporter, CacTheCCCDsToExcelFileExporter>();
            // containerRegistry.RegisterSingleton<IThonXomCRUDService, ThonXomCRUDService>();
            containerRegistry.RegisterSingleton<IThonXomCRUDService, ThonXomCRUDServiceSampleData>();


            //
            // containerRegistry.RegisterSingleton<ICommandBarCompositeCommands, CommandBarCompositeCommands>();
            // containerRegistry.RegisterSingleton<ICommandBarUIState, CommandBarUIState>();
            //
            // containerRegistry.RegisterScoped<IXuLyThongTinTheModuleCommands, XuLyThongTinTheModuleCommands>();
            // containerRegistry.RegisterSingleton<HoTenVnSpellChecker>();
            // containerRegistry.RegisterSingleton<GioiTinhChecker>();
            // containerRegistry.RegisterSingleton<AlreadyExistInDbChecker>();
            //
            // containerRegistry.RegisterSingleton<IXuLyTheCCCDReposService, XuLyTheCCCDReposService>();
            //
            // containerRegistry.RegisterForNavigation<XuLyThongTinTheMainView>();
            // containerRegistry.RegisterForNavigation<XemNoiDungTextFileOverlayView>();
            //
            containerRegistry.RegisterForNavigation<KhoiTaoDanhSachThonXomView>();
            // containerRegistry.RegisterForNavigation<KhoiTaoCacTapHSCTView>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // var regionManager = containerProvider.Resolve<IRegionManager>();
            // regionManager.RegisterViewWithRegion(MainWindowRegionNames.MAIN_MENU_REGION, typeof(OptionTheCCCDMenuItem));
            //
            // regionManager.RegisterViewWithRegion(XuLyThongTinTheRegionNames.DANH_SACH_VIEW_REGION, typeof(KetQuaPhanTichTextFileView));
            //
            // //Khoi tao view dau tien cho Main content view region
            // regionManager.RequestNavigate(MainWindowRegionNames.MAIN_VIEW_CONTENT_REGION,
            //     nameof(XuLyThongTinTheMainView));
        }
    }
}