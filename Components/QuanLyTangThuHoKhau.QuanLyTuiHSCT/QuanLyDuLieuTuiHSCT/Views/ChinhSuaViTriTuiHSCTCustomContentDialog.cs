using System.Threading.Tasks;
using CustomMVVMDialogs;
using ModernWpf.Controls;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views
{
    public class ChinhSuaViTriTuiHSCTCustomContentDialog : IContentDialog
    {
        private ContentDialog _dialog;

        public ChinhSuaViTriTuiHSCTCustomContentDialog()
        {
            _dialog = new ChinhSuaViTriTuiHSCTContentDialog();
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