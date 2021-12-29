using System;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.Types.QuanLyDuLieu;
using QuanLyTangThuHoKhau.QuanLyThaoTacDuLieu.Types;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views;

namespace QuanLyTangThuHoKhau.QuanLyThaoTacDuLieu.ViewModels
{
    public class QuanLyThaoTacDuLieuRootViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private Type _startView;
        // private bool _ignoreDoiViewHienThi;

        public QuanLyThaoTacDuLieuRootViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            InitData();
            InitCommands();
        }

        #region Khoi tao

        private void InitData()
        {
            _startView = typeof(TimKiemTuiHSCTView);

            CacChucNangViewList = new ViewNavigationListData();
            
        }

        private void InitCommands()
        {
            HienThiStartViewCommand = new DelegateCommand(HienThiStartView);
        }

        #endregion

        #region Hien thi view dau tien

        public ICommand HienThiStartViewCommand { get; private set; }

        public void HienThiStartView()
        {
            // _ignoreDoiViewHienThi = true;
            SelectedViewHienThi = CacChucNangViewList.FirstOrDefault(x => x.ViewType == _startView)?.ViewType;

            // _regionManager.RequestNavigate(QuanLyDuLieuRegionNames.QUAN_LY_DU_LIEU_ROOT_REGION, _startView.Name);
            // _regionManager.RegisterViewWithRegion(QuanLyDuLieuRegionNames.QUAN_LY_DU_LIEU_ROOT_REGION, _startView);
            // _regionManager.RegisterViewWithRegion(QuanLyDuLieuRegionNames.QUAN_LY_DU_LIEU_ROOT_REGION, typeof(TimKiemTuiHSCTView));
            // _ignoreDoiViewHienThi = false;
        }

        #endregion


        #region Dieu huong view

        private ViewNavigationListData _cacChucNangViewList;

        public ViewNavigationListData CacChucNangViewList
        {
            get => _cacChucNangViewList;
            set => SetProperty(ref _cacChucNangViewList, value);
        }

        private Type _selectedViewHienThi;

        public Type SelectedViewHienThi
        {
            get => _selectedViewHienThi;
            set
            {
                if (_selectedViewHienThi != value/* && !_ignoreDoiViewHienThi*/)
                {
                    DoiViewHienThi(value.Name);
                }

                SetProperty(ref _selectedViewHienThi, value);
            }
        }


        //Su dung trong phan code cua giao dien
        public void DoiViewHienThi(string viewName)
        {
            _regionManager.RequestNavigate(QuanLyDuLieuRegionNames.QUAN_LY_DU_LIEU_ROOT_REGION, viewName);
        }

        #endregion

    }
}