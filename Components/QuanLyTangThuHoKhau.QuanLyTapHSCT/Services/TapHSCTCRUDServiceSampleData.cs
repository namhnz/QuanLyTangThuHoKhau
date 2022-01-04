using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.AppServices.SampleDataServices;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Exceptions;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.Services
{
    public class TapHSCTCRUDServiceSampleData : ITapHSCTCRUDService
    {
        #region Lay thong tin

        public async Task<List<TapHSCT>> LietKeToanBoTapHSCT()
        {
            var toanBoTapHSCT = await Task.Run(() => TapHSCTSampleData.ToanBoHSCT().ToList());
            return toanBoTapHSCT;
        }

        public async Task<List<TapHSCT>> LietKeToanBoTapHSCTTheoThonXom(ThonXom thonXom)
        {
            if (thonXom == null)
            {
                throw new ChuaChonThonXomChuaTapHSCTException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm để lấy các tập hồ sơ"
                };
            }

            var thonXomDaCo = ThonXomSampleData.ToanBoThonXom().FirstOrDefault(x => x.Id == thonXom.Id);
            if (thonXomDaCo == null)
            {
                throw new ChuaChonThonXomChuaTapHSCTException()
                {
                    ErrorMessage = "Thôn, xóm đã chọn không tồn tại"
                };
            }

            var toanBoTapHSCT = await LietKeToanBoTapHSCT();

            var cacTapHSCTTheoThonXom = toanBoTapHSCT.Where(x => x.ThonXom.Id == thonXom.Id)
                .OrderBy(x => x.ThuTuTapHSCT).ToList();
            return cacTapHSCTTheoThonXom;
        }

        public async Task<TapHSCT> LayTapHSCTBoSungCuaThonXom(ThonXom thonXom)
        {
            var toanBoTapHSCT = await LietKeToanBoTapHSCT();

            var tapHSCTBoSungCuaThonXom = toanBoTapHSCT
                .Where(x => x.ThonXom.Id == thonXom.Id)
                .First(x => x.LoaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTBoSung);

            return tapHSCTBoSungCuaThonXom;
        }

        #endregion

        #region Them tap ho so moi

        public async Task ThemTapHSCTMoi(int thuTuTapHSCT, LoaiTapHSCT loaiTapHSCT, ThonXom thonXom)
        {
            throw new NotImplementedException(
                "Sample data không hỗ trợ các phương thức khởi tạo dữ liệu mà tự tạo dữ liệu có sẵn");
        }

        public Task ThemTapHSCTMoi(TapHSCT tapHSCTMoi)
        {
            throw new NotImplementedException(
                "Sample data không hỗ trợ các phương thức khởi tạo dữ liệu mà tự tạo dữ liệu có sẵn");
        }

        #endregion

        //Tap ho so sau khi da tao thi khong the thay doi

        #region Xoa tap ho so

        public async Task XoaTapHSCTDaCo(int idTapHSCTDaCo)
        {
            throw new NotImplementedException(
                "Sample data không hỗ trợ các phương thức khởi tạo dữ liệu mà tự tạo dữ liệu có sẵn");
        }

        #endregion
    }
}