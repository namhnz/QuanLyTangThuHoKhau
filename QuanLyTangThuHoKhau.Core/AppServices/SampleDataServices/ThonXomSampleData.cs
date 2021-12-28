using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.AppServices.SampleDataServices
{
    public class ThonXomSampleData
    {
        public ThonXomSampleData()
        {
            var thon1 = new ThonXom()
            {
                Id = 0,
                DonViHanhChinhPhuongXa = XaPhuongSampleData.XaQuynhHoa(),
                TenThonXom = "Thôn 1"
            };
            var thon2 = new ThonXom()
            {
                Id = 1,
                DonViHanhChinhPhuongXa = XaPhuongSampleData.XaQuynhHoa(),
                TenThonXom = "Thôn 2"
            };
            var thon3 = new ThonXom()
            {
                Id = 2,
                DonViHanhChinhPhuongXa = XaPhuongSampleData.XaQuynhHoa(),
                TenThonXom = "Thôn 3"
            };
            var thon4 = new ThonXom()
            {
                Id = 3,
                DonViHanhChinhPhuongXa = XaPhuongSampleData.XaQuynhHoa(),
                TenThonXom = "Thôn 4"
            };

            _danhSachThonXom = new List<ThonXom>()
            {
                thon1, thon2, thon3, thon4
            };

            //Log toan bo du lieu
            Debug.WriteLine(JsonConvert.SerializeObject(_danhSachThonXom));
        }

        private readonly List<ThonXom> _danhSachThonXom;

        private static ThonXomSampleData _sampleData;

        private static ThonXomSampleData SampleData
        {
            get
            {
                if (_sampleData == null)
                {
                    _sampleData = new ThonXomSampleData();
                }

                return _sampleData;
            }

        }

        public static List<ThonXom> ToanBoThonXom()
        {
            return SampleData._danhSachThonXom;
        }
    }
}