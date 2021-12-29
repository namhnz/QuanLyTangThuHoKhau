using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using log4net;
using Prism.Commands;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Exceptions;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Services;
using QuanLyTangThuHoKhau.QuanLyThonXom.Services;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.ViewModels
{
    public class ThemMoiTuiHSCTViewModel: BindableBase
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ThemMoiTuiHSCTViewModel(IThonXomCRUDService thonXomService,ITapHSCTCRUDService tapHSCTService, ITuiHSCTCRUDService tuiHSCTService)
        {
            _thonXomService = thonXomService;
            _tapHSCTService = tapHSCTService;
            _tuiHSCTService = tuiHSCTService;

            InitData();
            InitCommands();

        }

        private List<ThonXom> _danhSachThonXom;

        public List<ThonXom> DanhSachThonXom
        {
            get => _danhSachThonXom;
            set => SetProperty(ref _danhSachThonXom, value);
        }

        private ThonXom _selectedThonXomChuaTuiHSCT;

        public ThonXom SelectedThonXomChuaTuiHSCT
        {
            get => _selectedThonXomChuaTuiHSCT;
            set => SetProperty(ref _selectedThonXomChuaTuiHSCT, value);
        }

        private string _hoTenChuHo;

        public string HoTenChuHo
        {
            get => _hoTenChuHo;
            set => SetProperty(ref _hoTenChuHo, value);
        }

        private DateTime _ngayDangKy;

        public DateTime NgayDangKy
        {
            get => _ngayDangKy;
            set => SetProperty(ref _ngayDangKy, value);
        }

        private string _errorText;

        public string ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }

        #region Cac phu thuoc

        private readonly IThonXomCRUDService _thonXomService;
        private readonly ITapHSCTCRUDService _tapHSCTService;
        private readonly ITuiHSCTCRUDService _tuiHSCTService;

        #endregion

        #region Khoi tao

        private void InitCommands()
        {
            ThemMoiTuiHSCTCommand = new DelegateCommand(ThemMoiTuiHSCT);
        }

        private async void InitData()
        {
            DanhSachThonXom = await _thonXomService.LietKeToanBoThonXom();
        }

        #endregion

        #region Them moi tui ho so

        public ICommand ThemMoiTuiHSCTCommand { get; private set; }

        private async void ThemMoiTuiHSCT()
        {
            try
            {
                KiemTraThongTinCuaTuiHSCT();

                //Lay so HSCT lon nhat tu du lieu da co
                int soHSCTMoi = await _tuiHSCTService.TaoSoHSCTMoi();

                var hsctMoi = new HSCT((uint)soHSCTMoi, SelectedThonXomChuaTuiHSCT, NgayDangKy, HoTenChuHo);

                //Lay thong tin tap ho so bo sung cua thon, xom da chon
                // var tapHSCTBoSungCuaThonXom = await _tapHSCTService.LayTapHSCTBoSungCuaThonXom(SelectedThonXomChuaTuiHSCT);

                var tuiHSCTMoi = new TuiHSCT()
                {
                    HSCT = hsctMoi,
                    // TapHSCT = tapHSCTBoSungCuaThonXom
                };

                MessageBox.Show("Thêm hộ thường trú mới thành công");
            }
            catch (Exception ex)
            {
                if (ex is ChuaChonThonXomChuaTuiHSCTException or NgayDangKyTuiHSCTKhongDungException)
                {
                    var exBase = (BaseException)ex;
                    ErrorText = exBase.Message;
                }
                else
                {
                    Log.Error(ex);
                    MessageBox.Show("Đã có lỗi xảy ra khi thêm hộ thường trú mới");
                }
            }
        }

        private void KiemTraThongTinCuaTuiHSCT()
        {
            if (SelectedThonXomChuaTuiHSCT == null)
            {
                throw new ChuaChonThonXomChuaTuiHSCTException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm cho hộ đăng ký thường trú mới"
                };
            }

            if (NgayDangKy.Date > DateTime.Now.Date)
            {
                throw new NgayDangKyTuiHSCTKhongDungException()
                {
                    ErrorMessage = "Ngày đăng ký thường trú không được quá thời gian so với hiện tại"
                };
            }
            
        }

        #endregion
    }
}