using System.Collections.Generic;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.Services
{
    public interface ITapHSCTCRUDService
    {
        public Task<List<TapHSCT>> LietKeToanBoTapHSCT();
        public Task<List<TapHSCT>> LietKeToanBoTapHSCTTheoThonXom(ThonXom thonXom);
        public Task<TapHSCT> LayTapHSCTBoSungCuaThonXom(ThonXom thonXom);

        public Task ThemTapHSCTMoi(int thuTuTapHSCT, LoaiTapHSCT loaiTapHSCT, ThonXom thonXom);
        public Task ThemTapHSCTMoi(TapHSCT tapHSCTMoi);

        public Task XoaTapHSCTDaCo(int idTapHSCTDaCo);
    }
}