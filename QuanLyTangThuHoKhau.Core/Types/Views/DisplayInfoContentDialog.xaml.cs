using ModernWpf.Controls;

namespace QuanLyTangThuHoKhau.Core.Types.Views
{
    /// <summary>
    /// Interaction logic for DisplayInfoContentDialog.xaml
    /// </summary>
    public partial class DisplayInfoContentDialog
    {
        public DisplayInfoContentDialog()
        {
            InitializeComponent();
        }

        private void OnCloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            deferral.Complete();
        }
    }
}
