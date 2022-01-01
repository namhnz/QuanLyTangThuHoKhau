using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using log4net;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Exceptions;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.Core.Types.QuanLyDuLieu;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Services;
using QuanLyTangThuHoKhau.QuanLyThonXom.Services;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.ViewModels
{
    public class ThemMoiTuiHSCTViewModel : BindableBase, INavigationAware
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ThemMoiTuiHSCTViewModel(IThonXomCRUDService thonXomService, ITapHSCTCRUDService tapHSCTService,
            ITuiHSCTCRUDService tuiHSCTService, IRegionManager regionManager)
        {
            _thonXomService = thonXomService;
            _tapHSCTService = tapHSCTService;
            _tuiHSCTService = tuiHSCTService;
            _regionManager = regionManager;

            InitData();
            InitCommands();
        }

        #region Du lieu nhap vao

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

        private DateTime? _ngayDangKy;

        public DateTime? NgayDangKy
        {
            get => _ngayDangKy;
            set => SetProperty(ref _ngayDangKy, value);
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

        #region Cac phu thuoc

        private readonly IThonXomCRUDService _thonXomService;
        private readonly ITapHSCTCRUDService _tapHSCTService;
        private readonly ITuiHSCTCRUDService _tuiHSCTService;
        private readonly IRegionManager _regionManager;

        #endregion

        #region Khoi tao

        private void InitCommands()
        {
            ThemMoiTuiHSCTCommand = new DelegateCommand(ThemMoiTuiHSCT);
        }

        private async void InitData()
        {
            NgayDangKy = DateTime.Now;
            DanhSachThonXom = await _thonXomService.LietKeToanBoThonXom();
        }

        #endregion

        #region Them moi tui ho so

        public ICommand ThemMoiTuiHSCTCommand { get; private set; }

        private async void ThemMoiTuiHSCT()
        {
            ErrorText = null;

            try
            {
                KiemTraThongTinCuaTuiHSCT();

                //Lay so HSCT lon nhat tu du lieu da co
                int soHSCTMoi = await _tuiHSCTService.TaoSoHSCTMoi();

                var hsctMoi = new HSCT((uint)soHSCTMoi, SelectedThonXomChuaTuiHSCT, NgayDangKy, HoTenChuHo);

                //Lay thong tin tap ho so bo sung cua thon, xom da chon
                var tapHSCTBoSungCuaThonXom =
                    await _tapHSCTService.LayTapHSCTBoSungCuaThonXom(SelectedThonXomChuaTuiHSCT);
                var viTriTuiHSCTMoi = await _tuiHSCTService.TaoViTriTuiHSCTMoi(SelectedThonXomChuaTuiHSCT);

                var tuiHSCTMoi = new TuiHSCT()
                {
                    HSCT = hsctMoi,
                    TapHSCT = tapHSCTBoSungCuaThonXom,
                    ViTriTui = viTriTuiHSCTMoi
                };

                //Them moi tui ho so vao du lieu
                await _tuiHSCTService.ThemTuiHSCTMoi(tuiHSCTMoi);

                // Debug.WriteLine(JsonConvert.SerializeObject(tuiHSCTMoi));
                MessageBox.Show("Thêm hộ thường trú mới thành công");

                //Hien thi thong tin tui ho so moi tao o tren
                HienThiThongTinTuiHSCTMoiTao(tuiHSCTMoi);
            }
            catch (Exception ex)
            {
                if (ex is ChuaChonThonXomChuaTuiHSCTException or NgayDangKyTuiHSCTKhongDungException)
                {
                    var exBase = (BaseException)ex;
                    ErrorText = exBase.ErrorMessage;
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

            if (NgayDangKy == null)
            {
                throw new NgayDangKyTuiHSCTKhongDungException()
                {
                    ErrorMessage = "Ngày đăng ký thường trú không được để trống"
                };
            }

            if (NgayDangKy.Value.Date > DateTime.Now.Date)
            {
                throw new NgayDangKyTuiHSCTKhongDungException()
                {
                    ErrorMessage = "Ngày đăng ký thường trú không được quá thời gian so với hiện tại"
                };
            }
        }

        #endregion

        #region Dieu huong den thong tin tui ho so moi

        private void HienThiThongTinTuiHSCTMoiTao(TuiHSCT tuiHSCTMoiTao)
        {
            if (tuiHSCTMoiTao != null)
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("TuiHSCTCanHienThiChiTiet", tuiHSCTMoiTao);

                _regionManager.RequestNavigate(QuanLyDuLieuRegionNames.QUAN_LY_DU_LIEU_ROOT_REGION,
                    nameof(TimKiemTuiHSCTView), navigationParameters);
            }
        }

        #endregion

        #region Dieu huong

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}