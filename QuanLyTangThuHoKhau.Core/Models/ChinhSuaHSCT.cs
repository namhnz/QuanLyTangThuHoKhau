using System;
using LiteDB;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Database;

namespace QuanLyTangThuHoKhau.Core.Models
{
    [Serializable]
    public class ChinhSuaHSCT
    {
        [BsonId] public int Id { get; set; }
        public string ThongTinChinhSua { get; set; }
        public DateTime ThoiGianChinhSua { get; set; }

        [BsonRef(DataReposNames.CAC_TUI_HSCT)] public TuiHSCT TuiHSCT { get; set; }

    }
}