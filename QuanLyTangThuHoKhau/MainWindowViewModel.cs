using System;
using System.Windows;
using System.Windows.Input;
using log4net;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.Settings;
using QuanLyTangThuHoKhau.Core.Types;
using QuanLyTangThuHoKhau.KhoiTaoDuLieuBanDau.Views;
using QuanLyTangThuHoKhau.QuanLyThaoTacDuLieu.Views;
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
            InitCommands();
        }

        #region Khoi tao

        private void InitData()
        {
            
        }

        private void InitCommands()
        {
            ThayDoiViewTheoCaiDatCommand = new DelegateCommand(ThayDoiViewTheoCaiDat);
        }

        #endregion

        public ICommand ThayDoiViewTheoCaiDatCommand { get; private set; }

        private void ThayDoiViewTheoCaiDat()
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
                        _regionManager.RequestNavigate(MainWindowRegionNames
                            .MAIN_WINDOW_ROOT_REGION, nameof(QuanLyThaoTacDuLieuRootView));
                    }
                    else
                    {
                        _regionManager.RequestNavigate(MainWindowRegionNames
                            .MAIN_WINDOW_ROOT_REGION, nameof(KhoiTaoDuLieuBanDauRootView));
                    }
                }
                else
                {
                    _regionManager.RequestNavigate(MainWindowRegionNames
                        .MAIN_WINDOW_ROOT_REGION, nameof(KhoiTaoDuLieuBanDauRootView));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageBox.Show("Đã có lỗi xảy ra trong quá trình khởi chạp app");
                Application.Current.Shutdown();
            }   
            
        }
    }
}