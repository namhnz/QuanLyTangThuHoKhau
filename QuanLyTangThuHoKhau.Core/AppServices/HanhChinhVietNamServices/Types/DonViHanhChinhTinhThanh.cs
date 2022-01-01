using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types
{
    //https://stackoverflow.com/questions/8796618/how-can-i-change-property-names-when-serializing-with-json-net/8796648
    //https://stackoverflow.com/questions/6550409/how-to-add-attributes-to-a-base-classs-properties
    [Serializable]
    public class DonViHanhChinhTinhThanh: DonViHanhChinhChung
    {
        [JsonProperty(PropertyName = "level1_id")]
        public override string MaDonVi { get; set; }

        [JsonProperty(PropertyName = "level2s")]
        public override List<DonViHanhChinhChung> CacDonViHanhChinhCapDuoi { get; set; }

        public DonViHanhChinhTinhThanh()
        {
            LoaiCapDonVi = CapDonViHanhChinh.TinhThanh;
        }
    }
}