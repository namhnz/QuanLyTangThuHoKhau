using System.Threading.Tasks;
using CustomMVVMDialogs;
using ModernWpf.Controls;

namespace QuanLyTangThuHoKhau.MenuPart.TroGiup.Views
{
    public class GioiThieuCustomContentDialog: IContentDialog
    {
        private ContentDialog _dialog;

        public GioiThieuCustomContentDialog()
        {
            _dialog = new GioiThieuContentDialog();
        }

        public object DataContext
        {
            get => _dialog.DataContext;
            set => _dialog.DataContext = value;
        }

        public Task<ContentDialogResult> ShowAsync()
        {
            return _dialog.ShowAsync(ContentDialogPlacement.Popup);
        }
    }
}