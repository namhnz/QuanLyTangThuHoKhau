using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.AppServices.SampleDataServices
{
    public class TapHSCTSampleData
    {
        private static TapHSCTSampleData _sampleData;

        private static TapHSCTSampleData SampleData
        {
            get
            {
                if (_sampleData == null)
                {
                    _sampleData = new TapHSCTSampleData();
                }

                return _sampleData;
            }
        }

        private readonly List<TapHSCT> _toanBoTapHSCT;

        public TapHSCTSampleData()
        {
            //Thon 1 hien tai co 2 tap
            var tap1Thon1 = new TapHSCT()
            {
                Id = 0,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[0],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTGoc,
                ThuTuTapHSCT = 1
            };
            var tap2Thon1 = new TapHSCT()
            {
                Id = 1,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[0],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTGoc,
                ThuTuTapHSCT = 2
            };
            var tap2Thon1BoSung = new TapHSCT()
            {
                Id = 2,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[0],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTBoSung,
                ThuTuTapHSCT = 2
            };

            //Thon 2 hien tai co 3 tap
            var tap1Thon2 = new TapHSCT()
            {
                Id = 3,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[1],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTGoc,
                ThuTuTapHSCT = 1
            };
            var tap2Thon2 = new TapHSCT()
            {
                Id = 4,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[1],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTGoc,
                ThuTuTapHSCT = 2
            };
            var tap3Thon2 = new TapHSCT()
            {
                Id = 5,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[1],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTGoc,
                ThuTuTapHSCT = 3
            };
            var tap3Thon2BoSung = new TapHSCT()
            {
                Id = 6,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[1],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTBoSung,
                ThuTuTapHSCT = 3
            };

            //Thon 3 hien tai co 1 tap
            var tap1Thon3 = new TapHSCT()
            {
                Id = 7,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[2],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTGoc,
                ThuTuTapHSCT = 1
            };
            var tap1Thon3BoSung = new TapHSCT()
            {
                Id = 8,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[2],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTBoSung,
                ThuTuTapHSCT = 1
            };

            //Thon 4 hien tai co 2 tap
            var tap1Thon4 = new TapHSCT()
            {
                Id = 9,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[3],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTGoc,
                ThuTuTapHSCT = 1
            };
            var tap2Thon4 = new TapHSCT()
            {
                Id = 10,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[3],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTGoc,
                ThuTuTapHSCT = 2
            };
            var tap2Thon4BoSung = new TapHSCT()
            {
                Id = 11,
                ThonXom = ThonXomSampleData.ToanBoThonXom()[3],
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTBoSung,
                ThuTuTapHSCT = 2
            };

            _toanBoTapHSCT = new List<TapHSCT>()
            {
                tap1Thon1, tap2Thon1, tap2Thon1BoSung,
                tap1Thon2, tap2Thon2, tap3Thon2, tap3Thon2BoSung,
                tap1Thon3, tap1Thon3BoSung,
                tap1Thon4, tap2Thon4, tap2Thon4BoSung
            };

            //Log toan bo du lieu
            Debug.WriteLine(JsonConvert.SerializeObject(_toanBoTapHSCT));
        }

        public static List<TapHSCT> ToanBoHSCT()
        {
            return SampleData._toanBoTapHSCT;
        }
    }
}