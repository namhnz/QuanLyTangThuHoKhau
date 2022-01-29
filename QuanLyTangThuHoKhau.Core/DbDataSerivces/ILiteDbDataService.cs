using QuanLyTangThuHoKhau.Core.DbRepositories.DiaChiRepos;
using QuanLyTangThuHoKhau.Core.DbRepositories.HoSoCuTruRepos;

namespace QuanLyTangThuHoKhau.Core.DbDataSerivces
{
    public interface ILiteDbDataService
    {
        public ILiteDbTuiHSCTRepository TuiHSCTRepository { get; }
        public ILiteDbTapHSCTRepository TapHSCTRepository { get; }
        public ILiteDbThonXomRepository ThonXomRepository { get; }
    }
}