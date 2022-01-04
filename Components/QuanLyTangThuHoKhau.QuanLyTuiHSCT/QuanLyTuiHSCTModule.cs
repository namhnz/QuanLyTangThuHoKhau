using Prism.Ioc;
using Prism.Modularity;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.KhoiTaoCacTuiHSCT.Views;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT
{
    public class QuanLyTuiHSCTModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ITuiHSCTCRUDService, TuiHSCTCRUDService>();

            containerRegistry.RegisterForNavigation<KhoiTaoCacTuiHSCTView>();
            containerRegistry.RegisterForNavigation<TimKiemTuiHSCTView>();
            containerRegistry.RegisterForNavigation<ThemMoiTuiHSCTView>();
            containerRegistry.RegisterForNavigation<XemCacTuiHSCTView>();

        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}