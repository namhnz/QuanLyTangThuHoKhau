using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types
{
    public class DVHCVNJsonFileRootDataType
    {
        public List<DVHCVNJsonFileLV1DataType> data { get; set; }

        public List<DonViHanhChinhChung> ExportContent()
        {
            return data.Select(x => x.Export()).ToList();
        }
    }

    public class DVHCVNJsonFileLV1DataType
    {
        public string level1_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<DVHCVNJsonFileLV2DataType> level2s { get; set; }

        public DonViHanhChinhChung Export()
        {
            var tenDonViDuCap = name;

            var donViHanhChinhTinhThanh = new DonViHanhChinhTinhThanh()
            {
                MaDonVi = level1_id,
                TenDonVi = name,
                TenDonViDuCap = tenDonViDuCap,
                TenLoaiCapDonVi = type,
                CacDonViHanhChinhCapDuoi = level2s.Select(x => x.Export(tenDonViDuCap)).ToList()
            };
            return donViHanhChinhTinhThanh;
        }
    }

    public class DVHCVNJsonFileLV2DataType
    {
        public string level2_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<DVHCVNJsonFileLV3DataType> level3s { get; set; }

        public DonViHanhChinhChung Export(string tenDonViHanhChinhCapTren)
        {
            var tenDonViDuCap = $"{name}, {tenDonViHanhChinhCapTren}";

            var donViHanhChinhQuanHuyen = new DonViHanhChinhQuanHuyen()
            {
                MaDonVi = level2_id,
                TenDonVi = name,
                TenLoaiCapDonVi = type,
                TenDonViDuCap = tenDonViDuCap,
                CacDonViHanhChinhCapDuoi = level3s.Select(x => x.Export(tenDonViDuCap)).ToList()
            };
            return donViHanhChinhQuanHuyen;
        }
    }

    public class DVHCVNJsonFileLV3DataType
    {
        public string level3_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }

        public DonViHanhChinhChung Export(string tenDonViHanhChinhCapTren)
        {
            var tenDonViDuCap = $"{name}, {tenDonViHanhChinhCapTren}";

            var donViHanhChinhPhuongXa = new DonViHanhChinhPhuongXa()
            {
                MaDonVi = level3_id,
                TenDonVi = name,
                TenDonViDuCap = tenDonViDuCap,
                TenLoaiCapDonVi = type,
            };
            return donViHanhChinhPhuongXa;
        }
    }
}