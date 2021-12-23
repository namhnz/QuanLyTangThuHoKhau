using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModernWpf;
using ModernWpf.Controls;
using Frame = System.Windows.Controls.Frame;

namespace QuanLyTangThuHoKhau.QuanLyDuLieu.Views
{
    /// <summary>
    /// Interaction logic for QuanLyDuLieuRootView.xaml
    /// </summary>
    public partial class QuanLyDuLieuRootView : UserControl
    {
        public static QuanLyDuLieuRootView Current
        {
            get => _current.Value;
            private set => _current.Value = value;
        }

       private static readonly ThreadLocal<QuanLyDuLieuRootView> _current = new ThreadLocal<QuanLyDuLieuRootView>();

        private bool _ignoreSelectionChange;
        private readonly ControlPagesData _controlPagesData = new ControlPagesData();
        private Type _startPage;

        public QuanLyDuLieuRootView()
        {
            InitializeComponent(); 

            Current = this;

            SetStartPage();
            if (_startPage != null)
            {
                PagesList.SelectedItem =
                    PagesList.Items.OfType<ControlInfoDataItem>().FirstOrDefault(x => x.PageType == _startPage);
            }

            NavigateToSelectedPage();

            
        }

        partial void SetStartPage();

        private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine(nameof(ContextMenu_Loaded));
            var menu = (ContextMenu)sender;
            var tabItem = (TabItem)menu.PlacementTarget;
            var content = (FrameworkElement)tabItem.Content;
        }
        
        
        private void NavigateToSelectedPage()
        {
            if (PagesList.SelectedValue is Type type)
            {
                RootFrame?.Navigate(type);
            }
        }

        private void PagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_ignoreSelectionChange)
            {
                NavigateToSelectedPage();
            }
        }
        //
        // private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        // {
        //     if (e.NavigationMode == NavigationMode.Back)
        //     {
        //         RootFrame.RemoveBackEntry();
        //     }
        // }
        //
        // private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        // {
        //     Debug.Assert(!RootFrame.CanGoForward);
        //
        //     _ignoreSelectionChange = true;
        //     PagesList.SelectedValue = RootFrame.CurrentSourcePageType;
        //     _ignoreSelectionChange = false;
        // }
    }

}