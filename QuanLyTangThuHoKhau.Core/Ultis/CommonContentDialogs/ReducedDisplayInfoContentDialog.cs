using System.Threading.Tasks;
using CustomMVVMDialogs;
using QuanLyTangThuHoKhau.Core.Types.ViewModels;
using QuanLyTangThuHoKhau.Core.Types.Views;

namespace QuanLyTangThuHoKhau.Core.Ultis.CommonContentDialogs
{
    // Dung de nhanh chong hien thi thong tin
    public class ReducedDisplayInfoContentDialog
    {
        public static async Task Show(IDialogService dialogService, string message)
        {
            var dialogViewModel = new DisplayInfoCustomContentDialogViewModel
            {
                NoiDungThongBao = message
            };

            await dialogService.ShowCustomContentDialogAsync<DisplayInfoCustomContentDialog>(
                    dialogViewModel);
        }
    }
}