﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Gress;
using log4net;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.Core.Settings;
using QuanLyTangThuHoKhau.Core.Types.KhoiTaoDuLieuBanDau;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Services;
using QuanLyTangThuHoKhau.QuanLyThonXom.Services;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services;
using QuanLyTraThe.Core.Constants.Settings;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.KhoiTaoCacTuiHSCT.ViewModels
{
    public class KhoiTaoCacTuiHSCTViewModel : BindableBase, INavigationAware
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public KhoiTaoCacTuiHSCTViewModel(IRegionManager regionManager,
            ISettingsManager settingsManager, IThonXomCRUDService thonXomService, ITapHSCTCRUDService tapHSCTService,
            ITuiHSCTCRUDService tuiHSCTService)
        {
            _regionManager = regionManager;
            _settingsManager = settingsManager;
            _thonXomService = thonXomService;
            _tapHSCTService = tapHSCTService;
            _tuiHSCTService = tuiHSCTService;

            InitCommands();
        }

        #region Phu thuoc

        private readonly IRegionManager _regionManager;
        private readonly ISettingsManager _settingsManager;

        private readonly IThonXomCRUDService _thonXomService;
        private readonly ITapHSCTCRUDService _tapHSCTService;
        private readonly ITuiHSCTCRUDService _tuiHSCTService;

        #endregion

        #region Khoi tao

        public IProgressManager ProgressManager { get; } = new ProgressManager();

        private void InitCommands()
        {
            // Khoi tao command tao va ghi du lieu vao db
            TaoDuLieuVaGhiVaoDbCommand =
                new DelegateCommand(TaoDuLieuVaGhiVaoDb, () => !ProgressManager.IsActive).ObservesProperty(() =>
                    ProgressManager);

            //Khoi tao command dieu huong truoc, sau
            QuayVeBuocTaoCacTapHSCTGocCommand =
                new DelegateCommand(QuayVeBuocTaoCacTapHSCTGoc, () => !ProgressManager.IsActive).ObservesProperty(() =>
                    ProgressManager);
            HoanThanhKhoiTaoDuLieuBanDauCommand =
                new DelegateCommand(HoanThanhKhoiTaoDuLieuBanDau, () => !ProgressManager.IsActive).ObservesProperty(
                    () =>
                        ProgressManager);
        }

        #endregion

        private List<TapHSCTGocInitModel> _toanBoTapHSCTGoc;

        private List<ThonXom> _danhSachThonXomThemVaoDb;
        private List<TapHSCT> _toanBoTapHSCTThemVaoDb;
        private List<TuiHSCT> _toanBoTuiHSCTThemVaoDb;

        #region Hien thi trang thai

        private int _soLuongThonXomDaKhoiTaoXong;

        public int SoLuongThonXomDaKhoiTaoXong
        {
            get => _soLuongThonXomDaKhoiTaoXong;
            set => SetProperty(ref _soLuongThonXomDaKhoiTaoXong, value);
        }

        // private bool _isDangKhoiTaoCacTuiHSCT;
        //
        // public bool IsDangKhoiTaoCacTuiHSCT
        // {
        //     get { return _isDangKhoiTaoCacTuiHSCT; }
        //     set { SetProperty(ref _isDangKhoiTaoCacTuiHSCT, value); }
        // }

        private int _soLuongTapHSCTDaKhoiTaoXong;

        public int SoLuongTapHSCTDaKhoiTaoXong
        {
            get => _soLuongTapHSCTDaKhoiTaoXong;
            set => SetProperty(ref _soLuongTapHSCTDaKhoiTaoXong, value);
        }

        private int _soLuongTuiHSCTDaKhoiTaoXong;

        public int SoLuongTuiHSCTDaKhoiTaoXong
        {
            get => _soLuongTuiHSCTDaKhoiTaoXong;
            set => SetProperty(ref _soLuongTuiHSCTDaKhoiTaoXong, value);
        }

        #endregion


        #region Dieu huong truoc, sau

        public ICommand HoanThanhKhoiTaoDuLieuBanDauCommand { get; private set; }

        private void HoanThanhKhoiTaoDuLieuBanDau()
        {
            // if (IsDangKhoiTaoCacTuiHSCT)
            // {
            //     MessageBox.Show("Quá trình tạo dữ liệu đang được thực hiện");
            //     return;
            // }

            try
            {
                CapNhatCaiDatBoQuaBuocKhoiTaoDuLieuBanDau();
                MessageBox.Show("Khởi tạo dữ liệu ban đầu thành công. Phần mềm sẽ tự khởi động lại để tải dữ liệu mới");

                // Khoi dong lai app
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageBox.Show("Đã có lỗi xảy ra. Quá trình khởi tạo dữ liệu không thành công, vui lòng thử lại");
            }
        }

        private void CapNhatCaiDatBoQuaBuocKhoiTaoDuLieuBanDau()
        {
            _settingsManager.AddSetting(KhoiTaoDuLieuBanDauSettingKeys.APP_DA_KHOI_TAO_DU_LIEU_BAN_DAU, true);
            _settingsManager.SaveSettings();
        }

        public ICommand QuayVeBuocTaoCacTapHSCTGocCommand { get; private set; }

        private void QuayVeBuocTaoCacTapHSCTGoc()
        {
            // if (IsDangKhoiTaoCacTuiHSCT)
            // {
            //     MessageBox.Show("Quá trình tạo dữ liệu đang được thực hiện");
            //     return;
            // }

            _regionManager.RequestNavigate(KhoiTaoDuLieuBanDauRegionNames.KHOI_TAO_DU_LIEU_BAN_DAU_ROOT_REGION,
                "KhoiTaoCacTapHSCTView");
        }

        #endregion


        #region Dieu huong

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var danhSachThonXom = (List<ThonXom>)navigationContext.Parameters["DanhSachThonXomThuocXaPhuongDangQuanLy"];

            var toanBoTapHSCTGoc = (List<TapHSCTGocInitModel>)navigationContext.Parameters["ToanBoTapHSCTGoc"];

            if (danhSachThonXom != null && toanBoTapHSCTGoc != null)
            {
                _danhSachThonXomThemVaoDb = new List<ThonXom>(danhSachThonXom);
                _toanBoTapHSCTGoc = new List<TapHSCTGocInitModel>(toanBoTapHSCTGoc);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion

        #region Tao du lieu va ghi du lieu vao database

        private List<TuiHSCT> TaoCacTuiHSCTTheoTapHSCTGocInit(TapHSCTGocInitModel tapHSCTGoc)
        {
            // Tao tap ho so de them vao DB
            var tapHSCT = new TapHSCT()
            {
                LoaiTapHSCT = tapHSCTGoc.LoaiTapHSCT,
                ThonXom = tapHSCTGoc.ThonXom,
                ThuTuTapHSCT = tapHSCTGoc.ThuTuTapHSCT
            };

            //Them tap ho so vao danh sach tap ho so them vao DB
            _toanBoTapHSCTThemVaoDb.Add(tapHSCT);

            //Tao cac gia tri khac
            var thonXomChuaTapHSCT = tapHSCT.ThonXom;

            // Khoi tao danh sach tui ho so
            var cacTuiHSCTTrongTapHSCT = new List<TuiHSCT>();
            for (int i = tapHSCTGoc.SoHSCTBatDau; i <= tapHSCTGoc.SoHSCTKetThuc; i++)
            {
                var soHSCT = i;

                var hsct = new HSCT((uint)soHSCT, thonXomChuaTapHSCT, null);

                var tuiHSCTMoi = new TuiHSCT()
                {
                    HSCT = hsct,
                    TapHSCT = tapHSCT,
                    ViTriTui = soHSCT - tapHSCTGoc.SoHSCTBatDau + 1
                };

                // Debug.WriteLine(JsonConvert.SerializeObject(tuiHSCTMoi));

                cacTuiHSCTTrongTapHSCT.Add(tuiHSCTMoi);
            }

            return cacTuiHSCTTrongTapHSCT;
        }

        private void TaoDuLieuToanBoTuiHSCT()
        {
            _toanBoTapHSCTThemVaoDb = new List<TapHSCT>();
            _toanBoTuiHSCTThemVaoDb = new List<TuiHSCT>();

            // int tongSoTuiHSCTDaTao = 0;

            foreach (var tapHSCTGoc in _toanBoTapHSCTGoc)
            {
                var cacTuiHSCTDuocTao = TaoCacTuiHSCTTheoTapHSCTGocInit(tapHSCTGoc);

                _toanBoTuiHSCTThemVaoDb.AddRange(cacTuiHSCTDuocTao);

                // tongSoTuiHSCTDaTao += cacTuiHSCTDuocTao.Count;
            }

            // foreach (var tuiHSCT in _toanBoTuiHSCTThemVaoDb)
            // {
            //     Debug.WriteLine(JsonConvert.SerializeObject(tuiHSCT));
            // }

            // SoLuongTuiHSCTDaKhoiTaoXong = tongSoTuiHSCTDaTao;
        }

        private async Task ThemToanBoDuLieuVaoDb()
        {
            // Them thon xom
            // foreach (var thonXom in _danhSachThonXomThemVaoDb)
            // {
            //     await _thonXomService.ThemThonXomMoi(thonXom);
            // }
            SoLuongThonXomDaKhoiTaoXong = await _thonXomService.ThemNhieuThonXomMoi(_danhSachThonXomThemVaoDb);

            // Them toan bo tap ho so goc
            // foreach (var tapHSCTDb in _toanBoTapHSCTThemVaoDb)
            // {
            //     await _tapHSCTService.ThemTapHSCTMoi(tapHSCTDb);
            // }
            var soTapHSCTDaThemVaoDb = await _tapHSCTService.ThemNhieuTapHSCTMoi(_toanBoTapHSCTThemVaoDb);

            // Them cac tap ho so bo sung
            foreach (var thonXom in _danhSachThonXomThemVaoDb)
            {
                // int thuTuTapHSCTBoSung;
                //
                // if (_toanBoTapHSCTGoc.Count(x => x.ThonXom.Id == thonXom.Id) == 0)
                // {
                //     thuTuTapHSCTBoSung = 1;
                // }
                // else
                // {
                //     thuTuTapHSCTBoSung = _toanBoTapHSCTGoc.Where(x => x.ThonXom.Id == thonXom.Id)
                //         .Max(x => x.ThuTuTapHSCT);
                // }
                //
                // var tapHSCTBoSung = new TapHSCT()
                // {
                //     LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTBoSung,
                //     ThonXom = thonXom,
                //     ThuTuTapHSCT = thuTuTapHSCTBoSung
                // };

                // await _tapHSCTService.ThemTapHSCTMoi(tapHSCTBoSung);

                await _tapHSCTService.ThemTapHSCTBoSung(thonXom);
                soTapHSCTDaThemVaoDb++;
            }
            SoLuongTapHSCTDaKhoiTaoXong = soTapHSCTDaThemVaoDb;

            // Them cac tui ho so
            foreach (var tuiHSCTDb in _toanBoTuiHSCTThemVaoDb)
            {
                await _tuiHSCTService.ThemTuiHSCTMoi(tuiHSCTDb);
            }
        }

        public ICommand TaoDuLieuVaGhiVaoDbCommand { get; private set; }

        private async void TaoDuLieuVaGhiVaoDb()
        {
            try
            {
                using (var operation = ProgressManager.CreateOperation())
                {
                    // IsDangKhoiTaoCacTuiHSCT = true;
                    operation.Report(0);

                    // Tao cac tap ho so va tui ho so
                    var taoDuLieuTask = Task.Run(TaoDuLieuToanBoTuiHSCT);

                    // Reset lai database
                    var resetDbTask = _thonXomService.XoaTatCaDuLieu();

                    await Task.WhenAll(taoDuLieuTask, resetDbTask);

                    await ThemToanBoDuLieuVaoDb();

                    operation.Report(1);
                }

                MessageBox.Show("Thêm dữ liệu vào database thành công");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageBox.Show("Đã có lỗi xảy ra trong quá trình khởi tạo các túi HSCT");
            }
        }

        #endregion
    }
}