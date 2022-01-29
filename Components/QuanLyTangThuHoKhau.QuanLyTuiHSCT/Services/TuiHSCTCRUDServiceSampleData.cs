using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.AppServices.SampleDataServices;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Exceptions;
using QuanLyTangThuHoKhau.QuanLyThonXom.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Exceptions;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services
{
    public class TuiHSCTCRUDServiceSampleData : ITuiHSCTCRUDService
    {
        private ITuiHSCTCRUDService _tuiHsctcrudServiceImplementation;

        public TuiHSCTCRUDServiceSampleData()
        {
        }

        #region Lay danh sach tui ho so

        public async Task<List<TuiHSCT>> LietKeToanBoTuiHSCT()
        {
            var toanBoTuiHSCT = await Task.Run(TuiHSCTSampleData.ToanBoTuiHSCT);

            return toanBoTuiHSCT;
        }

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

            var toanBoTuiHSCTCuaThonXom = await Task.Run(() =>
                TuiHSCTSampleData.ToanBoTuiHSCT().Where(x => x.TapHSCT.ThonXom.Id == thonXom.Id).ToList());
            return toanBoTuiHSCTCuaThonXom;
        }

        public async Task<List<TuiHSCT>> LietKeToanBoTuiHSCTTheoTapHSCT(TapHSCT tapHSCT)
        {
            if (tapHSCT == null)
            {
                throw new ThuTuTapHSCTKhongDungException()
                {
                    ErrorMessage = "Chưa chọn tập HSCT để lấy các túi HSCT"
                };
            }

            var tapHSCTDaCo = TapHSCTSampleData.ToanBoHSCT().FirstOrDefault(x => x.Id == tapHSCT.Id);
            if (tapHSCTDaCo == null)
            {
                throw new ThuTuTapHSCTKhongDungException()
                {
                    ErrorMessage = "Tập HSCT đã chọn không tồn tại"
                };
            }

            // var cacTapHSCTCuaThonXom =
            //     _dataService.TapHSCTRepository.FindAll().Where(x => x.ThonXom.Id == thonXom.Id).ToList();

            var toanBoTuiHSCT = await LietKeToanBoTuiHSCT();

            var toanBoTuiHSCTTheoTapHSCT =
                toanBoTuiHSCT.Where(x => x.TapHSCT.Id == tapHSCT.Id).ToList();

            return toanBoTuiHSCTTheoTapHSCT;
        }

        public async Task<TuiHSCT> TimKiemTuiHSCTTheoSoHSCT(int soHSCTCanTim)
        {
            Debug.WriteLine(TuiHSCTSampleData.ToanBoTuiHSCT().Any(x => x==null));

            var tuiHSCTCanTim = await Task.Run(() =>
                TuiHSCTSampleData.ToanBoTuiHSCT().FirstOrDefault(x => x.HSCT.SoHSCT == soHSCTCanTim));

            return tuiHSCTCanTim;
        }

        public async Task<int> TaoSoHSCTMoi()
        {
            var soHSCTLonNhat = await Task.Run(() => TuiHSCTSampleData.ToanBoTuiHSCT().Max(x => x.HSCT.SoHSCT));
            return soHSCTLonNhat + 1;
        }

        public async Task<int> TaoViTriTuiHSCTMoi(ThonXom thonXom)
        {
            if (thonXom == null)
            {
                throw new ThonXomKhongTonTaiException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm để lấy vị trí túi hồ sơ mới"
                };
            }

            var viTriTuiLonNhat = (await LietKeToanBoTuiHSCTTheoThonXom(thonXom))
                .Where(x => x.TapHSCT.LoaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTBoSung).Max(x => x.ViTriTui);

            return viTriTuiLonNhat + 1;
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

            var idTuiHSCTMoi = TuiHSCTSampleData.ToanBoTuiHSCT().Max(x => x.Id) + 1;
            tuiHSCTMoi.Id = idTuiHSCTMoi;

            await Task.Run(() => { TuiHSCTSampleData.ThemTuiHSCTMoi(tuiHSCTMoi); });
        }

        public async Task ThemTuiHSCTMoi(TuiHSCT tuiHSCTMoi)
        {
            if (tuiHSCTMoi == null)
            {
                throw new TuiHSCTKhongTonTaiException()
                {
                    ErrorMessage = "Không có túi HSCT để thêm mới"
                };
            }

            var idTuiHSCTMoi = TuiHSCTSampleData.ToanBoTuiHSCT().Max(x => x.Id) + 1;
            tuiHSCTMoi.Id = idTuiHSCTMoi;

            await Task.Run(() => { TuiHSCTSampleData.ThemTuiHSCTMoi(tuiHSCTMoi); });
        }

        public Task<int> ThemNhieuTuiHSCTMoi(List<TuiHSCT> cacTuiHSCTMoi)
        {
            throw new NotImplementedException();
        }

        public Task ThayDoiViTriCuaTuiHSCT(int idTuiHSCT, int viTriMoi)
        {
            throw new NotImplementedException();
        }

        //Dung cho phuong thuc tao moi tui ho so
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

        public Task ThayDoiThonXomCuaTuiHSCT(int idTuiHSCT, ThonXom thonXom)
        {
            throw new NotImplementedException();
        }

        public async Task CapNhatThongTinTuiHSCT(TuiHSCT tuiHSCTChinhSua)
        {
            // tuiHSCTChinhSua.HSCT.ChuHo = tuiHSCTChinhSua.HSCT.ChuHo.Trim();

            await Task.Run(() =>
            {
                var tuiHSCTTonTai = TuiHSCTSampleData.ToanBoTuiHSCT().FirstOrDefault(x => x.Id == tuiHSCTChinhSua.Id);

                if (tuiHSCTTonTai == null)
                {
                    throw new TuiHSCTKhongTonTaiException()
                    {
                        ErrorMessage = "Túi hồ sơ cần chỉnh sửa không tồn tại"
                    };
                }

                //Khong duoc de trong ten chu ho neu truoc do da co
                if (!string.IsNullOrEmpty(tuiHSCTTonTai.HSCT.ChuHo))
                {
                    if (string.IsNullOrEmpty(tuiHSCTChinhSua.HSCT.ChuHo))
                    {
                        throw new TenChuHoKhongDungException()
                        {
                            ErrorMessage = "Tên chủ hộ không đúng"
                        };
                    }
                }

                TuiHSCTSampleData.ChinhSuaTuiHSCT(tuiHSCTChinhSua);
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