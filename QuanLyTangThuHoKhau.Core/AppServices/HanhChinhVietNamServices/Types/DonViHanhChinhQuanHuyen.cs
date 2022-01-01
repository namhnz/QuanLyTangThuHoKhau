using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types
{
    [Serializable]
    public class DonViHanhChinhQuanHuyen : DonViHanhChinhChung
    {
        [JsonProperty(PropertyName = "level2_id")]
        public override string MaDonVi { get; set; }

        [JsonProperty(PropertyName = "level3s")]
        public override List<DonViHanhChinhChung> CacDonViHanhChinhCapDuoi { get; set; }

        public DonViHanhChinhQuanHuyen()
        {
            LoaiCapDonVi = CapDonViHanhChinh.QuanHuyen;
        }
    }
}