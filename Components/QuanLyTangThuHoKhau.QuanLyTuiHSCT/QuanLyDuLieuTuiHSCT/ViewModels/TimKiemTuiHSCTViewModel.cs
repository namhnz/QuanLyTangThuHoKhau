﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CustomMVVMDialogs;
using log4net;
using ModernWpf.Controls;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using QuanLyTangThuHoKhau.Core.AppServices.SampleDataServices;
using QuanLyTangThuHoKhau.Core.Exceptions;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.Core.Ultis;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.ViewModels;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Views;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Exceptions.TimKiemTuiHSCTExceptions;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.ViewModels
{
    public class TimKiemTuiHSCTViewModel : BindableBase, INavigationAware
    {
        private readonly ITuiHSCTCRUDService _tuiHSCTService;
        private readonly IDialogService _dialogService;

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TimKiemTuiHSCTViewModel(ITuiHSCTCRUDService tuiHSCTService, IDialogService dialogService)
        {
            _tuiHSCTService = tuiHSCTService;
            _dialogService = dialogService;
            InitCommands();
            InitData();
        }

        #region Tim kiem

        private int _soHSCTRutGonCanTim;

        public int SoHSCTRutGonCanTim
        {
            get => _soHSCTRutGonCanTim;
            set => SetProperty(ref _soHSCTRutGonCanTim, value);
        }

        #endregion

        #region Hien thi template ket qua

        private LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT _dataTemplateHienThiKetQuaTimKiem;

        public LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT DataTemplateHienThiKetQuaTimKiem
        {
            get => _dataTemplateHienThiKetQuaTimKiem;
            set => SetProperty(ref _dataTemplateHienThiKetQuaTimKiem, value);
        }

        #endregion

        #region Hien thi ket qua tim kiem

        // private string _ketQuaSoHSCTDayDu;
        //
        // public string KetQuaSoHSCTDayDu
        // {
        //     get => _ketQuaSoHSCTDayDu;
        //     set => SetProperty(ref _ketQuaSoHSCTDayDu, value);
        // }
        //
        // private ThonXom _ketQuaDiaChiHoThuongTru;
        //
        // public ThonXom KetQuaDiaChiHoThuongTru
        // {
        //     get => _ketQuaDiaChiHoThuongTru;
        //     set => SetProperty(ref _ketQuaDiaChiHoThuongTru, value);
        // }
        //
        // private int _ketQuaThuTuTapHSCT;
        //
        // public int KetQuaThuTuTapHSCT
        // {
        //     get => _ketQuaThuTuTapHSCT;
        //     set => SetProperty(ref _ketQuaThuTuTapHSCT, value);
        // }
        //
        // private int _ketQuaViTriTuiHSCT;
        //
        // public int KetQuaViTriTuiHSCT
        // {
        //     get => _ketQuaViTriTuiHSCT;
        //     set => SetProperty(ref _ketQuaViTriTuiHSCT, value);
        // }
        //
        // private string _ketQuaHoTenChuHo;
        //
        // public string KetQuaHoTenChuHo
        // {
        //     get => _ketQuaHoTenChuHo;
        //     set => SetProperty(ref _ketQuaHoTenChuHo, value);
        // }
        //
        // private DateTime? _ketQuaNgayDangKy;
        //
        // public DateTime? KetQuaNgayDangKy
        // {
        //     get => _ketQuaNgayDangKy;
        //     set => SetProperty(ref _ketQuaNgayDangKy, value);
        // }

        private TuiHSCT _ketQuaTuiHSCTTimThay;

        public TuiHSCT KetQuaTuiHSCTTimThay
        {
            get => _ketQuaTuiHSCTTimThay;
            set => SetProperty(ref _ketQuaTuiHSCTTimThay, value);
        }

        #endregion

        #region Hien thi loi

        private string _errorText;

        public string ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }

        #endregion

        #region Khoi tao

        public ICommand TimKiemThongTinHSCTCommand { get; private set; }

        private async void TimKiemThongTinHSCT()
        {
            ErrorText = null;

            try
            {
                //Kiem tra dieu kien
                if (SoHSCTRutGonCanTim <= 0)
                {
                    DataTemplateHienThiKetQuaTimKiem = LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT
                        .ChuaNhapSoHSCTDeTimKiemDataTemplate;

                    throw new SoHSCTKhongDungException()
                    {
                        ErrorMessage = "Số HSCT cần tìm không đúng"
                    };
                }

                //Lay thong tin ho so
                var ketQuaTuiHSCT = await _tuiHSCTService.TimKiemTuiHSCTTheoSoHSCT(SoHSCTRutGonCanTim);
                if (ketQuaTuiHSCT == null)
                {
                    // throw new SoHSCTKhongDungException()
                    // {
                    //     ErrorMessage = "Không tìm thấy hộ thường trú nào có số HSCT như trên"
                    // };
                    DataTemplateHienThiKetQuaTimKiem =
                        LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT.KhongTimThayTuiHSCTDataTemplate;
                    return;
                }

                // var ketQuaTapHSCT = ketQuaTuiHSCT.TapHSCT;

                //Hien thi cac gia tri
                // KetQuaSoHSCTDayDu = ketQuaTuiHSCT.HSCT.MaHSCTDayDu;
                // KetQuaDiaChiHoThuongTru = ketQuaTapHSCT.ThonXom;
                // KetQuaThuTuTapHSCT = ketQuaTapHSCT.ThuTuTapHSCT;
                // KetQuaViTriTuiHSCT = ketQuaTuiHSCT.ViTriTui;
                // KetQuaHoTenChuHo = ketQuaTuiHSCT.HSCT.ChuHo;
                // KetQuaNgayDangKy = ketQuaTuiHSCT.HSCT.NgayDangKy;
                KetQuaTuiHSCTTimThay = ketQuaTuiHSCT;

                //Thay doi template hien thi
                DataTemplateHienThiKetQuaTimKiem = LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT
                    .HienThiThongTinTuiHSCTTimThayDataTemplate;
            }
            catch (Exception ex)
            {
                if (ex is SoHSCTKhongDungException)
                {
                    ErrorText = ((BaseException)ex).ErrorMessage;
                }
                else
                {
                    Log.Error(ex);
                    MessageBox.Show("Đã có lỗi xảy ra khi tìm kiếm thông tin hộ thường trú");
                }
            }
        }

        private void InitCommands()
        {
            TimKiemThongTinHSCTCommand = new DelegateCommand(TimKiemThongTinHSCT);

            ShowChinhSuaTuiHSCTCustomContentDialogCommand =
                new DelegateCommand(() => ShowChinhSuaTuiHSCTCustomContentDialog(false));

            ShowXoaThuongTruTuiHSCTCustomContentDialogCommand =
                new DelegateCommand(() => ShowChinhSuaTuiHSCTCustomContentDialog(true));
        }

        private void InitData()
        {
            DataTemplateHienThiKetQuaTimKiem =
                LoaiDataTemplateHienThiKetQuaTimKiemTuiHSCT.ChuaNhapSoHSCTDeTimKiemDataTemplate;
        }

        #endregion

        #region Dieu huong

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["TuiHSCTCanHienThiChiTiet"] is TuiHSCT tuiHSCTTruyenTuViewKhac)
            {
                SoHSCTRutGonCanTim = tuiHSCTTruyenTuViewKhac.HSCT.SoHSCT;
                TimKiemThongTinHSCT();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // throw new NotImplementedException();
        }

        #endregion

        #region Chinh sua thong tin ho thuong tru tim thay

        public ICommand ShowChinhSuaTuiHSCTCustomContentDialogCommand { get; private set; }

        private async void ShowChinhSuaTuiHSCTCustomContentDialog(bool isXoaThuongTru)
        {
            var tuiHSCTCanChinhSua = KetQuaTuiHSCTTimThay.DeepClone();

            var dialogViewModel = new ChinhSuaTuiHSCTCustomContentDialogViewModel();

            dialogViewModel.SoHSCT = tuiHSCTCanChinhSua.HSCT.MaHSCTDayDu;
            dialogViewModel.ChuHo = tuiHSCTCanChinhSua.HSCT.ChuHo;
            dialogViewModel.DangThuongTru = isXoaThuongTru ? false : tuiHSCTCanChinhSua.HSCT.DangThuongTru;

            var dialogResult =
                await _dialogService.ShowCustomContentDialogAsync<ChinhSuaTuiHSCTCustomContentDialog>(
                    dialogViewModel);

            if (dialogResult == ContentDialogResult.Primary)
            {
                try
                {
                    //Lay cac thong tin da chinh sua
                    tuiHSCTCanChinhSua.HSCT.ChuHo = dialogViewModel.ChuHo;
                    tuiHSCTCanChinhSua.HSCT.DangThuongTru = dialogViewModel.DangThuongTru;

                    //Chinh sua tap ho so
                    await _tuiHSCTService.CapNhatThongTinTuiHSCT(tuiHSCTCanChinhSua);

                    MessageBox.Show("Chỉnh sửa tập hồ sơ gốc trong thôn, xóm thành công");

                    // Debug.WriteLine(JsonConvert.SerializeObject(TuiHSCTSampleData.ToanBoTuiHSCT()
                    //     .FirstOrDefault(x => x.Id == tuiHSCTCanChinhSua.Id)));
                    TimKiemThongTinHSCT();
                }
                catch (Exception ex)
                {
                    if (ex is TenChuHoKhongDungException or TuiHSCTKhongTonTaiException)
                    {
                        var exBase = (BaseException)ex;
                        MessageBox.Show(exBase.ErrorMessage);
                    }
                    else
                    {
                        Log.Error(ex);
                        MessageBox.Show("Đã có lỗi xảy ra khi chỉnh sửa thông tin hộ thường trú");
                    }
                }
            }
            else
            {
                // Debug.WriteLine("secondary or none");
            }
        }

        public ICommand ShowXoaThuongTruTuiHSCTCustomContentDialogCommand { get; private set; }

        #endregion
    }
}