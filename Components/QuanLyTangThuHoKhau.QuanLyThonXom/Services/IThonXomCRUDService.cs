using System.Collections.Generic;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyThonXom.Services
{
    public interface IThonXomCRUDService
    {
        public Task<List<ThonXom>> LietKeToanBoThonXom();

        public Task ThemThonXomMoi(string tenThonXom, DonViHanhChinhChung donViHanhChinhXaPhuong);
        public Task ThemThonXomMoi(ThonXom thonXomMoi);

        public Task ThayDoiTenThonXomDaCo(int idThonXomDaCo, string tenThonXom);

        public Task XoaThonXomDaCo(int idThonXomDaCo);

        public Task XoaTatCaDuLieu();
    }
}