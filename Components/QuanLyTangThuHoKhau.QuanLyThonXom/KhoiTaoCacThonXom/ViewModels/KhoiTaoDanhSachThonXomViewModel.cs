using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using log4net;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.Core.Types.KhoiTaoDuLieuBanDau;
using QuanLyTangThuHoKhau.Core.Ultis;

namespace QuanLyTangThuHoKhau.QuanLyThonXom.KhoiTaoCacThonXom.ViewModels
{
    public class KhoiTaoDanhSachThonXomViewModel : BindableBase
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IRegionManager _regionManager;

        private readonly IDonViHanhChinhService _dvhcService;
        // private readonly IThonXomCRUDService _thonXomService;

        public KhoiTaoDanhSachThonXomViewModel(IDonViHanhChinhService dvhcService,
            IRegionManager regionManager /*, IThonXomCRUDService thonXomService*/)
        {
            _dvhcService = dvhcService;
            _regionManager = regionManager;
            // _thonXomService = thonXomService;

            InitCommands();

            InitData();
        }

        private List<DonViHanhChinhChung> _toanBoXaPhuongVietNam;

        private DonViHanhChinhChung _donViXaPhuongDaChon;

        public void XacDinhDonViXaPhuongDaChon(DonViHanhChinhChung donviXaPhuongDaChon)
        {
            _donViXaPhuongDaChon = donviXaPhuongDaChon;
        }

        public List<DonViHanhChinhChung> TimKiemCacXaPhuongTheoDieuKien(string query)
        {
            var querySplit = query.Split(' ');
            var suggestions = _toanBoXaPhuongVietNam.Where(
                item =>
                {
                    // Idea: check for every word entered (separated by space) if it is in the name,  
                    // e.g. for query "split button" the only result should "SplitButton" since its the only query to contain "split" and "button" 
                    // If any of the sub tokens is not in the string, we ignore the item. So the search gets more precise with more words 
                    bool flag = true;
                    foreach (string queryToken in querySplit)
                    {
                        // Check if token is not in string 
                        if (item.TenDonViDuCap.IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) < 0)
                        {
                            // Token is not in string, so we ignore this item. 
                            flag = false;
                        }
                    }

                    return flag;
                });
            return suggestions
                .OrderByDescending(i => i.TenDonViDuCap.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenBy(x => x.TenDonViDuCap).ToList();
        }

        private void InitCommands()
        {
            //1


            //2
            DeleteThonXomItemCommand = new DelegateCommand<ThonXom>(DeleteThonXomItem);
            ThemThonXomItemCommand = new DelegateCommand<string>(ThemThonXomItem);

            //3
            ChuyenBuocKhoiTaoCacTapHSCTGocCommand = new DelegateCommand(ChuyenBuocKhoiTaoCacTapHSCTGoc);
        }

        private async void InitData()
        {
            CacThonXomThuocXaPhuongDaChon = new ObservableCollection<ThonXom>();

            _cacThonXomThuocXaPhuongDaChonCollectionLock = new object();
            BindingOperations.EnableCollectionSynchronization(_cacThonXomThuocXaPhuongDaChon,
                _cacThonXomThuocXaPhuongDaChonCollectionLock);

            _toanBoXaPhuongVietNam = (await _dvhcService.LoadToanBoXaPhuongVietNam()).ToList();
        }


        #region Quan ly cac thon, xom

        //Cap nhat ObservableCollection tu thread khac ngoai UI thread
        private object _cacThonXomThuocXaPhuongDaChonCollectionLock;

        //Danh sach the view model dung de hien thi
        private ObservableCollection<ThonXom> _cacThonXomThuocXaPhuongDaChon;

        public ObservableCollection<ThonXom> CacThonXomThuocXaPhuongDaChon
        {
            get => _cacThonXomThuocXaPhuongDaChon;
            set => SetProperty(ref _cacThonXomThuocXaPhuongDaChon, value);
        }

        public ICommand DeleteThonXomItemCommand { get; private set; }

        private void DeleteThonXomItem(ThonXom thonXomItem)
        {
            var xoaThonXomItemKhoiDanhSachConfirmResult = MessageBox.Show(
                "Bạn có muốn xoá thôn, xóm này khỏi danh sách các thôn, xóm đã nhập không?", "Xoá thôn, xóm",
                MessageBoxButton.YesNo);

            if (xoaThonXomItemKhoiDanhSachConfirmResult == MessageBoxResult.Yes)
            {
                lock (_cacThonXomThuocXaPhuongDaChonCollectionLock)
                {
                    CacThonXomThuocXaPhuongDaChon.Remove(thonXomItem);
                }
            }
        }

        public ICommand ThemThonXomItemCommand { get; private set; }

        private void ThemThonXomItem(string tenThonXomItem)
        {
            try
            {
                tenThonXomItem = tenThonXomItem.Trim().EnsureStringHasSingleSpace();

                if (string.IsNullOrEmpty(tenThonXomItem))
                {
                    MessageBox.Show("Tên thôn, xóm thêm mới không đúng");
                    return;
                }

                if (_donViXaPhuongDaChon == null || _donViXaPhuongDaChon.LoaiCapDonVi != CapDonViHanhChinh.PhuongXa)
                {
                    MessageBox.Show("Lựa chọn đơn vị hành chính của thôn, xóm thêm mới không phải cấp xã, phường");
                    return;
                }

                var thonXomItemMoi = new ThonXom()
                {
                    TenThonXom = tenThonXomItem,
                    DonViHanhChinhPhuongXa = _donViXaPhuongDaChon
                };

                //Them thon, xom moi vao danh sach
                var isThonXomItemMoiDaDuocThemVaoDanhSach =
                    CacThonXomThuocXaPhuongDaChon.Any(x => x.TenThonXom == tenThonXomItem);
                if (isThonXomItemMoiDaDuocThemVaoDanhSach)
                {
                    MessageBox.Show("Thôn, xóm này đã được thêm vào danh sách các thôn, xóm thuộc xã, phường đã chọn");
                }
                else
                {
                    lock (_cacThonXomThuocXaPhuongDaChonCollectionLock)
                    {
                        CacThonXomThuocXaPhuongDaChon.Add(thonXomItemMoi);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageBox.Show("Đã có lỗi xảy ra khi thêm thôn, xóm mới");
            }
        }

        #endregion

        public ICommand ChuyenBuocKhoiTaoCacTapHSCTGocCommand { get; private set; }

        private void ChuyenBuocKhoiTaoCacTapHSCTGoc()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.AddNavigationSource(this);

            navigationParameters.Add("DanhSachThonXom", CacThonXomThuocXaPhuongDaChon.ToList());

            _regionManager.RequestNavigate(KhoiTaoDuLieuBanDauRegionNames.KHOI_TAO_DU_LIEU_BAN_DAU_ROOT_REGION,
                "KhoiTaoCacTapHSCTView", navigationParameters);
        }
    }
}