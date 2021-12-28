using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.AppServices.SampleDataServices
{
    public class TuiHSCTSampleData
    {
        private static TuiHSCTSampleData _sampleData;

        private static TuiHSCTSampleData SampleData
        {
            get
            {
                if (_sampleData == null)
                {
                    _sampleData = new TuiHSCTSampleData();
                }

                return _sampleData;
            }
        }

        private List<TuiHSCT> _toanBoTuiHSCT;

        public TuiHSCTSampleData()
        {
            var randNgay = new Random();
            var randThang = new Random();

            Func<DateTime> taoNgayThangNgauNhien = () =>
            {
                var thang = randThang.Next(1, 12);

                var thang31Ngay = new List<int>() { 1, 3, 5, 7, 8, 10, 12 };
                var thang30Ngay = new List<int>() { 4, 6, 9, 11 };
                var thang28Ngay = new List<int>() { 2 };

                int ngay = 1;
                if (thang28Ngay.Contains(thang))
                {
                    ngay = randNgay.Next(1, 28);
                }

                if (thang30Ngay.Contains(thang))
                {
                    ngay = randNgay.Next(1, 30);
                }

                if (thang31Ngay.Contains(thang))
                {
                    ngay = randNgay.Next(1, 31);
                }

                return new DateTime(2021, thang, ngay);
            };

            var randSoLuongTuiHSCT = new Random();
            Func<int> taoSoLuongTuiHSCTNgauNhien = () => { return randSoLuongTuiHSCT.Next(50, 80); };

            _toanBoTuiHSCT = new List<TuiHSCT>();

            uint soHSCTTheoThuTu = 1;
            int idTheoThuTu = 0;

            foreach (var tapHSCT in TapHSCTSampleData.ToanBoHSCT())
            {
                var soLuongTuiHSCTTrongTapHSCT = taoSoLuongTuiHSCTNgauNhien();
                int viTriTui = 1;

                for (int i = 0; i < soLuongTuiHSCTTrongTapHSCT; i++)
                {
                    var hsct1 = new HSCT(soHSCTTheoThuTu++, tapHSCT.ThonXom, taoNgayThangNgauNhien());
                    var tuiHSCT = new TuiHSCT()
                    {
                        Id = idTheoThuTu++,
                        HSCT = hsct1,
                        TapHSCT = tapHSCT,
                        ViTriTui = viTriTui++
                    };

                    _toanBoTuiHSCT.Add(tuiHSCT);
                }
            }

            //Log toan bo du lieu
            Debug.WriteLine(JsonConvert.SerializeObject(_toanBoTuiHSCT));
        }

        public static List<TuiHSCT> ToanBoTuiHSCT()
        {
            return SampleData._toanBoTuiHSCT;
        }

        public static bool ThemTuiHSCTMoi(TuiHSCT tuiHSCTMoi)
        {
            var idTuiHSCTMoi = SampleData._toanBoTuiHSCT.Max(x => x.Id);
            tuiHSCTMoi.Id = idTuiHSCTMoi + 1;

            SampleData._toanBoTuiHSCT.Add(tuiHSCTMoi);
            return true;
        }

        public static bool ChinhSuaTuiHSCT(TuiHSCT tuiHSCTChinhSua)
        {
            var idTuiHSCTChinhSua = SampleData._toanBoTuiHSCT.FindIndex(x => x.Id == tuiHSCTChinhSua.Id);
            if (idTuiHSCTChinhSua < 0)
            {
                return false;
            }

            SampleData._toanBoTuiHSCT[idTuiHSCTChinhSua] = tuiHSCTChinhSua;
            return true;
        }

        public static bool XoaTuiHSCT(int idTuiHSCTCanXoa)
        {
            if (idTuiHSCTCanXoa < 0 || idTuiHSCTCanXoa>=SampleData._toanBoTuiHSCT.Count)
            {
                return false;
            }
            SampleData._toanBoTuiHSCT.RemoveAt(idTuiHSCTCanXoa);
            return true;
        }
    }
}