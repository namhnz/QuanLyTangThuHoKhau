using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCTMoi.ViewModels
{
    public class TimKiemTuiHSCTViewModel: BindableBase
    {
        #region Tim kiem

        private int _soHSCTRutGonCanTim;

        public int SoHSCTRutGonCanTim
        {
            get => _soHSCTRutGonCanTim;
            set => SetProperty(ref _soHSCTRutGonCanTim, value);
        }

        #endregion

        #region Hien thi ket qua tim kiem

        private string _ketQuaSoHSCTDayDu;

        public string KetQuaSoHSCTDayDu
        {
            get => _ketQuaSoHSCTDayDu;
            set => SetProperty(ref _ketQuaSoHSCTDayDu, value);
        }

        private string _ketQuaDiaChiHoThuongTru;

        public string KetQuaDiaChiHoThuongTru
        {
            get => _ketQuaDiaChiHoThuongTru;
            set => SetProperty(ref _ketQuaDiaChiHoThuongTru, value);
        }

        private int _ketQuaThuTuTapHSCT;

        public int KetQuaThuTuTapHSCT
        {
            get => _ketQuaThuTuTapHSCT;
            set => SetProperty(ref _ketQuaThuTuTapHSCT, value);
        }

        private int _ketQuaViTriTuiHSCT;

        public int KetQuaViTriTuiHSCT
        {
            get => _ketQuaViTriTuiHSCT;
            set => SetProperty(ref _ketQuaViTriTuiHSCT, value);
        }

        private string _ketQuaHoTenChuHo;

        public string KetQuaHoTenChuHo
        {
            get => _ketQuaHoTenChuHo;
            set => SetProperty(ref _ketQuaHoTenChuHo, value);
        }

        private DateTime _ketQuaNgayDangKy;

        public DateTime KetQuaNgayDangKy
        {
            get => _ketQuaNgayDangKy;
            set => SetProperty(ref _ketQuaNgayDangKy, value);
        }

        #endregion

        #region Khoi tao

        public ICommand TimKiemThongTinHSCTCommand { get; private set; }

        private void TimKiemThongTinHSCT(int soHSCTRutGonCanTim)
        {

        }

        private void InitCommands()
        {
            TimKiemThongTinHSCTCommand = new DelegateCommand<int>(TimKiemThongTinHSCT);
        }

        #endregion

        public TimKiemTuiHSCTViewModel()
        {
            
        }
    }
}