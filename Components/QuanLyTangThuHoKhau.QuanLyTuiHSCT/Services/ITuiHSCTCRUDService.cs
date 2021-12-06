using System.Collections.Generic;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services
{
    public interface ITuiHSCTCRUDService
    {
        public Task<List<TuiHSCT>> LietKeToanBoThonXom();
        public Task ThemTuiHSCTMoi(string tenThonXom, DonViHanhChinhChung donViHanhChinhXaPhuong);
        public Task ThayDoiTenThonXomDaCo(int idThonXomDaCo, string tenThonXom);
        public Task XoaTuiHSCT(int idTuiHSCT);
    }
}