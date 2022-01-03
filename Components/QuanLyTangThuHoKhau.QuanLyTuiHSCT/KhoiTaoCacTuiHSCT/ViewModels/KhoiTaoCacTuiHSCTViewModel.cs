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
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.Core.Settings;
using QuanLyTangThuHoKhau.Core.Types;
using QuanLyTangThuHoKhau.Core.Types.KhoiTaoDuLieuBanDau;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types;
using QuanLyTraThe.Core.Constants.Settings;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.KhoiTaoCacTuiHSCT.ViewModels
{
    public class KhoiTaoCacTuiHSCTViewModel : BindableBase, INavigationAware
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public KhoiTaoCacTuiHSCTViewModel(IDonViHanhChinhService dvhcService, IRegionManager regionManager,
            ISettingsManager settingsManager)
        {
            _dvhcService = dvhcService;
            _regionManager = regionManager;
            _settingsManager = settingsManager;

            InitCommands();
        }

        #region Phu thuoc

        private readonly IDonViHanhChinhService _dvhcService;
        private readonly IRegionManager _regionManager;
        private readonly ISettingsManager _settingsManager;

        #endregion

        #region Khoi tao

        private void InitCommands()
        {
            //Khoi tao command dieu huong truoc, sau
            QuayVeBuocTaoCacTapHSCTGocCommand = new DelegateCommand(QuayVeBuocTaoCacTapHSCTGoc);
            HoanThanhKhoiTaoDuLieuBanDauCommand = new DelegateCommand(HoanThanhKhoiTaoDuLieuBanDau);
        }

        #endregion

        private List<ThonXom> _danhSachThonXom;
        private List<TapHSCTGocInitModel> _toanBoTapHSCTGoc;
        private List<TuiHSCT> _toanBoTuiHSCTBanDau;

        private List<TuiHSCT> TaoCacTuiHSCTTheoTapHSCTGocInit(TapHSCTGocInitModel tapHSCTGoc)
        {
            var thonXomChuaTapHSCT = tapHSCTGoc.ThonXom;

            var cacTuiHSCTTrongTapHSCT = new List<TuiHSCT>();
            for (int i = tapHSCTGoc.SoHSCTBatDau; i <= tapHSCTGoc.SoHSCTKetThuc; i++)
            {
                var soHSCT = i;

                var hsct = new HSCT((uint)soHSCT, thonXomChuaTapHSCT, null);

                var tuiHSCTMoi = new TuiHSCT()
                {
                    HSCT = hsct,
                    TapHSCT = (TapHSCT)tapHSCTGoc,
                    ViTriTui = soHSCT - tapHSCTGoc.SoHSCTBatDau + 1
                };

                // Debug.WriteLine(JsonConvert.SerializeObject(tuiHSCTMoi));

                cacTuiHSCTTrongTapHSCT.Add(tuiHSCTMoi);
            }

            return cacTuiHSCTTrongTapHSCT;
        }

        private void GenerateData()
        {
            try
            {
                IsDangKhoiTaoCacTuiHSCT = true;

                _toanBoTuiHSCTBanDau = new List<TuiHSCT>();
                int tongSoTuiHSCTDaTao = 0;

                foreach (var tapHSCTGoc in _toanBoTapHSCTGoc)
                {
                    var cacTuiHSCTDuocTao = TaoCacTuiHSCTTheoTapHSCTGocInit(tapHSCTGoc);
                    _toanBoTuiHSCTBanDau.AddRange(cacTuiHSCTDuocTao);

                    tongSoTuiHSCTDaTao += cacTuiHSCTDuocTao.Count;
                }

                foreach (var tuiHSCT in _toanBoTuiHSCTBanDau)
                {
                    Debug.WriteLine(JsonConvert.SerializeObject(tuiHSCT));
                }

                SoLuongTuiHSCTDaKhoiTaoXong = tongSoTuiHSCTDaTao;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageBox.Show("Đã có lỗi xảy ra trong quá trình khởi tạo các túi HSCT");
            }
            finally
            {
                IsDangKhoiTaoCacTuiHSCT = false;
            }
        }

        #region Hien thi trang thai

        private bool _isDangKhoiTaoCacTuiHSCT;

        public bool IsDangKhoiTaoCacTuiHSCT
        {
            get { return _isDangKhoiTaoCacTuiHSCT; }
            set { SetProperty(ref _isDangKhoiTaoCacTuiHSCT, value); }
        }

        #endregion


        private int _soLuongTuiHSCTDaKhoiTaoXong;

        public int SoLuongTuiHSCTDaKhoiTaoXong
        {
            get { return _soLuongTuiHSCTDaKhoiTaoXong; }
            set { SetProperty(ref _soLuongTuiHSCTDaKhoiTaoXong, value); }
        }

        #region Dieu huong truoc, sau

        public ICommand HoanThanhKhoiTaoDuLieuBanDauCommand { get; private set; }

        private void HoanThanhKhoiTaoDuLieuBanDau()
        {
            // _regionManager.RequestNavigate(KhoiTaoDuLieuBanDauRegionNames.KHOI_TAO_DU_LIEU_BAN_DAU_ROOT_REGION,
            //     "KhoiTaoCacTuiHSCTView");

            CapNhatCaiDatBoQuaBuocKhoiTaoDuLieuBanDau();

            //Khoi dong lai app
            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Application.Current.Shutdown();
        }

        private void CapNhatCaiDatBoQuaBuocKhoiTaoDuLieuBanDau()
        {
            _settingsManager.AddSetting(KhoiTaoDuLieuBanDauSettingKeys.APP_DA_KHOI_TAO_DU_LIEU_BAN_DAU, true);
            _settingsManager.SaveSettings();
        }

        public ICommand QuayVeBuocTaoCacTapHSCTGocCommand { get; private set; }

        private void QuayVeBuocTaoCacTapHSCTGoc()
        {
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
                _danhSachThonXom = new List<ThonXom>(danhSachThonXom);
                _toanBoTapHSCTGoc = new List<TapHSCTGocInitModel>(toanBoTapHSCTGoc);
            }

            GenerateData();
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