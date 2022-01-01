using System.Threading.Tasks;
using CustomMVVMDialogs;
using ModernWpf.Controls;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Views;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views
{
    public class ChinhSuaTuiHSCTCustomContentDialog: IContentDialog
    {
        private ContentDialog _dialog;

        public ChinhSuaTuiHSCTCustomContentDialog()
        {
            _dialog = new ChinhSuaTuiHSCTContentDialog();
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