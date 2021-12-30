using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services
{
    public interface ITuiHSCTCRUDService
    {
        public Task<List<TuiHSCT>> LietKeToanBoTuiHSCT();
        public Task<List<TuiHSCT>> LietKeToanBoTuiHSCTTheoThonXom(ThonXom thonXom);
        public Task<TuiHSCT> TimKiemTuiHSCTTheoSoHSCT(int soHSCTCanTim);

        public Task<int> TaoSoHSCTMoi();
        public Task<int> TaoViTriTuiHSCTMoi(ThonXom thonXom);

        public Task ThemTuiHSCTMoi(TapHSCT tapHSCT, int viTriTui, DateTime? ngayDangKy, string chuHo = "");
        public Task ThemTuiHSCTMoi(TuiHSCT tuiHSCTMoi);

        public Task ThayDoiTenChuHoCuaTuiHSCT(int idTuiHSCT, string chuHoMoi);
        public Task XoaTuiHSCT(int idTuiHSCT);
    }
}