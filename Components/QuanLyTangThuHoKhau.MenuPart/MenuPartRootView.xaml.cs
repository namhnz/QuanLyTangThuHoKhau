using System.Windows;
using System.Windows.Controls;
using ModernWpf;
using QuanLyTangThuHoKhau.MenuPart.TepTin.Ultis;

namespace QuanLyTangThuHoKhau.MenuPart
{
    /// <summary>
    /// Interaction logic for MenuPartRootView.xaml
    /// </summary>
    public partial class MenuPartRootView : UserControl
    {
        public MenuPartRootView()
        {
            InitializeComponent();
        }


        #region Thay doi theme

        private void Default_Checked(object sender, RoutedEventArgs e)
        {
            SetApplicationTheme(null);
        }

        private void Light_Checked(object sender, RoutedEventArgs e)
        {
            SetApplicationTheme(ApplicationTheme.Light);
        }

        private void Dark_Checked(object sender, RoutedEventArgs e)
        {
            SetApplicationTheme(ApplicationTheme.Dark);
        }

        private void SetApplicationTheme(ApplicationTheme? theme)
        {
            DispatcherHelper.RunOnMainThread(() => { ThemeManager.Current.ApplicationTheme = theme; });
        }

        #endregion

        private void On_ThoatApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}