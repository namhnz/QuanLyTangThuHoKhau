using System.Collections.Generic;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;

namespace QuanLyTangThuHoKhau.Core.AppServices.SampleDataServices
{
    public class XaPhuongSampleData
    {
        public DonViHanhChinhChung XaQuynhHoa()
        {
            var xaQuynhHoa = new DonViHanhChinhPhuongXa()
            {
                CacDonViHanhChinhCapDuoi = null,
                MaDonVi = "1"
            }

            var tinhNgheAn = new DonViHanhChinhTinhThanh()
            {
                CacDonViHanhChinhCapDuoi = new List<DonViHanhChinhChung>(),
                MaDonVi = "40",
                TenDonVi = "Tỉnh Nghệ An",
                TenDonViDuCap = "Tỉnh Nghệ An",
                LoaiCapDonVi = CapDonViHanhChinh.TinhThanh,
                TenLoaiCapDonVi = "Tỉnh"
            };
        }
    }
}