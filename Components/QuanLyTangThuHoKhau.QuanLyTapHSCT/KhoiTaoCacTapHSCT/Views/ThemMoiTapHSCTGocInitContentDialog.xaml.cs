using System;
using System.Windows;
using ModernWpf.Controls;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Views
{
    /// <summary>
    /// Interaction logic for ThemMoiTapHSCTGocInitContentDialog.xaml
    /// </summary>
    public partial class ThemMoiTapHSCTGocInitContentDialog
    {
        public ThemMoiTapHSCTGocInitContentDialog()
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
