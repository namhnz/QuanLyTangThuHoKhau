using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using CustomMVVMDialogs;
using log4net;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.Settings;
using QuanLyTangThuHoKhau.Core.Types.ViewModels;
using QuanLyTangThuHoKhau.Core.Types.Views;
using QuanLyTangThuHoKhau.MenuPart.TroGiup;
using QuanLyTangThuHoKhau.MenuPart.TroGiup.ViewModels;
using QuanLyTangThuHoKhau.MenuPart.TroGiup.Views;
using QuanLyTangThuHoKhau.QuanLyThonXom.Services;
using QuanLyTraThe.Core.Constants.Settings;

namespace QuanLyTangThuHoKhau.MenuPart
{
    public class MenuPartRootViewModel : BindableBase
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Phu thuoc

        private readonly IDialogService _dialogService;
        private readonly IThonXomCRUDService _thonXomService;
        private readonly ISettingsManager _settingsManager;

        #endregion

        public MenuPartRootViewModel(IDialogService dialogService, IThonXomCRUDService thonXomService,
            ISettingsManager settingsManager)
        {
            _dialogService = dialogService;
            _thonXomService = thonXomService;
            _settingsManager = settingsManager;

            InitCommands();
        }

        #region Khoi tao

        private void InitCommands()
        {
            // Tro giup
            ShowGioiThieuContentDialogCommand = new DelegateCommand(ShowGioiThieuContentDialog);

            // Du lieu
            ShowResetToanBoDuLieuDialogCommand = new DelegateCommand(ShowResetToanBoDuLieuDialog);
        }

        private void InitData()
        {
        }

        #endregion

        #region Tro giup

        public ICommand ShowGioiThieuContentDialogCommand { get; private set; }

        private async void ShowGioiThieuContentDialog()
        {
            var dialogViewModel = new GioiThieuCustomContentDialogViewModel();

            await _dialogService.ShowCustomContentDialogAsync<GioiThieuCustomContentDialog>(
                dialogViewModel);
        }

        #endregion

        #region Du lieu

        public ICommand ShowResetToanBoDuLieuDialogCommand { get; private set; }

        private async void ShowResetToanBoDuLieuDialog()
        {
            var dialogViewModel = new YesNoConfirmCustomContentDialogViewModel();
            dialogViewModel.NoiDungXacNhan = "Bạn có muốn xoá toàn bộ dữ liệu và khởi tạo lại từ đầu không?";

            var dialogResult = await _dialogService.ShowCustomContentDialogAsync<YesNoConfirmCustomContentDialog>(
                dialogViewModel);

            if (dialogResult == ContentDialogResult.Primary)
            {
                try
                {
                    // Reset lai Db
                    await _thonXomService.XoaTatCaDuLieu();

                    // Cap nhat lai cai dat
                    CapNhatCaiDatQuayLaiBuocKhoiTaoDuLieuBanDau();

                    MessageBox.Show(
                        "Khởi tạo dữ liệu ban đầu thành công. Phần mềm sẽ tự khởi động lại để tải dữ liệu mới");

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
        }

        private void CapNhatCaiDatQuayLaiBuocKhoiTaoDuLieuBanDau()
        {
            _settingsManager.AddSetting(KhoiTaoDuLieuBanDauSettingKeys.APP_DA_KHOI_TAO_DU_LIEU_BAN_DAU, false);
            _settingsManager.SaveSettings();
        }

        #endregion
    }
}