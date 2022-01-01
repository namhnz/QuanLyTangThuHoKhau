using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.Core.Types.QuanLyDuLieu;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Services;
using QuanLyTangThuHoKhau.QuanLyThonXom.Services;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types.ViewModels;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.ViewModels
{
    public class XemCacTuiHSCTViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private readonly IThonXomCRUDService _thonXomService;
        private readonly ITapHSCTCRUDService _tapHSCTService;
        private readonly ITuiHSCTCRUDService _tuiHSCTService;

        public XemCacTuiHSCTViewModel(IThonXomCRUDService thonXomService, ITapHSCTCRUDService tapHSCTService,
            ITuiHSCTCRUDService tuiHSCTService, IRegionManager regionManager)
        {
            _thonXomService = thonXomService;
            _tapHSCTService = tapHSCTService;
            _tuiHSCTService = tuiHSCTService;
            _regionManager = regionManager;

            InitCommands();
            InitData();
            
        }


        #region Khoi tao

        private async void InitData()
        {
            //Khoi tao tree view hien thi cac cap luu tru
            DanhSachCapLuuTru = new ObservableCollection<ExplorerItemViewModel>();

            var toanBoThonXom = await _thonXomService.LietKeToanBoThonXom();

            //Hien thi thong ke
            TongSoThonXom = toanBoThonXom.Count;

            foreach (var thonXom in toanBoThonXom)
            {
                var toanBoTapHSCTTheoThonXomVM = (await _tapHSCTService.LietKeToanBoTapHSCTTheoThonXom(thonXom)).Select(
                    x => new ExplorerItemViewModel()
                    {
                        SourceId = x.Id,
                        Name = x.ToString(),
                        Type = ExplorerItemType.TapHSCT
                    });

                DanhSachCapLuuTru.Add(new ExplorerItemViewModel()
                {
                    SourceId = thonXom.Id,
                    Name = thonXom.TenThonXom,
                    Type = ExplorerItemType.ThonXom,
                    Children = new ObservableCollection<ExplorerItemViewModel>(toanBoTapHSCTTheoThonXomVM)
                });
            }

            //Khoi tao list view hien thi danh sach ho so
            _toanBoHSCTCuaXaPhuong = (await _tuiHSCTService.LietKeToanBoTuiHSCT()).ToList();

            //Hien thi thong ke
            TongSoHoDangThuongTru = _toanBoHSCTCuaXaPhuong.Count(x => x.HSCT.DangThuongTru);
        }

        private void InitCommands()
        {
            ThayDoiDanhSachHSCTHienThiCommand = new DelegateCommand<ExplorerItemViewModel>(ThayDoiDanhSachHSCTHienThi);

            //Command bar flyout
            XemThongChiTietTuiHSCTCommand = new DelegateCommand(() => XemThongChiTietTuiHSCT(false, false));
            ChinhSuaTuiHSCTCommand = new DelegateCommand(() => XemThongChiTietTuiHSCT(true, false));
            XoaDangKyThuongTruTuiHSCTCommand = new DelegateCommand(() => XemThongChiTietTuiHSCT(true, true));
        }

        #endregion

        #region Cac cap chua ho so

        private ObservableCollection<ExplorerItemViewModel> _danhSachCapLuuTru;

        public ObservableCollection<ExplorerItemViewModel> DanhSachCapLuuTru
        {
            get => _danhSachCapLuuTru;
            set => SetProperty(ref _danhSachCapLuuTru, value);
        }
        
        #endregion

        #region Toan bo tui ho so

        private List<TuiHSCT> _toanBoHSCTCuaXaPhuong;

        private List<TuiHSCT> _danhSachHSCTTheoCapLuuTru;

        public List<TuiHSCT> DanhSachHSCTTheoCapLuuTru
        {
            get => _danhSachHSCTTheoCapLuuTru;
            set => SetProperty(ref _danhSachHSCTTheoCapLuuTru, value);
        }

        public ICommand ThayDoiDanhSachHSCTHienThiCommand { get; private set; }

        private void ThayDoiDanhSachHSCTHienThi(ExplorerItemViewModel selectedCapLuuTru)
        {
            if (selectedCapLuuTru != null)
            {
                if (selectedCapLuuTru.Type == ExplorerItemType.ThonXom)
                {
                    DanhSachHSCTTheoCapLuuTru = _toanBoHSCTCuaXaPhuong
                        .Where(x => x.TapHSCT.ThonXom.Id == selectedCapLuuTru.SourceId).ToList();
                }
                else
                {
                    DanhSachHSCTTheoCapLuuTru = _toanBoHSCTCuaXaPhuong
                        .Where(x => x.TapHSCT.Id == selectedCapLuuTru.SourceId).ToList();
                }
            }
        }

        private TuiHSCT _selectedTuiHSCT;

        public TuiHSCT SelectedTuiHSCT
        {
            get => _selectedTuiHSCT;
            set => SetProperty(ref _selectedTuiHSCT, value);
        }


        #endregion

        #region Cac so lieu tong quan

        private int _tongSoThonXom;

        public int TongSoThonXom
        {
            get => _tongSoThonXom;
            set => SetProperty(ref _tongSoThonXom, value);
        }

        private int _tongSoHoDangThuongTru;

        public int TongSoHoDangThuongTru
        {
            get => _tongSoHoDangThuongTru;
            set => SetProperty(ref _tongSoHoDangThuongTru, value);
        }


        #endregion

        #region Command bar flyout

        public ICommand XemThongChiTietTuiHSCTCommand { get; private set; }

        private void XemThongChiTietTuiHSCT(bool showChinhSuaTuiHSCTKemTheo, bool xoaDangKyThuongTruTuiHSCTKemTheo)
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("TuiHSCTCanHienThiChiTiet", SelectedTuiHSCT);

            navigationParameters.Add("ShowChinhSuaTuiHSCTKemTheo", showChinhSuaTuiHSCTKemTheo);
            navigationParameters.Add("XoaDangKyThuongTruTuiHSCTKemTheo", xoaDangKyThuongTruTuiHSCTKemTheo);

            _regionManager.RequestNavigate(QuanLyDuLieuRegionNames.QUAN_LY_DU_LIEU_ROOT_REGION,
                nameof(TimKiemTuiHSCTView), navigationParameters);
        }

        public ICommand ChinhSuaTuiHSCTCommand { get; private set; }
        public ICommand XoaDangKyThuongTruTuiHSCTCommand { get; private set; }

        #endregion
    }
}