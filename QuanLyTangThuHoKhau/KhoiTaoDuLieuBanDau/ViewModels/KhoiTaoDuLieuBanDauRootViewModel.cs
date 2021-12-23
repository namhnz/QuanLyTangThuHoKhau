using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.Types.KhoiTaoDuLieuBanDau;
using QuanLyTangThuHoKhau.QuanLyThonXom.KhoiTaoCacThonXom.Views;

namespace QuanLyTangThuHoKhau.KhoiTaoDuLieuBanDau.ViewModels
{
    public class KhoiTaoDuLieuBanDauRootViewModel: BindableBase
    {
        private readonly IRegionManager _regionManager;
        public KhoiTaoDuLieuBanDauRootViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            InitData();
        }

        private void InitData()
        {
            _regionManager.RegisterViewWithRegion<KhoiTaoDanhSachThonXomView>(KhoiTaoDuLieuBanDauRegionNames
                .KHOI_TAO_DU_LIEU_BAN_DAU_ROOT_REGION);
        }
    }
}