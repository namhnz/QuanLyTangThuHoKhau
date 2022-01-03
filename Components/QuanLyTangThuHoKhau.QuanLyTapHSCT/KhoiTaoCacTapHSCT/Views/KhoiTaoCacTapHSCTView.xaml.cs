using System.Windows;
using System.Windows.Controls;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.ViewModels;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Views
{
    /// <summary>
    /// Interaction logic for KhoiTaoCacTapHSCTView.xaml
    /// </summary>
    public partial class KhoiTaoCacTapHSCTView : UserControl
    {
        public KhoiTaoCacTapHSCTView()
        {
            InitializeComponent();
        }

        private void ChinhSuaThongTinTapHSCTItemContextMenu_OnChinhSua(object sender, RoutedEventArgs e)
        {
            var viewModel = (KhoiTaoCacTapHSCTViewModel)this.DataContext;

            var senderListBoxItem = (MenuItem)sender;
            viewModel.ShowChinhSuaTapHSCTGocInitCustomContentDialogCommand.Execute(senderListBoxItem.DataContext);

            e.Handled = true;
        }

        private void ChinhSuaThongTinTapHSCTItemContextMenu_OnXoa(object sender, RoutedEventArgs e)
        {
            var viewModel = (KhoiTaoCacTapHSCTViewModel)this.DataContext;

            var senderListBoxItem = (MenuItem)sender;
            viewModel.XoaTapHSCTGocInitCommand.Execute(senderListBoxItem.DataContext);

            e.Handled = true;
        }
    }
}