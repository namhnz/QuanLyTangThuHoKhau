using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CustomMVVMDialogs;
using log4net;
using ModernWpf.Controls;
using Newtonsoft.Json;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types;
using Prism.Commands;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.Exceptions;
using QuanLyTangThuHoKhau.Core.Types.KhoiTaoDuLieuBanDau;
using QuanLyTangThuHoKhau.Core.Ultis;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Views;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.ViewModels
{
    public class KhoiTaoCacTapHSCTViewModel : BindableBase, INavigationAware
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Cac phu thuoc

        private readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;

        #endregion

        public KhoiTaoCacTapHSCTViewModel(IDialogService dialogService, IRegionManager regionManager)
        {
            _dialogService = dialogService;
            _regionManager = regionManager;

            InitCommands();

            // InitSampleData();
            InitData();
        }

        #region Khoi tao

        private void InitData()
        {
            CacThonXomKemTheoTapHSCT = new List<ThonXomKemTheoTapHSCT>();

            // _danhSachThonXomGoc = new List<ThonXom>();
        }

        private void InitCommands()
        {
            ShowThemMoiTapHSCTGocInitCustomContentDialogCommand =
                new DelegateCommand(ShowThemMoiTapHSCTGocInitCustomContentDialog);

            ShowChinhSuaTapHSCTGocInitCustomContentDialogCommand =
                new DelegateCommand<TapHSCTGocInitModel>(ShowChinhSuaTapHSCTGocInitCustomContentDialog);

            XoaTapHSCTGocInitCommand = new DelegateCommand<TapHSCTGocInitModel>(XoaTapHSCTGocInit);

            //Khoi tao command dieu huong truoc, sau
            QuayVeBuocKhoiTaoDanhSachThonXomCommand = new DelegateCommand(QuayVeBuocKhoiTaoDanhSachThonXom);
            ChuyenBuocKhoiTaoCacTuiHSCTCommand = new DelegateCommand(ChuyenBuocKhoiTaoCacTuiHSCT);
        }

        #endregion

        #region Cac danh sach hien thi

        private List<ThonXomKemTheoTapHSCT> _cacThonXomKemTheoTapHSCT;

        public List<ThonXomKemTheoTapHSCT> CacThonXomKemTheoTapHSCT
        {
            get => _cacThonXomKemTheoTapHSCT;
            set
            {
                if (value is List<ThonXomKemTheoTapHSCT> list)
                {
                    SetProperty(ref _cacThonXomKemTheoTapHSCT, list);
                }
                else
                {
                    _cacThonXomKemTheoTapHSCT = new List<ThonXomKemTheoTapHSCT>();
                }
            }
        }

        #endregion

        #region Cac command thao tac du lieu

        public ICommand ShowThemMoiTapHSCTGocInitCustomContentDialogCommand { get; private set; }

        private async void ShowThemMoiTapHSCTGocInitCustomContentDialog()
        {
            var cacThonXomChuaCacTapHSCT = CacThonXomKemTheoTapHSCT.Select(x => x.ThonXom).ToList();

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

                    CacThonXomKemTheoTapHSCT.First(
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
            if (CacThonXomKemTheoTapHSCT.First(x => x.ThonXom.TenThonXom == tapHSCTMoi.ThonXom.TenThonXom)
                    .CacTapHSCTGoc.Count > 0)
            {
                var thuTuTapHSCTLonNhatTrongThonXom =
                    CacThonXomKemTheoTapHSCT.Where(x => x.ThonXom.TenThonXom == tapHSCTMoi.ThonXom.TenThonXom)
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
            if (CacThonXomKemTheoTapHSCT.SelectMany(x => x.CacTapHSCTGoc).Any())
            {
                var soHSCTLonNhatToanXaPhuong = CacThonXomKemTheoTapHSCT.SelectMany(x => x.CacTapHSCTGoc)
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
        }

        public ICommand ShowChinhSuaTapHSCTGocInitCustomContentDialogCommand { get; private set; }

        private async void ShowChinhSuaTapHSCTGocInitCustomContentDialog(TapHSCTGocInitModel tapHSCTGoc)
        {
            var tapHSCTCanChinhSua = tapHSCTGoc.DeepClone();

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

                    var cacTapHSCTGocTrongCungThonXom = CacThonXomKemTheoTapHSCT.First(
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
            if (CacThonXomKemTheoTapHSCT.First(x => x.ThonXom.TenThonXom == tapHSCTChinhSua.ThonXom.TenThonXom)
                    .CacTapHSCTGoc.Count > 0)
            {
                var isThuTuTapHSCTTonTaiTrongThonXom =
                    CacThonXomKemTheoTapHSCT
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
            var toanBoCacTapHSCTTrongToanXaPhuong = CacThonXomKemTheoTapHSCT.SelectMany(x => x.CacTapHSCTGoc)
                .ToList().DeepClone().ToList();
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
                var toanBoCacTapHSCTTrongToanXaPhuong = CacThonXomKemTheoTapHSCT
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

        #endregion

        #region Dieu huong truoc, sau

        public ICommand ChuyenBuocKhoiTaoCacTuiHSCTCommand { get; private set; }

        private void ChuyenBuocKhoiTaoCacTuiHSCT()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("DanhSachThonXomThuocXaPhuongDangQuanLy",
                CacThonXomKemTheoTapHSCT.Select(x => x.ThonXom).ToList());
            navigationParameters.Add("ToanBoTapHSCTGoc",
                CacThonXomKemTheoTapHSCT.SelectMany(x => x.CacTapHSCTGoc).ToList());

            _regionManager.RequestNavigate(KhoiTaoDuLieuBanDauRegionNames.KHOI_TAO_DU_LIEU_BAN_DAU_ROOT_REGION,
                "KhoiTaoCacTuiHSCTView", navigationParameters);
        }

        public ICommand QuayVeBuocKhoiTaoDanhSachThonXomCommand { get; private set; }

        private void QuayVeBuocKhoiTaoDanhSachThonXom()
        {
            _regionManager.RequestNavigate(KhoiTaoDuLieuBanDauRegionNames.KHOI_TAO_DU_LIEU_BAN_DAU_ROOT_REGION,
                "KhoiTaoDanhSachThonXomView");
        }

        #endregion


        #region Thuc thi INavigationAware

        // private List<ThonXom> _danhSachThonXomGoc;

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //Lay toan bo tap ho so da them truoc do
            var toanBoTapHSCTDaCo = CacThonXomKemTheoTapHSCT.SelectMany(x => x.CacTapHSCTGoc).ToList();


            // Debug.WriteLine("1");
            var navigationParameters = navigationContext.Parameters;

            var sourceViewName = navigationParameters.GetNavigationSource();

            //Truong hop duoc dieu huong tu view khoi tao thon, xom
            if (!string.IsNullOrEmpty(sourceViewName) && sourceViewName == "KhoiTaoDanhSachThonXomView")
            {
                var danhSachThonXomMoi = (List<ThonXom>)navigationParameters["DanhSachThonXom"];

                // Debug.WriteLine(JsonConvert.SerializeObject(danhSachThonXomMoi));

                // var duLieuThonXomCu = string.Join(string.Empty, _danhSachThonXomGoc.Select(x => x.TenThonXomDayDu));
                // var duLieuThonXomMoi = string.Join(string.Empty, danhSachThonXomMoi.Select(x => x.TenThonXomDayDu));
                //
                // if (duLieuThonXomMoi != duLieuThonXomCu)
                // {
                //     foreach (var thonXomGoc in _danhSachThonXomGoc)
                //     {
                //         //Truong hop thon, xom goc khong co trong danh sach thon, xom moi thi xoa di
                //         if (!danhSachThonXomMoi.Select(x => x.TenThonXomDayDu).Contains(thonXomGoc.TenThonXomDayDu))
                //         {
                //             var thonXomCanXoa = CacThonXomKemTheoTapHSCT.FirstOrDefault(x =>
                //                 x.ThonXom.TenThonXomDayDu == thonXomGoc.TenThonXomDayDu);
                //
                //             if (thonXomCanXoa != null)
                //             {
                //                 var indexCanXoa = CacThonXomKemTheoTapHSCT.IndexOf(thonXomCanXoa);
                //
                //                 CacThonXomKemTheoTapHSCT.RemoveAt(indexCanXoa);
                //             }
                //         }
                //     }
                //
                //     foreach (var thonXomMoi in danhSachThonXomMoi)
                //     {
                //         //Truong hop thon, xom moi khong co trong danh sach thon, xom goc thi them vao
                //         if (!_danhSachThonXomGoc.Select(x => x.TenThonXomDayDu).Contains(thonXomMoi.TenThonXomDayDu))
                //         {
                //             var thonXomMoiKemTheoTapHSCTGoc = new ThonXomKemTheoTapHSCT(thonXomMoi,
                //                 new List<TapHSCTGocInitModel>());
                //
                //             CacThonXomKemTheoTapHSCT.Add(thonXomMoiKemTheoTapHSCTGoc);
                //         }
                //     }
                // }

                var cacThonXomKemTheoTapHSCTMoi = new List<ThonXomKemTheoTapHSCT>();

                foreach (var thonXomMoi in danhSachThonXomMoi)
                {
                    var cacTapHSCTThuocThonXom = toanBoTapHSCTDaCo
                        .Where(x => x.ThonXom.TenThonXomDayDu == thonXomMoi.TenThonXomDayDu).ToList();

                    cacThonXomKemTheoTapHSCTMoi.Add(new ThonXomKemTheoTapHSCT(thonXomMoi, cacTapHSCTThuocThonXom));
                }

                CacThonXomKemTheoTapHSCT = cacThonXomKemTheoTapHSCTMoi;

                // _danhSachThonXomGoc = new List<ThonXom>(danhSachThonXomMoi);

                // Debug.WriteLine(JsonConvert.SerializeObject(CacThonXomKemTheoTapHSCT));
            }

            //Truong hop duoc dieu huong tu view khac
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}