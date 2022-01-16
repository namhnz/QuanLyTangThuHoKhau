using System.Windows;
using ModernWpf.Controls;

namespace QuanLyTangThuHoKhau.Core.Types.Views
{
    /// <summary>
    /// Interaction logic for YesNoConfirmContentDialog.xaml
    /// </summary>
    public partial class YesNoConfirmContentDialog
    {
        public YesNoConfirmContentDialog()
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
