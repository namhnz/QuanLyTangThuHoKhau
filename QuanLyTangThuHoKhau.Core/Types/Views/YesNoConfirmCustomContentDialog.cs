using System.Threading.Tasks;
using CustomMVVMDialogs;
using ModernWpf.Controls;

namespace QuanLyTangThuHoKhau.Core.Types.Views
{
    public class YesNoConfirmCustomContentDialog : IContentDialog
    {
        private ContentDialog _dialog;

        public YesNoConfirmCustomContentDialog()
        {
            _dialog = new YesNoConfirmContentDialog();
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