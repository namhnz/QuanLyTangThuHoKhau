using System.Windows;
using System.Windows.Input;
using CustomMVVMDialogs;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.MenuPart.TroGiup;
using QuanLyTangThuHoKhau.MenuPart.TroGiup.ViewModels;
using QuanLyTangThuHoKhau.MenuPart.TroGiup.Views;

namespace QuanLyTangThuHoKhau.MenuPart
{
    public class MenuPartRootViewModel: BindableBase
    {
        private readonly IDialogService _dialogService;

        public MenuPartRootViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            InitCommands();
        }

        #region Khoi tao

        private void InitCommands()
        {
            ShowGioiThieuContentDialogCommand = new DelegateCommand(ShowGioiThieuContentDialog);
        }

        private void InitData()
        {

        }

        #endregion

        public ICommand ShowGioiThieuContentDialogCommand { get; private set; }

        private async void ShowGioiThieuContentDialog()
        {
            var dialogViewModel = new GioiThieuCustomContentDialogViewModel();

            await _dialogService.ShowCustomContentDialogAsync<GioiThieuCustomContentDialog>(
                    dialogViewModel);
            
        }
    }
}