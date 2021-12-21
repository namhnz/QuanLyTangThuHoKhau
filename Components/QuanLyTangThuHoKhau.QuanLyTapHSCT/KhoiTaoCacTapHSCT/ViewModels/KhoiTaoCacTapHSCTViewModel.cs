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
using QuanLyTangThuHoKhau.Core.Ultis;
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

        public KhoiTaoCacTapHSCTViewModel(IDialogService dialogService,
            IDonViHanhChinhService dvhcService /*, IThonXomCRUDService thonXomService*/
            /*, ITapHSCTCRUDService tapHSCTService*/)
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

        public ICommand ShowThemMoiTapHSCTGocInitCustomContentDialogCommand { get; private set; }

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

                    var tapHSCTGocMoi = new TapHSCTGocInitModel();
                    tapHSCTGocMoi.KhoiTaoCacGiaTriCuaTapHSCT(dialogViewModel.SelectedThonXomChuaTapHSCT,
                        (uint)dialogViewModel.ThuTuTapHSCT, (uint)dialogViewModel.SoHSCTBatDau,
                        (uint)dialogViewModel.SoHSCTKetThuc);

                    KiemTraTapHSCTMoiDungTrongToanBoTapHSCT(tapHSCTGocMoi);

                    CacThonXomKemTheoTapHSCTViewModel.First(
                            x => x.ThonXom.TenThonXom == dialogViewModel.SelectedThonXomChuaTapHSCT.TenThonXom)
                        .CacTapHSCTGoc.Add(tapHSCTGocMoi);

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

        private void KiemTraTapHSCTMoiDungTrongToanBoTapHSCT(TapHSCTGocInitModel tapHSCTMoi)
        {
            //Kiem tra thu tu tap ho so
            if (CacThonXomKemTheoTapHSCTViewModel.First(x => x.ThonXom.TenThonXom == tapHSCTMoi.ThonXom.TenThonXom)
                    .CacTapHSCTGoc.Count > 0)
            {
                var thuTuTapHSCTLonNhatTrongThonXom =
                    CacThonXomKemTheoTapHSCTViewModel.Where(x => x.ThonXom.TenThonXom == tapHSCTMoi.ThonXom.TenThonXom)
                        .SelectMany(x => x.CacTapHSCTGoc).Max(x => x.ThuTuTapHSCT);

                if (tapHSCTMoi.ThuTuTapHSCT <= thuTuTapHSCTLonNhatTrongThonXom)
                {
                    throw new ThuTuTapHSCTKhongDungException()
                    {
                        ErrorMessage = "Thứ tự tập hồ sơ mới phải lớn hơn thứ tự tập hồ sơ cũ trong cùng thôn, xóm"
                    };
                }
            }

            //Kiem tra khoang so ho so
            var soHSCTLonNhatToanXaPhuong = CacThonXomKemTheoTapHSCTViewModel.SelectMany(x => x.CacTapHSCTGoc)
                .Max(x => x.SoHSCTKetThuc);

            if (tapHSCTMoi.SoHSCTBatDau <= soHSCTLonNhatToanXaPhuong)
            {
                throw new KhoangSoHSCTKhongDungException()
                {
                    ErrorMessage =
                        "Số hồ sơ bắt đầu của tập mới phải lớn hơn số hồ sơ kết thúc của các tập hồ sơ trước đó"
                };
            }
        }

        public ICommand ShowChinhSuaTapHSCTGocInitCustomContentDialogCommand { get; private set; }

        private async void ShowChinhSuaTapHSCTGocInitCustomContentDialog(TapHSCTGocInitModel tapHSCTGoc)
        {
            var tapHSCTCanChinhSua = (TapHSCTGocInitModel)tapHSCTGoc.Clone();

            var dialogViewModel = new ChinhSuaTapHSCTGocInitCustomContentDialogViewModel();

            dialogViewModel.SoHSCTBatDau = tapHSCTCanChinhSua.SoHSCTBatDau;
            dialogViewModel.SoHSCTKetThuc = tapHSCTCanChinhSua.SoHSCTKetThuc;

            var dialogResult =
                await _dialogService.ShowCustomContentDialogAsync<ChinhSuaTapHSCTGocInitCustomContentDialog>(
                    dialogViewModel);

            if (dialogResult == ContentDialogResult.Primary)
            {
                // Debug.WriteLine("primary");
                try
                {
                    //Chinh sua tap ho so
                    tapHSCTCanChinhSua.CapNhatKhoangSoHSCT((uint)dialogViewModel.SoHSCTBatDau,
                        (uint)dialogViewModel.SoHSCTKetThuc);

                    KiemTraTapHSCTChinhSuaDungTrongToanBoTapHSCT(tapHSCTCanChinhSua);

                    var cacTapHSCTGocTrongCungThonXom = CacThonXomKemTheoTapHSCTViewModel.First(
                            x => x.ThonXom.TenThonXom == tapHSCTCanChinhSua.ThonXom.TenThonXom)
                        .CacTapHSCTGoc;

                    var indexTapHSCTGocChinhSua =
                        cacTapHSCTGocTrongCungThonXom.IndexOf(
                            cacTapHSCTGocTrongCungThonXom.First(x =>
                                x.ThuTuTapHSCT == tapHSCTCanChinhSua.ThuTuTapHSCT));

                    cacTapHSCTGocTrongCungThonXom[indexTapHSCTGocChinhSua] = tapHSCTCanChinhSua;

                    MessageBox.Show("Chỉnh sửa tập hồ sơ gốc trong thôn, xóm thành công");
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
                        MessageBox.Show("Đã có lỗi xảy ra khi chỉnh sửa tập hồ sơ gốc");
                    }
                }
            }
            else
            {
                // Debug.WriteLine("secondary or none");
            }
        }

        private void KiemTraTapHSCTChinhSuaDungTrongToanBoTapHSCT(TapHSCTGocInitModel tapHSCTChinhSua)
        {
            //Kiem tra thu tu tap ho so
            if (CacThonXomKemTheoTapHSCTViewModel.First(x => x.ThonXom.TenThonXom == tapHSCTChinhSua.ThonXom.TenThonXom)
                    .CacTapHSCTGoc.Count > 0)
            {
                var isThuTuTapHSCTTonTaiTrongThonXom =
                    CacThonXomKemTheoTapHSCTViewModel
                        .Where(x => x.ThonXom.TenThonXom == tapHSCTChinhSua.ThonXom.TenThonXom)
                        .SelectMany(x => x.CacTapHSCTGoc).Any(x => x.ThuTuTapHSCT == tapHSCTChinhSua.ThuTuTapHSCT);

                if (!isThuTuTapHSCTTonTaiTrongThonXom)
                {
                    throw new ThuTuTapHSCTKhongDungException()
                    {
                        ErrorMessage =
                            "Thứ tự tập hồ sơ chỉnh sửa phải tồn tại trong các tập hồ sơ đang có trong thôn, xóm"
                    };
                }
            }

            //Kiem tra khoang so ho so
            var toanBoCacTapHSCTTrongToanXaPhuong = CacThonXomKemTheoTapHSCTViewModel.SelectMany(x => x.CacTapHSCTGoc)
                .ToList().Clone().ToList();
            //Loai bo tap ho so dang chinh sua
            toanBoCacTapHSCTTrongToanXaPhuong.RemoveAll(x =>
                x.ThonXom.TenThonXom == tapHSCTChinhSua.ThonXom.TenThonXom &&
                x.ThuTuTapHSCT == tapHSCTChinhSua.ThuTuTapHSCT);

            foreach (var tapHSCTKhongChinhSua in toanBoCacTapHSCTTrongToanXaPhuong)
            {
                if (tapHSCTChinhSua.SoHSCTBatDau >= tapHSCTKhongChinhSua.SoHSCTBatDau &&
                    tapHSCTChinhSua.SoHSCTBatDau <= tapHSCTKhongChinhSua.SoHSCTKetThuc ||
                    tapHSCTChinhSua.SoHSCTKetThuc >= tapHSCTKhongChinhSua.SoHSCTBatDau &&
                    tapHSCTChinhSua.SoHSCTKetThuc <= tapHSCTKhongChinhSua.SoHSCTKetThuc)
                {
                    throw new KhoangSoHSCTKhongDungException()
                    {
                        ErrorMessage =
                            "Khoảng số hồ sơ bắt đầu của tập hồ sơ không được lặp với khoảng số hồ sơ các tập hồ sơ khác"
                    };
                }
            }
        }

        public ICommand XoaTapHSCTGocInitCommand { get; private set; }

        private void XoaTapHSCTGocInit(TapHSCTGocInitModel tapHSCTGoc)
        {
            try
            {
                var toanBoCacTapHSCTTrongToanXaPhuong = CacThonXomKemTheoTapHSCTViewModel
                    .First(x => x.ThonXom.TenThonXom == tapHSCTGoc.ThonXom.TenThonXom).CacTapHSCTGoc;

                if (toanBoCacTapHSCTTrongToanXaPhuong.Max(x => x.ThuTuTapHSCT) > tapHSCTGoc.ThuTuTapHSCT)
                {
                    throw new ThuTuTapHSCTKhongDungException()
                    {
                        ErrorMessage = "Không thể xoá tập hồ sơ do tồn tại tập hồ sơ có thứ tự lớn hơn"
                    };
                }

                toanBoCacTapHSCTTrongToanXaPhuong.Remove(tapHSCTGoc);

                MessageBox.Show("Xoá tập hồ sơ gốc thành công");
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
                    MessageBox.Show("Đã có lỗi xảy ra khi xoá tập hồ sơ gốc");
                }
            }
        }

        private void InitCommands()
        {
            ShowThemMoiTapHSCTGocInitCustomContentDialogCommand =
                new DelegateCommand(ShowThemMoiTapHSCTGocInitCustomContentDialog);

            ShowChinhSuaTapHSCTGocInitCustomContentDialogCommand =
                new DelegateCommand<TapHSCTGocInitModel>(ShowChinhSuaTapHSCTGocInitCustomContentDialog);

            XoaTapHSCTGocInitCommand = new DelegateCommand<TapHSCTGocInitModel>(XoaTapHSCTGocInit);
        }
    }
}