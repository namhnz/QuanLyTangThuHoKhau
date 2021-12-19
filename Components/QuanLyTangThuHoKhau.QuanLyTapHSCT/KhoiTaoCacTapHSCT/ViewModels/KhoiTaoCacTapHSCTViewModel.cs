using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CustomMVVMDialogs;
using log4net;
using ModernWpf.Controls;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types;
using Prism.Commands;
using QuanLyTangThuHoKhau.Core.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Views;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.ViewModels
{
    public class KhoiTaoCacTapHSCTViewModel : BindableBase
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;
        private readonly IDonViHanhChinhService _dvhcService;
        //private IThonXomCRUDService _thonXomService;
        //private ITapHSCTCRUDService _tapHSCTService;

        private List<ThonXomKemTheoTapHSCTViewModel> _cacThonXomKemTheoTapHSCTViewModel;

        public List<ThonXomKemTheoTapHSCTViewModel> CacThonXomKemTheoTapHSCTViewModel
        {
            get => _cacThonXomKemTheoTapHSCTViewModel;
            set => SetProperty(ref _cacThonXomKemTheoTapHSCTViewModel, value);
        }

        public KhoiTaoCacTapHSCTViewModel(IDialogService dialogService, IDonViHanhChinhService dvhcService/*, IThonXomCRUDService thonXomService*//*, ITapHSCTCRUDService tapHSCTService*/)
        {
            _dialogService = dialogService;
            _dvhcService = dvhcService;
            //_thonXomService = thonXomService;
            //_tapHSCTService = tapHSCTService;

            InitCommands();

            InitSampleData();
        }

        private async void InitSampleData()
        {
            var xaPhuongHienDangQuanLy =
                (await _dvhcService.LoadToanBoXaPhuongVietNam()).First(x => x.TenDonVi.Contains("Quỳnh Hoa"));

            var cacThonXomKhoiTao = new List<ThonXom>();
            for (int i = 0; i < 6; i++)
            {
                cacThonXomKhoiTao.Add(new ThonXom()
                {
                    TenThonXom = $"Thôn {i + 1}",
                    DonViHanhChinhPhuongXa = xaPhuongHienDangQuanLy
                });
            }

            CacThonXomKemTheoTapHSCTViewModel = new List<ThonXomKemTheoTapHSCTViewModel>();
            foreach (var thonXom in cacThonXomKhoiTao)
            {
                var tapHSCTGoc1 = new TapHSCTGocInitModel();
                tapHSCTGoc1.KhoiTaoCacGiaTriCuaTapHSCT(thonXom, 1, 1, 255);
                // var tapHSCTGoc2 = new TapHSCTGocInitModel();
                // tapHSCTGoc2.KhoiTaoCacGiaTriCuaTapHSCT(thonXom, 2, 256, 320);
                // var tapHSCTGoc3 = new TapHSCTGocInitModel();
                // tapHSCTGoc3.KhoiTaoCacGiaTriCuaTapHSCT(thonXom, 1, 321, 330);

                CacThonXomKemTheoTapHSCTViewModel.Add(
                    new ThonXomKemTheoTapHSCTViewModel(thonXom, new ObservableCollection<TapHSCTGocInitModel>()
                    {
                        tapHSCTGoc1,
                        // tapHSCTGoc2,
                        // tapHSCTGoc3
                    }));
            }
        }

        public ICommand ShowThemMoiTapHsctGocInitCustomContentDialogCommand { get; private set; }

        private async void ShowThemMoiTapHSCTGocInitCustomContentDialog()
        {
            var cacThonXomChuaCacTapHSCT = CacThonXomKemTheoTapHSCTViewModel.Select(x => x.ThonXom).ToList();

            var dialogViewModel = new ThemMoiTapHSCTGocInitCustomContentDialogViewModel();

            dialogViewModel.CacThonXomChuaCacTapHSCT = cacThonXomChuaCacTapHSCT;

            var dialogResult =
                await _dialogService.ShowCustomContentDialogAsync<ThemMoiTapHSCTGocInitCustomContentDialog>(
                    dialogViewModel);

            if (dialogResult == ContentDialogResult.Primary)
            {
                // Debug.WriteLine("primary");
                try
                {
                    //Them tap ho so moi vao xom

                    var tapHSCTgocMoi = new TapHSCTGocInitModel();
                    tapHSCTgocMoi.KhoiTaoCacGiaTriCuaTapHSCT(dialogViewModel.SelectedThonXomChuaTapHSCT,
                        (uint)dialogViewModel.SoTapHSCT, (uint)dialogViewModel.SoHSCTBatDau, (uint)dialogViewModel.SoHSCTKetThuc);

                    CacThonXomKemTheoTapHSCTViewModel.First(
                            x => x.ThonXom.TenThonXom == dialogViewModel.SelectedThonXomChuaTapHSCT.TenThonXom)
                        .CacTapHSCTGoc.Add(tapHSCTgocMoi);

                    MessageBox.Show("Thêm tập hồ sơ gốc mới vào thôn, xóm thành công");
                }
                catch (Exception ex)
                {
                    if (ex is ChuaChonThonXomChuaTapHSCTException or KhoangSoHSCTKhongDungException or
                        ThuTuTapHSCTKhongDungException)
                    {
                        var exBase = (BaseException)ex;
                        MessageBox.Show(exBase.ErrorMessage);
                    }
                    else
                    {
                        Log.Error(ex);
                        MessageBox.Show("Đã có lỗi xảy ra khi thêm tập hồ sơ gốc mới");
                    }
                }
            }
            else
            {
                // Debug.WriteLine("secondary or none");
            }
        }

        private void InitCommands()
        {
            ShowThemMoiTapHsctGocInitCustomContentDialogCommand =
                new DelegateCommand(ShowThemMoiTapHSCTGocInitCustomContentDialog);
        }
    }
}