using System.Windows;
using ModernWpf.Controls;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views
{
    /// <summary>
    /// Interaction logic for ChinhSuaTuiHSCTContentDialog.xaml
    /// </summary>
    public partial class ChinhSuaTuiHSCTContentDialog
    {
        public ChinhSuaTuiHSCTContentDialog()
        {
            InitializeComponent();
        }
        

        private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void OnCloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            deferral.Complete();
        }

        private void OnClosed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            ErrorText.Text = string.Empty;
            ErrorText.Visibility = Visibility.Collapsed;
        }
    }
}
