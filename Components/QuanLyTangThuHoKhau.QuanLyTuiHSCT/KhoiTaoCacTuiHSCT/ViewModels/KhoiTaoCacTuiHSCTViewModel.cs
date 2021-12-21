using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using log4net;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.KhoiTaoCacTuiHSCT.ViewModels
{
    public class KhoiTaoCacTuiHSCTViewModel: BindableBase
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDonViHanhChinhService _dvhcService;

        public KhoiTaoCacTuiHSCTViewModel(IDonViHanhChinhService dvhcService)
        {
            _dvhcService = dvhcService;

            InitData();
        }

        private List<TuiHSCT> TaoCacTuiHSCTTheoTapHSCTGocInit(TapHSCTGocInitModel tapHSCTGoc)
        {
            var thonXomChuaTapHSCT = tapHSCTGoc.ThonXom;

            var cacTuiHSCTTrongTapHSCT = new List<TuiHSCT>();
            for (int i = tapHSCTGoc.SoHSCTBatDau; i <= tapHSCTGoc.SoHSCTKetThuc; i++)
            {
                var soHSCT = i;

                var hsct = new HSCT((uint)soHSCT, thonXomChuaTapHSCT);

                var tuiHSCTMoi = new TuiHSCT()
                {
                    HSCT = hsct,
                    TapHSCT = (TapHSCT)tapHSCTGoc,
                    ViTriTui = soHSCT - tapHSCTGoc.SoHSCTBatDau + 1
                };

                // Debug.WriteLine(JsonConvert.SerializeObject(tuiHSCTMoi));

                cacTuiHSCTTrongTapHSCT.Add(tuiHSCTMoi);
            }

            return cacTuiHSCTTrongTapHSCT;
        }

        private async void InitData()
        {
            try
            {
                IsDangKhoiTaoCacTuiHSCT = true;

                // await Task.Delay(5000);

                var xaPhuongHienDangQuanLy =
                    (await _dvhcService.LoadToanBoXaPhuongVietNam()).First(x => x.TenDonVi.Contains("Quỳnh Hoa"));

                var thonXom = new ThonXom()
                {
                    DonViHanhChinhPhuongXa = xaPhuongHienDangQuanLy,
                    TenThonXom = "Thôn 1"
                };

                var tapHSCTGoc1 = new TapHSCTGocInitModel();
                tapHSCTGoc1.KhoiTaoCacGiaTriCuaTapHSCT(thonXom, 1, 1, 120);

                var cacTuiHSCTDuocTao = TaoCacTuiHSCTTheoTapHSCTGocInit(tapHSCTGoc1);
                SoLuongTuiHSCTDaKhoiTaoXong = cacTuiHSCTDuocTao.Count;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageBox.Show("Đã có lỗi xảy ra trong quá trình khởi tạo các túi HSCT");
            }
            finally
            {
                IsDangKhoiTaoCacTuiHSCT = false;
            }
            
        }

        private bool _isDangKhoiTaoCacTuiHSCT;

        public bool IsDangKhoiTaoCacTuiHSCT
        {
            get { return _isDangKhoiTaoCacTuiHSCT; }
            set { SetProperty(ref _isDangKhoiTaoCacTuiHSCT, value); }
        }

        private int _soLuongTuiHSCTDaKhoiTaoXong;

        public int SoLuongTuiHSCTDaKhoiTaoXong
        {
            get { return _soLuongTuiHSCTDaKhoiTaoXong; }
            set { SetProperty(ref _soLuongTuiHSCTDaKhoiTaoXong, value); }
        }

    }
}