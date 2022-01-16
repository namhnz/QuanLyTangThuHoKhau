﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using CustomMVVMDialogs;
using log4net;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.Core.Types.KhoiTaoDuLieuBanDau;
using QuanLyTangThuHoKhau.Core.Ultis;
using QuanLyTangThuHoKhau.Core.Ultis.CommonContentDialogs;

namespace QuanLyTangThuHoKhau.QuanLyThonXom.KhoiTaoCacThonXom.ViewModels
{
    public class KhoiTaoDanhSachThonXomViewModel : BindableBase, INavigationAware
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;

        private readonly IDonViHanhChinhService _dvhcService;

        public KhoiTaoDanhSachThonXomViewModel(IDonViHanhChinhService dvhcService,
            IRegionManager regionManager, IDialogService dialogService)
        {
            _dvhcService = dvhcService;
            _regionManager = regionManager;
            _dialogService = dialogService;

            InitCommands();

            InitData();
        }

        #region Khoi tao

        private async void InitData()
        {
            CacThonXomThuocXaPhuongDaChon = new ObservableCollection<ThonXom>();

            _toanBoXaPhuongVietNam = (await _dvhcService.LoadToanBoXaPhuongVietNam()).ToList();
        }

        private void InitCommands()
        {
            //1


            //2
            XoaThonXomItemCommand = new DelegateCommand<ThonXom>(XoaThonXomItem);
            ThemThonXomItemCommand =
                new DelegateCommand<string>(ThemThonXomItem,
                        s => DonViXaPhuongDaChon != null)
                    .ObservesProperty(() => DonViXaPhuongDaChon);

            //3
            ChuyenBuocKhoiTaoCacTapHSCTGocCommand =
                new DelegateCommand(ChuyenBuocKhoiTaoCacTapHSCTGoc, () => CacThonXomThuocXaPhuongDaChon.Count > 0)
                    .ObservesProperty(() => CacThonXomThuocXaPhuongDaChon.Count);
        }

        #endregion

        #region Lua chon don vi xa, phuong

        private List<DonViHanhChinhChung> _toanBoXaPhuongVietNam;

        private DonViHanhChinhChung _donViXaPhuongDaChon;

        public DonViHanhChinhChung DonViXaPhuongDaChon
        {
            get => _donViXaPhuongDaChon;
            set
            {
                if (_donViXaPhuongDaChon != value)
                {
                    SetProperty(ref _donViXaPhuongDaChon, value);
                    DeleteAllThonXom();
                }
            }
        }

        public void XacDinhDonViXaPhuongDaChon(DonViHanhChinhChung donviXaPhuongDaChon)
        {
            DonViXaPhuongDaChon = donviXaPhuongDaChon;
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

        #endregion


        #region Quan ly cac thon, xom

        //Danh sach the view model dung de hien thi
        private ObservableCollection<ThonXom> _cacThonXomThuocXaPhuongDaChon;

        public ObservableCollection<ThonXom> CacThonXomThuocXaPhuongDaChon
        {
            get => _cacThonXomThuocXaPhuongDaChon;
            set
            {
                if (value is ObservableCollection<ThonXom> list)
                {
                    SetProperty(ref _cacThonXomThuocXaPhuongDaChon, list);
                }
                else
                {
                    _cacThonXomThuocXaPhuongDaChon = new ObservableCollection<ThonXom>();
                }
            }
        }

        public ICommand XoaThonXomItemCommand { get; private set; }

        private async void XoaThonXomItem(ThonXom thonXomItem)
        {
            var xoaThonXomItemKhoiDanhSachConfirmResult = await ReducedYesNoConfirmContentDialog.Show(_dialogService,
                "Bạn có muốn xoá thôn, xóm này khỏi danh sách các thôn, xóm đã nhập không?");


            if (xoaThonXomItemKhoiDanhSachConfirmResult == ContentDialogResult.Primary)
            {
                var indexCanXoa = CacThonXomThuocXaPhuongDaChon.IndexOf(thonXomItem);

                CacThonXomThuocXaPhuongDaChon.RemoveAt(indexCanXoa);
            }
        }

        private void DeleteAllThonXom()
        {
            CacThonXomThuocXaPhuongDaChon.Clear();
        }

        public ICommand ThemThonXomItemCommand { get; private set; }

        private async void ThemThonXomItem(string tenThonXomItem)
        {
            try
            {
                tenThonXomItem = tenThonXomItem.Trim().EnsureStringHasSingleSpace();

                if (string.IsNullOrEmpty(tenThonXomItem))
                {
                    await ReducedDisplayInfoContentDialog.Show(_dialogService, "Tên thôn, xóm thêm mới không đúng");
                    return;
                }

                if (DonViXaPhuongDaChon == null || DonViXaPhuongDaChon.LoaiCapDonVi != CapDonViHanhChinh.PhuongXa)
                {
                    await ReducedDisplayInfoContentDialog.Show(_dialogService, "Lựa chọn đơn vị hành chính của thôn, xóm thêm mới không phải cấp xã, phường");
                    return;
                }

                var thonXomItemMoi = new ThonXom()
                {
                    TenThonXom = tenThonXomItem,
                    DonViHanhChinhPhuongXa = DonViXaPhuongDaChon
                };

                //Them thon, xom moi vao danh sach
                var isThonXomItemMoiDaDuocThemVaoDanhSach =
                    CacThonXomThuocXaPhuongDaChon.Any(x => x.TenThonXom == tenThonXomItem);
                if (isThonXomItemMoiDaDuocThemVaoDanhSach)
                {
                    await ReducedDisplayInfoContentDialog.Show(_dialogService, "Thôn, xóm này đã được thêm vào danh sách các thôn, xóm thuộc xã, phường đã chọn");
                }
                else
                {
                    CacThonXomThuocXaPhuongDaChon.Add(thonXomItemMoi);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                await ReducedDisplayInfoContentDialog.Show(_dialogService, "Đã có lỗi xảy ra khi thêm thôn, xóm mới");
            }
        }

        #endregion

        #region Dieu huong view khac

        public ICommand ChuyenBuocKhoiTaoCacTapHSCTGocCommand { get; private set; }

        private void ChuyenBuocKhoiTaoCacTapHSCTGoc()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.AddNavigationSource(this);

            navigationParameters.Add("DanhSachThonXom", CacThonXomThuocXaPhuongDaChon.ToList());

            _regionManager.RequestNavigate(KhoiTaoDuLieuBanDauRegionNames.KHOI_TAO_DU_LIEU_BAN_DAU_ROOT_REGION,
                "KhoiTaoCacTapHSCTView", navigationParameters);
        }

        #endregion

        #region Dieu huong

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        #endregion
    }
}