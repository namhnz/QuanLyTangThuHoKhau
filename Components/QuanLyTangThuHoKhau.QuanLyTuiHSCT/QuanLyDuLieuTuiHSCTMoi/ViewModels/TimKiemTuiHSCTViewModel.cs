using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using log4net;
using Prism.Commands;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.Exceptions;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Exceptions.TimKiemTuiHSCTExceptions;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCTMoi.ViewModels
{
    public class TimKiemTuiHSCTViewModel : BindableBase
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        #region Hien thi loi

        private string _errorText;

        public string ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }

        #endregion

        #region Khoi tao

        public ICommand TimKiemThongTinHSCTCommand { get; private set; }

        private void TimKiemThongTinHSCT(int soHSCTRutGonCanTim)
        {
            try
            {
                //Kiem tra dieu kien
                if (soHSCTRutGonCanTim <= 0)
                {
                    throw new SoHSCTKhongDungException()
                    {
                        ErrorMessage = "Số HSCT cần tìm không đúng"
                    };
                }

                //Lay thong tin ho so
                TuiHSCT ketQuaTuiHSCT;
                TapHSCT ketQuaTapHSCT;

                //Hien thi cac gia tri
                KetQuaSoHSCTDayDu = ketQuaTuiHSCT.HSCT.MaHSCTDayDu;
                KetQuaDiaChiHoThuongTru = ketQuaTapHSCT.ThonXom.ToString();
                KetQuaThuTuTapHSCT = ketQuaTapHSCT.ThuTuTapHSCT;
                KetQuaViTriTuiHSCT = ketQuaTuiHSCT.ViTriTui;
                KetQuaHoTenChuHo = ketQuaTuiHSCT.HSCT.ChuHo;
                KetQuaNgayDangKy = ketQuaTuiHSCT.HSCT.NgayDangKy;
            }
            catch (Exception ex)
            {
                if (ex is SoHSCTKhongDungException)
                {
                    ErrorText = ((BaseException)ex).ErrorMessage;
                }
                else
                {
                    Log.Error(ex);
                    MessageBox.Show("Đã có lỗi xảy ra khi tìm kiếm thông tin hộ thường trú");
                }
            }
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