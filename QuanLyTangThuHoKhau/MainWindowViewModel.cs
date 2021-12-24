using System.Windows;
using log4net;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.Settings;
using QuanLyTraThe.Core.Constants.Settings;

namespace QuanLyTangThuHoKhau
{
    public class MainWindowViewModel: BindableBase
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISettingsManager _settingsManager;

        public MainWindowViewModel(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;

            InitData();
        }

        private bool _appDaKhoiTaoDuLieuBanDau;

        public bool AppDaKhoiTaoDuLieuBanDau
        {
            get => _appDaKhoiTaoDuLieuBanDau;
            set => SetProperty(ref _appDaKhoiTaoDuLieuBanDau, value);
        }

        private void InitData()
        {
            bool appDataKhoiTaoDuLieuBanDau;

            var getSettingDaKhoiTaoDuLieuBanDauResult = _settingsManager.GetSetting(
                KhoiTaoDuLieuBanDauSettingKeys.APP_DA_KHOI_TAO_DU_LIEU_BAN_DAU, out appDataKhoiTaoDuLieuBanDau);

            if (getSettingDaKhoiTaoDuLieuBanDauResult)
            {
                AppDaKhoiTaoDuLieuBanDau = appDataKhoiTaoDuLieuBanDau;
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