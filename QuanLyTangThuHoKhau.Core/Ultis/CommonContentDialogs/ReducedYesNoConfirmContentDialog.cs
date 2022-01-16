using System.Threading.Tasks;
using CustomMVVMDialogs;
using ModernWpf.Controls;
using QuanLyTangThuHoKhau.Core.Types.ViewModels;
using QuanLyTangThuHoKhau.Core.Types.Views;

namespace QuanLyTangThuHoKhau.Core.Ultis.CommonContentDialogs
{
    public class ReducedYesNoConfirmContentDialog
    {
        public static async Task<ContentDialogResult> Show(IDialogService dialogService, string message)
        {
            var dialogViewModel = new YesNoConfirmCustomContentDialogViewModel();
            dialogViewModel.NoiDungXacNhan = message;

            var dialogResult = await dialogService.ShowCustomContentDialogAsync<YesNoConfirmCustomContentDialog>(
                dialogViewModel);

            return dialogResult;
        }
    }
}