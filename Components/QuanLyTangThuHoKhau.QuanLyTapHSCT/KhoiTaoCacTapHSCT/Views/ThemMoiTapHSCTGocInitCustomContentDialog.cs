using System.Threading.Tasks;
using CustomMVVMDialogs;
using ModernWpf.Controls;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Views
{
    public class ThemMoiTapHSCTGocInitCustomContentDialog : IContentDialog
    {
        private ContentDialog _dialog;

        public ThemMoiTapHSCTGocInitCustomContentDialog()
        {
            _dialog = new ThemMoiTapHSCTGocInitContentDialog();
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