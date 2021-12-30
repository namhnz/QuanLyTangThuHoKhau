using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Services;
using QuanLyTangThuHoKhau.QuanLyThonXom.Services;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types.ViewModels;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.ViewModels
{
    public class XemCacTuiHSCTViewModel : BindableBase
    {
        private readonly IThonXomCRUDService _thonXomService;
        private readonly ITapHSCTCRUDService _tapHSCTService;
        private readonly ITuiHSCTCRUDService _tuiHSCTService;

        public XemCacTuiHSCTViewModel(IThonXomCRUDService thonXomService, ITapHSCTCRUDService tapHSCTService,
            ITuiHSCTCRUDService tuiHSCTService)
        {
            _thonXomService = thonXomService;
            _tapHSCTService = tapHSCTService;
            _tuiHSCTService = tuiHSCTService;
            InitCommands();
            InitData();
            
        }


        #region Khoi tao

        private async void InitData()
        {
            //Khoi tao tree view hien thi cac cap luu tru
            DanhSachCapLuuTru = new ObservableCollection<ExplorerItemViewModel>();

            var toanBoThonXom = await _thonXomService.LietKeToanBoThonXom();

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
        }

        private void InitCommands()
        {
            ThayDoiDanhSachHSCTHienThiCommand = new DelegateCommand<ExplorerItemViewModel>(ThayDoiDanhSachHSCTHienThi);
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

        #endregion
    }
}