using Prism.Ioc;
using Prism.Modularity;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Views;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Services;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT
{
    public class QuanLyTapHSCTModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ITapHSCTCRUDService, TapHSCTCRUDService>();

            containerRegistry.RegisterForNavigation<KhoiTaoCacTapHSCTView>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}