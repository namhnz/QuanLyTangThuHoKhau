using System;
using System.Windows;
using log4net;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.Settings;
using QuanLyTangThuHoKhau.Core.Types;
using QuanLyTangThuHoKhau.KhoiTaoDuLieuBanDau.Views;
using QuanLyTangThuHoKhau.QuanLyDuLieu.Views;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views;
using QuanLyTraThe.Core.Constants.Settings;

namespace QuanLyTangThuHoKhau
{
    public class MainWindowViewModel : BindableBase
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISettingsManager _settingsManager;
        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(ISettingsManager settingsManager, IRegionManager regionManager)
        {
            _settingsManager = settingsManager;
            _regionManager = regionManager;

            InitData();
        }

        private void InitData()
        {
            try
            {
                var foundSettingDaKhoiTaoDuLieuBanDauResult = _settingsManager.GetSetting(
                    KhoiTaoDuLieuBanDauSettingKeys.APP_DA_KHOI_TAO_DU_LIEU_BAN_DAU,
                    out bool appDataKhoiTaoDuLieuBanDau);

                if (foundSettingDaKhoiTaoDuLieuBanDauResult)
                {
                    if (appDataKhoiTaoDuLieuBanDau)
                    {
                        _regionManager.RegisterViewWithRegion<TimKiemTuiHSCTView>(MainWindowRegionNames
                            .MAIN_WINDOW_ROOT_REGION);
                    }
                    else
                    {
                        _regionManager.RegisterViewWithRegion<KhoiTaoDuLieuBanDauRootView>(MainWindowRegionNames
                            .MAIN_WINDOW_ROOT_REGION);
                    }
                }
                else
                {
                    _regionManager.RegisterViewWithRegion<KhoiTaoDuLieuBanDauRootView>(MainWindowRegionNames
                        .MAIN_WINDOW_ROOT_REGION);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageBox.Show("Đã có lỗi xảy ra trong quá trình khởi chạp app");
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}