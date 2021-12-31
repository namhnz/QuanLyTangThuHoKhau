using System.Windows;
using System.Windows.Controls;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types.Controls
{
    /// <summary>
    /// Interaction logic for SummaryInfoItemControl.xaml
    /// </summary>
    public partial class SummaryInfoItemControl : UserControl
    {
        public SummaryInfoItemControl()
        {
            InitializeComponent();
        }


        public int SoLuong
        {
            get { return (int)GetValue(SoLuongProperty); }
            set { SetValue(SoLuongProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SoLuong.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SoLuongProperty =
            DependencyProperty.Register("SoLuong", typeof(int), typeof(SummaryInfoItemControl),
                new PropertyMetadata(0));


        public string TieuDe
        {
            get { return (string)GetValue(TieuDeProperty); }
            set { SetValue(TieuDeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TieuDe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TieuDeProperty =
            DependencyProperty.Register("TieuDe", typeof(string), typeof(SummaryInfoItemControl),
                new PropertyMetadata(null));


        public string BieuTuong
        {
            get { return (string)GetValue(BieuTuongProperty); }
            set { SetValue(BieuTuongProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BieuTuong.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BieuTuongProperty =
            DependencyProperty.Register("BieuTuong", typeof(string), typeof(SummaryInfoItemControl),
                new PropertyMetadata("&#xE11B;"));
    }
}