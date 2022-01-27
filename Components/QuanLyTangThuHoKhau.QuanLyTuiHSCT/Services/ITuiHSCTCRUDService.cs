using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services
{
    public interface ITuiHSCTCRUDService
    {
        #region Lay danh sach tui ho so

        public Task<List<TuiHSCT>> LietKeToanBoTuiHSCT();
        public Task<List<TuiHSCT>> LietKeToanBoTuiHSCTTheoThonXom(ThonXom thonXom);
        public Task<List<TuiHSCT>> LietKeToanBoTuiHSCTTheoTapHSCT(TapHSCT tapHSCT);

        #endregion

        public Task<TuiHSCT> TimKiemTuiHSCTTheoSoHSCT(int soHSCTCanTim);

        public Task<int> TaoSoHSCTMoi();
        public Task<int> TaoViTriTuiHSCTMoi(ThonXom thonXom);

        public Task ThemTuiHSCTMoi(TapHSCT tapHSCT, int viTriTui, DateTime? ngayDangKy, string chuHo = "");
        public Task ThemTuiHSCTMoi(TuiHSCT tuiHSCTMoi);
        public Task<int> ThemNhieuTuiHSCTMoi(List<TuiHSCT> cacTuiHSCTMoi);

        public Task ThayDoiTenChuHoCuaTuiHSCT(int idTuiHSCT, string chuHoMoi);
        public Task ThayDoiThonXomCuaTuiHSCT(int idTuiHSCT, ThonXom thonXom);
        public Task CapNhatThongTinTuiHSCT(TuiHSCT tuiHSCTChinhSua);

        public Task XoaTuiHSCT(int idTuiHSCT);
    }
}