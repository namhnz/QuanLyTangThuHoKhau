using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.ViewModels;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views
{
    /// <summary>
    /// Interaction logic for XemCacTuiHSCTView.xaml
    /// </summary>
    public partial class XemCacTuiHSCTView : UserControl
    {
        private CommandBarFlyout ChinhSuaTuiHSCTCommandBarFlyout;

        public XemCacTuiHSCTView()
        {
            InitializeComponent();

            ChinhSuaTuiHSCTCommandBarFlyout = (CommandBarFlyout)Resources[nameof(ChinhSuaTuiHSCTCommandBarFlyout)];
        }

        private void ShowMenu(bool isTransient, FrameworkElement anchor)
        {
            ChinhSuaTuiHSCTCommandBarFlyout.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;
            ChinhSuaTuiHSCTCommandBarFlyout.ShowAt(anchor);
        }

        private void DanhSachTuiHSCTTheoCapLuuTruDataGrid_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            //https://stackoverflow.com/a/29505965/7182661

            var selectedDataGridRow = ItemsControl.ContainerFromElement((DataGrid)sender,
                e.OriginalSource as DependencyObject) as DataGridRow;
            if (selectedDataGridRow == null)
            {
                e.Handled = true;
                return;
            }

            ShowMenu(selectedDataGridRow.IsMouseOver, selectedDataGridRow);
        }

        private void ChinhSuaTuiHSCTCommandBarFlyout_OnXem(object sender, RoutedEventArgs e)
        {
            if (ChinhSuaTuiHSCTCommandBarFlyout.IsOpen)
            {
                ChinhSuaTuiHSCTCommandBarFlyout.Hide();
            }

            var viewModel = (XemCacTuiHSCTViewModel)this.DataContext;
            viewModel.XemThongChiTietTuiHSCTCommand.Execute(sender);
        }
    }
}