using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types
{
    [Serializable]
    public class DonViHanhChinhPhuongXa: DonViHanhChinhChung
    {
        [JsonProperty(PropertyName = "level3_id")]
        public override string MaDonVi { get; set; }

        public override List<DonViHanhChinhChung> CacDonViHanhChinhCapDuoi { get; set; }

        public DonViHanhChinhPhuongXa()
        {
            LoaiCapDonVi = CapDonViHanhChinh.PhuongXa;
        }
    }
}