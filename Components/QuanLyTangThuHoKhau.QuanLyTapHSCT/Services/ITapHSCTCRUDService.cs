using System.Collections.Generic;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.Services
{
    public interface ITapHSCTCRUDService
    {
        #region Tim kiem

        public Task<List<TapHSCT>> LietKeToanBoTapHSCT();
        public Task<List<TapHSCT>> LietKeToanBoTapHSCTTheoThonXom(ThonXom thonXom);
        public Task<TapHSCT> LayTapHSCTBoSungCuaThonXom(ThonXom thonXom);

        #endregion

        #region Them moi

        public Task ThemTapHSCTMoi(int thuTuTapHSCT, LoaiTapHSCT loaiTapHSCT, ThonXom thonXom);
        public Task ThemTapHSCTMoi(TapHSCT tapHSCTMoi);
        public Task<int> ThemNhieuTapHSCTMoi(List<TapHSCT> cacTapHSCTMoi);
        public Task ThemTapHSCTBoSung(ThonXom thonXom);

        #endregion

        #region Chinh sua

        #endregion

        #region Xoa

        public Task XoaTapHSCTDaCo(int idTapHSCTDaCo);

        #endregion
    }
}