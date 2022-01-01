using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types
{
    [Serializable]
    public class DonViHanhChinhChung
    {
        public virtual string MaDonVi { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string TenDonVi { get; set; }

        [JsonProperty(PropertyName = "full_name")]
        public string TenDonViDuCap { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string TenLoaiCapDonVi { get; set; }
        public CapDonViHanhChinh LoaiCapDonVi { get; set; }

        public virtual List<DonViHanhChinhChung> CacDonViHanhChinhCapDuoi { get; set; }

        public override string ToString()
        {
            return TenDonViDuCap;
        }
    }
}