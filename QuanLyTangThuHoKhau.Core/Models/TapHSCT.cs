using System;
using LiteDB;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Database;

namespace QuanLyTangThuHoKhau.Core.Models
{
    [Serializable]
    public class TapHSCT
    {
        [BsonId] public int Id { get; set; }

        public int ThuTuTapHSCT { get; set; }
        public LoaiTapHSCT LoaiTapHSCT { get; set; }

        [BsonRef(DataReposNames.CAC_THON_XOM)] public ThonXom ThonXom { get; set; }

        public override string ToString()
        {
            return $"Tập {ThuTuTapHSCT} - " + (LoaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTGoc ? "Gốc" : "Bổ sung");
        }
    }
}