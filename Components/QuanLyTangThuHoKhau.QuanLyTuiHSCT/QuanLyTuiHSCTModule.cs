using Prism.Ioc;
using Prism.Modularity;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Views;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.KhoiTaoCacTuiHSCT.Views;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT
{
    public class QuanLyTuiHSCTModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // containerRegistry.Register<QRCodeStringToTheCCCDConverter>();
            // containerRegistry.Register<ICacTheCCCDsToExcelFileExporter, CacTheCCCDsToExcelFileExporter>();
            // containerRegistry.RegisterSingleton<ITuiHSCTCRUDService, TuiHSCTCRUDService>();
            //
            // containerRegistry.RegisterSingleton<ITuiHSCTCRUDService, TuiHSCTCRUDServiceSampleData>();

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
            containerRegistry.RegisterForNavigation<KhoiTaoCacTuiHSCTView>();
            containerRegistry.RegisterForNavigation<TimKiemTuiHSCTView>();
            containerRegistry.RegisterForNavigation<ThemMoiTuiHSCTView>();
            containerRegistry.RegisterForNavigation<XemCacTuiHSCTView>();
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