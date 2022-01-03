using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.Types.KhoiTaoDuLieuBanDau;
using QuanLyTangThuHoKhau.KhoiTaoDuLieuBanDau.Views;
using QuanLyTangThuHoKhau.QuanLyThonXom.KhoiTaoCacThonXom.Views;

namespace QuanLyTangThuHoKhau.KhoiTaoDuLieuBanDau.ViewModels
{
    public class KhoiTaoDuLieuBanDauRootViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public KhoiTaoDuLieuBanDauRootViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            InitData();
            InitCommands();
        }

        #region Khoi tao

        private void InitData()
        {
        }

        private void InitCommands()
        {
            HienThiViewGioiThieuKhoiTaoDuLieuCommand = new DelegateCommand(HienThiViewGioiThieuKhoiTaoDuLieu);
        }

        #endregion

        #region Hien thi view gioi thieu khoi tao du lieu ban dau

        public ICommand HienThiViewGioiThieuKhoiTaoDuLieuCommand { get; private set; }

        private void HienThiViewGioiThieuKhoiTaoDuLieu()
        {
            _regionManager.RequestNavigate(KhoiTaoDuLieuBanDauRegionNames
                .KHOI_TAO_DU_LIEU_BAN_DAU_ROOT_REGION, nameof(GioiThieuKhoiTaoDuLieuBanDauView));
        }

        #endregion
    }
}