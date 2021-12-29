using System;
using System.Linq;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.Types.QuanLyDuLieu;
using QuanLyTangThuHoKhau.QuanLyDuLieu.Types;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCTMoi.Views;

namespace QuanLyTangThuHoKhau.QuanLyDuLieu.ViewModels
{
    public class QuanLyDuLieuRootViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private Type _startView;

        public QuanLyDuLieuRootViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            InitData();
        }

        #region Khoi tao

        private void InitData()
        {
            _startView = typeof(TimKiemTuiHSCTView);

            CacChucNangViewList = new ViewNavigationListData();
            SelectedViewHienThi = CacChucNangViewList.FirstOrDefault(x => x.ViewType == _startView)?.ViewType;

            _regionManager.RegisterViewWithRegion(QuanLyDuLieuRegionNames.QUAN_LY_DU_LIEU_ROOT_REGION, _startView);
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
                if (_selectedViewHienThi != value)
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