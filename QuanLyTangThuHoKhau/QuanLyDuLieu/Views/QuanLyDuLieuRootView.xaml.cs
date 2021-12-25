using System;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Prism.Ioc;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.Types.QuanLyDuLieu;
using QuanLyTangThuHoKhau.QuanLyDuLieu.Types;
using Unity;

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

        // private IRegionManager _regionManager;

       private static readonly ThreadLocal<QuanLyDuLieuRootView> _current = new ThreadLocal<QuanLyDuLieuRootView>();

        private bool _ignoreSelectionChange;
        private readonly ViewNavigationListData _controlPagesData = new ViewNavigationListData();
        private Type _startPage;

        public QuanLyDuLieuRootView()
        {
            InitializeComponent(); 

            Current = this;

            // _regionManager = ContainerLocator.Current.Resolve<IRegionManager>();
            // _regionManager = ContainerLocator.Container.Resolve<IRegionManager>();
            
            SetStartPage();
            if (_startPage != null)
            {
                ViewNavigationListView.SelectedItem =
                    ViewNavigationListView.Items.OfType<ViewInfoNavigationItem>()
                        .FirstOrDefault(x => x.ViewType == _startPage);
            }

            NavigateToSelectedPage();
            //https://github.com/PrismLibrary/Prism/issues/1805#issuecomment-495690608
            // var regionManager = ContainerLocator.Current.Resolve<IRegionManager>();
            // RegionManager.SetRegionManager(this, regionManager);
            // regionManager.RegisterViewWithRegion(QuanLyDuLieuRegionNames.QUAN_LY_DU_LIEU_ROOT_REGION, _startPage);
        }

        partial void SetStartPage();

        // private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
        // {
        //     //Debug.WriteLine(nameof(ContextMenu_Loaded));
        //     var menu = (ContextMenu)sender;
        //     var tabItem = (TabItem)menu.PlacementTarget;
        //     var content = (FrameworkElement)tabItem.Content;
        // }
        
        
        private void NavigateToSelectedPage()
        {
            if (ViewNavigationListView.SelectedValue is Type type)
            {
                var regionManager = ContainerLocator.Current.Resolve<IRegionManager>();

                regionManager.RequestNavigate(QuanLyDuLieuRegionNames.QUAN_LY_DU_LIEU_ROOT_REGION, type.Name);
            }
        }

        private void ViewNavigationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        //     ViewNavigationListView.SelectedValue = RootFrame.CurrentSourcePageType;
        //     _ignoreSelectionChange = false;
        // }
    }

}