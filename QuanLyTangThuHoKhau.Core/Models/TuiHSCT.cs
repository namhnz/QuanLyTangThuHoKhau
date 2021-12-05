using LiteDB;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Database;

namespace QuanLyTangThuHoKhau.Core.Models
{
    public class TuiHSCT
    {
        [BsonId] public int Id { get; set; }
        public HSCT HSCT { get; set; }

        [BsonRef(DataReposNames.CAC_TAP_HSCT)] public TapHSCT TapHSCT { get; set; }

        public int ViTriTui { get; set; }
    }
}