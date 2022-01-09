using System.Collections.Generic;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyThonXom.Services
{
    public interface IThonXomCRUDService
    {
        #region Tim kiem

        public Task<List<ThonXom>> LietKeToanBoThonXom();

        #endregion

        #region Them moi

        public Task ThemThonXomMoi(ThonXom thonXomMoi);
        public Task ThemThonXomMoi(string tenThonXom, DonViHanhChinhChung donViHanhChinhXaPhuong);

        public Task<int> ThemNhieuThonXomMoi(List<ThonXom> cacThonXomMoi);
        #endregion

        #region Chinh sua

        public Task ThayDoiTenThonXomDaCo(int idThonXomDaCo, string tenThonXom);

        #endregion

        #region Xoa

        public Task XoaThonXomDaCo(int idThonXomDaCo);

        public Task XoaTatCaDuLieu();

        #endregion
    }
}