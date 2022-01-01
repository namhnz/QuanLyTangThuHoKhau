using System;
using System.Windows;
using System.Windows.Input;
using log4net;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.Exceptions;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Exceptions.TimKiemTuiHSCTExceptions;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.ViewModels
{
    public class TimKiemTuiHSCTViewModel : BindableBase, INavigationAware
    {
        private readonly ITuiHSCTCRUDService _tuiHSCTService;

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TimKiemTuiHSCTViewModel(ITuiHSCTCRUDService tuiHSCTService)
        {
            _tuiHSCTService = tuiHSCTService;
            InitCommands();
            InitData();
        }

        #region Tim kiem

        private int _soHSCTRutGonCanTim;

        public int SoHSCTRutGonCanTim
        {
            get => _soHSCTRutGonCanTim;
            set => SetProperty(ref _soHSCTRutGonCanTim, value);
        }

        #endregion

        #region Hien thi template ket qua

        private LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT _dataTemplateHienThiKetQuaTimKiem;

        public LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT DataTemplateHienThiKetQuaTimKiem
        {
            get => _dataTemplateHienThiKetQuaTimKiem;
            set => SetProperty(ref _dataTemplateHienThiKetQuaTimKiem, value);
        }

        #endregion

        #region Hien thi ket qua tim kiem

        private string _ketQuaSoHSCTDayDu;

        public string KetQuaSoHSCTDayDu
        {
            get => _ketQuaSoHSCTDayDu;
            set => SetProperty(ref _ketQuaSoHSCTDayDu, value);
        }

        private ThonXom _ketQuaDiaChiHoThuongTru;

        public ThonXom KetQuaDiaChiHoThuongTru
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

        private DateTime? _ketQuaNgayDangKy;

        public DateTime? KetQuaNgayDangKy
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

        private async void TimKiemThongTinHSCT()
        {
            ErrorText = null;

            try
            {
                //Kiem tra dieu kien
                if (SoHSCTRutGonCanTim <= 0)
                {
                    DataTemplateHienThiKetQuaTimKiem = LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT
                        .ChuaNhapSoHSCTDeTimKiemDataTemplate;

                    throw new SoHSCTKhongDungException()
                    {
                        ErrorMessage = "Số HSCT cần tìm không đúng"
                    };
                }

                //Lay thong tin ho so
                var ketQuaTuiHSCT = await _tuiHSCTService.TimKiemTuiHSCTTheoSoHSCT(SoHSCTRutGonCanTim);
                if (ketQuaTuiHSCT == null)
                {
                    // throw new SoHSCTKhongDungException()
                    // {
                    //     ErrorMessage = "Không tìm thấy hộ thường trú nào có số HSCT như trên"
                    // };
                    DataTemplateHienThiKetQuaTimKiem =
                        LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT.KhongTimThayTuiHSCTDataTemplate;
                    return;
                }

                var ketQuaTapHSCT = ketQuaTuiHSCT.TapHSCT;

                //Hien thi cac gia tri
                KetQuaSoHSCTDayDu = ketQuaTuiHSCT.HSCT.MaHSCTDayDu;
                KetQuaDiaChiHoThuongTru = ketQuaTapHSCT.ThonXom;
                KetQuaThuTuTapHSCT = ketQuaTapHSCT.ThuTuTapHSCT;
                KetQuaViTriTuiHSCT = ketQuaTuiHSCT.ViTriTui;
                KetQuaHoTenChuHo = ketQuaTuiHSCT.HSCT.ChuHo;
                KetQuaNgayDangKy = ketQuaTuiHSCT.HSCT.NgayDangKy;

                //Thay doi template hien thi
                DataTemplateHienThiKetQuaTimKiem = LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT
                    .HienThiThongTinTuiHSCTTimThayDataTemplate;
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
            TimKiemThongTinHSCTCommand = new DelegateCommand(TimKiemThongTinHSCT);
        }

        private void InitData()
        {
            DataTemplateHienThiKetQuaTimKiem =
                LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT.ChuaNhapSoHSCTDeTimKiemDataTemplate;
        }

        #endregion

        #region Dieu huong

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["TuiHSCTCanHienThiChiTiet"] is TuiHSCT tuiHSCTTruyenTuViewKhac)
            {
                SoHSCTRutGonCanTim = tuiHSCTTruyenTuViewKhac.HSCT.SoHSCT;
                TimKiemThongTinHSCT();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // throw new NotImplementedException();
        }

        #endregion
    }
}