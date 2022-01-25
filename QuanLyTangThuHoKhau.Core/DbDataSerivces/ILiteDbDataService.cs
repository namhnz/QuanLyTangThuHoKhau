using QuanLyTangThuHoKhau.Core.DbRepositories.DiaChiRepos;
using QuanLyTangThuHoKhau.Core.DbRepositories.HoSoCuTruRepos;
using QuanLyTangThuHoKhau.Core.DbRepositories.ThayDoiThongTinRepos;

namespace QuanLyTangThuHoKhau.Core.DbDataSerivces
{
    public interface ILiteDbDataService
    {
        public ILiteDbTuiHSCTRepository TuiHSCTRepository { get; }
        public ILiteDbTapHSCTRepository TapHSCTRepository { get; }
        public ILiteDbThonXomRepository ThonXomRepository { get; }
        public ILiteDbChinhSuaHSCTRepository ChinhSuaHSCTRepository { get; }
    }
}