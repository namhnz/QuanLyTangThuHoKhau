using Prism.Ioc;
using Prism.Modularity;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT
{
    public class ThongTinChinhSuaHSCTModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // containerRegistry.RegisterSingleton<ITuiHSCTCRUDService, TuiHSCTCRUDService>();
            //
            // containerRegistry.RegisterForNavigation<KhoiTaoCacTuiHSCTView>();
            // containerRegistry.RegisterForNavigation<TimKiemTuiHSCTView>();
            // containerRegistry.RegisterForNavigation<ThemMoiTuiHSCTView>();
            // containerRegistry.RegisterForNavigation<XemCacTuiHSCTView>();

        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}