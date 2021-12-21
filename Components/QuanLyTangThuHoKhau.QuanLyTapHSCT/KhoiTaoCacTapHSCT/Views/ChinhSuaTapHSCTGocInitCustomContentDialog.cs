using System.Threading.Tasks;
using CustomMVVMDialogs;
using ModernWpf.Controls;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Views
{
    public class ChinhSuaTapHSCTGocInitCustomContentDialog : IContentDialog
    {
        private ContentDialog _dialog;

        public ChinhSuaTapHSCTGocInitCustomContentDialog()
        {
            _dialog = new ChinhSuaTapHSCTGocInitContentDialog();
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