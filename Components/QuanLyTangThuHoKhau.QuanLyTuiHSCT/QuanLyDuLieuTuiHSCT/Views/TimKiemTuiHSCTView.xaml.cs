using System.Windows;
using System.Windows.Controls;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views
{
    /// <summary>
    /// Interaction logic for TimKiemTuiHSCTView.xaml
    /// </summary>
    public partial class TimKiemTuiHSCTView : UserControl
    {
        public TimKiemTuiHSCTView()
        {
            InitializeComponent();

            #if DEBUG
                HienThiThongTinDonateUserControl.Visibility = Visibility.Hidden;
#else
                HienThiThongTinDonateUserControl.Visibility = Visibility.Visible; // or Collapsed
#endif
        }
    }
}
