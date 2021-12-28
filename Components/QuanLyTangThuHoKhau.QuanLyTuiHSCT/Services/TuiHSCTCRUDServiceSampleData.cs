using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.AppServices.SampleDataServices;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyThonXom.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Exceptions;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services
{
    public class TuiHSCTCRUDServiceSampleData: ITuiHSCTCRUDService
    {

        #region Lay danh sach tui ho so

        public async Task<List<TuiHSCT>> LietKeToanBoTuiHSCTTheoThonXom(ThonXom thonXom)
        {
            if (thonXom == null)
            {
                throw new ThonXomKhongTonTaiException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm để lấy các túi hồ sơ"
                };
            }

            var thonXomDaCo = ThonXomSampleData.ToanBoThonXom().FirstOrDefault(x => x.Id == thonXom.Id);
            if (thonXomDaCo == null)
            {
                throw new ThonXomKhongTonTaiException()
                {
                    ErrorMessage = "Thôn, xóm đã chọn không tồn tại"
                };
            }
            
            var toanBoTuiHSCTCuaThonXom = await Task.Run(() => TuiHSCTSampleData.ToanBoTuiHSCT().Where(x => x.TapHSCT.ThonXom.Id == thonXom.Id).ToList());
            return toanBoTuiHSCTCuaThonXom;
        }

        #endregion


        #region Them tui ho so moi
        public async Task ThemTuiHSCTMoi(TapHSCT tapHSCT, int viTriTui, DateTime? ngayDangKy, string chuHo = "")
        {
            if (tapHSCT == null)
            {
                throw new TapHSCTChuaTuiHSCTMoiKhongDungException()
                {
                    ErrorMessage = "Chưa chọn tập hồ sơ để thêm túi hồ sơ mới"
                };
            }

            //Kiem tra tap ho so ton tai
            var tapHSCTDaCo = TapHSCTSampleData.ToanBoHSCT().FirstOrDefault(x => x.Id == tapHSCT.Id);
            if (tapHSCTDaCo == null)
            {
                throw new TapHSCTChuaTuiHSCTMoiKhongDungException()
                {
                    ErrorMessage = "Tập hồ sơ đã chọn không tồn tại"
                };
            }

            // Tao ho so moi
            var thonXomThemHSCTMoi = tapHSCT.ThonXom;
            var hsctMoi = await TaoHSCTMoi(thonXomThemHSCTMoi, DateTime.Now, chuHo);

            // Lay vi tri tui ho so moi
            var viTriTuiHSCTMoi = TuiHSCTSampleData.ToanBoTuiHSCT().Count(x => x.TapHSCT.Id == tapHSCT.Id) + 1;

            // Tao tui ho so moi
            var tuiHSCTMoi = new TuiHSCT()
            {
                HSCT = hsctMoi,
                TapHSCT = tapHSCT,
                ViTriTui = viTriTuiHSCTMoi
            };

            await Task.Run(() => { TuiHSCTSampleData.ThemTuiHSCTMoi(tuiHSCTMoi); });
        }

        private async Task<HSCT> TaoHSCTMoi(ThonXom thonXom, DateTime? ngayDangKy, string chuHo = "")
        {
            //Kiem tra thon xom da chon xem ton tai hay khong
            if (thonXom == null)
            {
                throw new ThonXomKhongTonTaiException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm để thêm mới hồ sơ"
                };
            }

            var thonXomDaCo = ThonXomSampleData.ToanBoThonXom().FirstOrDefault(x => x.Id == thonXom.Id);
            if (thonXomDaCo == null)
            {
                throw new ThonXomKhongTonTaiException()
                {
                    ErrorMessage = "Thôn, xóm đã chọn không tồn tại"
                };
            }

            var hsctMoi = await Task.Run(() =>
            {
                //Lay so ho so moi nhat
                var soHSCTCuoiCung = TuiHSCTSampleData.ToanBoTuiHSCT().Max(x => x.HSCT.SoHSCT);
                int soHSCTMoiNhat = soHSCTCuoiCung + 1;

                //Tao ho so moi
                return new HSCT((uint)soHSCTMoiNhat, thonXom, ngayDangKy, chuHo);
            });

            return hsctMoi;
        }

        #endregion

        #region Chinh sua tui ho so, thong tin trong ho so

        public async Task ThayDoiTenChuHoCuaTuiHSCT(int idTuiHSCT, string chuHoMoi)
        {
            chuHoMoi = chuHoMoi.Trim();

            if (string.IsNullOrEmpty(chuHoMoi))
            {
                throw new TenChuHoKhongDungException()
                {
                    ErrorMessage = "Tên chủ hộ không đúng"
                };
            }

            await Task.Run(() =>
            {
                var tuiHSCTDoiTenChuHo = TuiHSCTSampleData.ToanBoTuiHSCT().FirstOrDefault(x => x.Id == idTuiHSCT);

                if (tuiHSCTDoiTenChuHo == null)
                {
                    throw new TuiHSCTKhongTonTaiException()
                    {
                        ErrorMessage = "Túi hồ sơ cần chỉnh sửa không tồn tại"
                    };
                }

                tuiHSCTDoiTenChuHo.HSCT.ChuHo = chuHoMoi;

                TuiHSCTSampleData.ChinhSuaTuiHSCT(tuiHSCTDoiTenChuHo);
            });
        }

        #endregion

        #region Xoa tui ho so cu tru

        public async Task XoaTuiHSCT(int idTuiHSCT)
        {
            await Task.Run(() => { TuiHSCTSampleData.XoaTuiHSCT(idTuiHSCT); });
        }

        #endregion
    }
}