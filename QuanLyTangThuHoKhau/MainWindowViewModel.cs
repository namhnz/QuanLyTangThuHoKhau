using System.Windows;
using log4net;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.Settings;
using QuanLyTangThuHoKhau.Core.Types;
using QuanLyTangThuHoKhau.KhoiTaoDuLieuBanDau.Views;
using QuanLyTangThuHoKhau.QuanLyDuLieu.Views;
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
            var getSettingDaKhoiTaoDuLieuBanDauResult = _settingsManager.GetSetting(
                KhoiTaoDuLieuBanDauSettingKeys.APP_DA_KHOI_TAO_DU_LIEU_BAN_DAU, out bool appDataKhoiTaoDuLieuBanDau);

            if (getSettingDaKhoiTaoDuLieuBanDauResult)
            {
                if (appDataKhoiTaoDuLieuBanDau)
                {
                    _regionManager.RegisterViewWithRegion<QuanLyDuLieuRootView>(MainWindowRegionNames
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
                Log.Error("Co loi xay ra khi lay gia tri cua APP_DA_KHOI_TAO_DU_LIEU_BAN_DAU tu settings");
                MessageBox.Show("Đã có lỗi xảy ra trong quá trình lấy các giá trị cài đặt");
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}