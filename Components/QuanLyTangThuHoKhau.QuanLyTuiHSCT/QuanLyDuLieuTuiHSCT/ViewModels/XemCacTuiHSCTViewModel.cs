using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Services;
using QuanLyTangThuHoKhau.QuanLyThonXom.Services;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types.ViewModels;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.ViewModels
{
    public class XemCacTuiHSCTViewModel : BindableBase
    {
        private readonly IThonXomCRUDService _thonXomService;
        private readonly ITapHSCTCRUDService _tapHSCTService;

        public XemCacTuiHSCTViewModel(IThonXomCRUDService thonXomService, ITapHSCTCRUDService tapHSCTService)
        {
            _thonXomService = thonXomService;
            _tapHSCTService = tapHSCTService;
            InitData();
        }


        #region Khoi tao

        private async void InitData()
        {
            DanhSachCapLuuTru = new ObservableCollection<ExplorerItemViewModel>();

            var toanBoThonXom = await _thonXomService.LietKeToanBoThonXom();

            foreach (var thonXom in toanBoThonXom)
            {
                var toanBoTapHSCTTheoThonXomVM = (await _tapHSCTService.LietKeToanBoTapHSCTTheoThonXom(thonXom)).Select(
                    x => new ExplorerItemViewModel()
                    {
                        Name = x.LoaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTGoc ? x.ToString() : $"{x.ToString()} - Bổ sung",
                        Type = ExplorerItemType.TapHSCT
                    });

                DanhSachCapLuuTru.Add(new ExplorerItemViewModel()
                {
                    Name = thonXom.TenThonXom,
                    Type = ExplorerItemType.ThonXom,
                    Children = new ObservableCollection<ExplorerItemViewModel>(toanBoTapHSCTTheoThonXomVM)
                });
            }
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
    }
}