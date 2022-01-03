using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.Types.KhoiTaoDuLieuBanDau;
using QuanLyTangThuHoKhau.QuanLyThonXom.KhoiTaoCacThonXom.Views;

namespace QuanLyTangThuHoKhau.KhoiTaoDuLieuBanDau.ViewModels
{
    public class GioiThieuKhoiTaoDuLieuBanDauViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public GioiThieuKhoiTaoDuLieuBanDauViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            
            InitCommands();
        }

        #region Khoi tao

        private void InitCommands()
        {
            BatDauKhoiTaoDuLieuCommand = new DelegateCommand(BatDauKhoiTaoDuLieu);
        }

        #endregion

        #region Dieu huong den khoi tao du lieu

        public ICommand BatDauKhoiTaoDuLieuCommand { get; private set; }

        private void BatDauKhoiTaoDuLieu()
        {
            _regionManager.RequestNavigate(KhoiTaoDuLieuBanDauRegionNames
                .KHOI_TAO_DU_LIEU_BAN_DAU_ROOT_REGION, nameof(KhoiTaoDanhSachThonXomView));
        }

        #endregion
    }
}