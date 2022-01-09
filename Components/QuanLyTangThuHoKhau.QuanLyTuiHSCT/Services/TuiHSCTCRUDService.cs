using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.DbDataSerivces;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyThonXom.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Exceptions.TimKiemTuiHSCTExceptions;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services
{
    public class TuiHSCTCRUDService : ITuiHSCTCRUDService
    {
        private readonly ILiteDbDataService _dataService;

        public TuiHSCTCRUDService(ILiteDbDataService dataService)
        {
            _dataService = dataService;
        }

        #region Lay danh sach tui ho so

        public async Task<List<TuiHSCT>> LietKeToanBoTuiHSCT()
        {
            var toanBoTuiHSCT = await Task.Run(() => _dataService.TuiHSCTRepository.FindAll().ToList());
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

            var thonXomDaCo = _dataService.ThonXomRepository.FindOne(thonXom.Id);
            if (thonXomDaCo == null)
            {
                throw new ThonXomKhongTonTaiException()
                {
                    ErrorMessage = "Thôn, xóm đã chọn không tồn tại"
                };
            }

            // var cacTapHSCTCuaThonXom =
            //     _dataService.TapHSCTRepository.FindAll().Where(x => x.ThonXom.Id == thonXom.Id).ToList();

            var toanBoTuiHSCT = await LietKeToanBoTuiHSCT();

            var toanBoTuiHSCTCuaThonXom =
                toanBoTuiHSCT.Where(x => x.TapHSCT.ThonXom.TenThonXom == thonXom.TenThonXom).ToList();

            return toanBoTuiHSCTCuaThonXom;
        }

        public async Task<TuiHSCT> TimKiemTuiHSCTTheoSoHSCT(int soHSCTCanTim)
        {
            var tuiHSCTCanTim = await Task.Run(() => _dataService.TuiHSCTRepository.FindOneTheoSoHSCT(soHSCTCanTim));

            return tuiHSCTCanTim;
        }

        #endregion


        #region Them tui ho so moi

        public async Task<int> TaoSoHSCTMoi()
        {
            var tuiHSCTMoiNhat = await Task.Run(() => _dataService.TuiHSCTRepository.FindTuiHSCTMoiNhat());
            return tuiHSCTMoiNhat.HSCT.SoHSCT + 1;
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

        public async Task ThemTuiHSCTMoi(TapHSCT tapHSCT, int viTriTui, DateTime? ngayDangKy, string chuHo = "")
        {
            // Tao ho so moi
            var thonXomThemHSCTMoi = tapHSCT.ThonXom;
            var hsctMoi = await TaoHSCTMoi(thonXomThemHSCTMoi, ngayDangKy, chuHo);

            // Lay vi tri tui ho so moi
            var viTriTuiHSCTMoi = _dataService.TuiHSCTRepository.FindAll().Count(x => x.TapHSCT.Id == tapHSCT.Id) + 1;

            // Tao tui ho so moi
            var tuiHSCTMoi = new TuiHSCT()
            {
                HSCT = hsctMoi,
                TapHSCT = tapHSCT,
                ViTriTui = viTriTuiHSCTMoi
            };

            await Task.Run(() => { _dataService.TuiHSCTRepository.Insert(tuiHSCTMoi); });
        }

        public async Task ThemTuiHSCTMoi(TuiHSCT tuiHSCTMoi)
        {
            // Kiem tra thong tin tui ho so
            KiemTraTuiHSCTThemMoiTheoKieuDuLieu(tuiHSCTMoi);
            await KiemTraTuiHSCTThemMoiTheoDb(tuiHSCTMoi);

            // Co 2 truong hop:
            // TH1: Da lay so HSCT
            // TH2: Chua lay so HSCT
            if (tuiHSCTMoi.HSCT.SoHSCT == 0)
            {
                tuiHSCTMoi.HSCT.SoHSCT = await TaoSoHSCTMoi();
            }

            await Task.Run(() => _dataService.TuiHSCTRepository.Insert(tuiHSCTMoi));
        }

        private async Task<HSCT> TaoHSCTMoi(ThonXom thonXom, DateTime? ngayDangKy, string chuHo = "")
        {
            //Kiem tra thon xom da chon xem ton tai hay khong
            

            
            


            var hsctMoi = await Task.Run(() =>
            {
                //Lay so ho so moi nhat
                var tuiHSCTCuoiCung = _dataService.TuiHSCTRepository.FindTuiHSCTMoiNhat();
                int soHSCTMoiNhat;

                soHSCTMoiNhat = tuiHSCTCuoiCung == null ? 1 : tuiHSCTCuoiCung.HSCT.SoHSCT + 1;

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
                var tuiHSCTDoiTenChuHo = _dataService.TuiHSCTRepository.FindOne(idTuiHSCT);

                if (tuiHSCTDoiTenChuHo == null)
                {
                    throw new TuiHSCTKhongTonTaiException()
                    {
                        ErrorMessage = "Túi hồ sơ cần chỉnh sửa không tồn tại"
                    };
                }

                tuiHSCTDoiTenChuHo.HSCT.ChuHo = chuHoMoi;

                _dataService.TuiHSCTRepository.Update(tuiHSCTDoiTenChuHo);
            });
        }

        public async Task CapNhatThongTinTuiHSCT(TuiHSCT tuiHSCTChinhSua)
        {
            // tuiHSCTChinhSua.HSCT.ChuHo = tuiHSCTChinhSua.HSCT.ChuHo.Trim();


            await Task.Run(() =>
            {
                var tuiHSCTTonTai = _dataService.TuiHSCTRepository.FindOne(tuiHSCTChinhSua.Id);

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

                _dataService.TuiHSCTRepository.Update(tuiHSCTChinhSua);
            });
        }

        #endregion

        #region Xoa tui ho so cu tru

        public async Task XoaTuiHSCT(int idTuiHSCT)
        {
            await Task.Run(() => { _dataService.TuiHSCTRepository.Delete(idTuiHSCT); });
        }

        #endregion

        #region Cac phuong thuc ho tro

        private void KiemTraTuiHSCTThemMoiTheoKieuDuLieu(TuiHSCT tuiHSCTCanKiemTra)
        {
            if (tuiHSCTCanKiemTra == null)
            {
                throw new TuiHSCTKhongTonTaiException()
                {
                    ErrorMessage = "Không có túi HSCT để thêm mới"
                };
            }

            if (tuiHSCTCanKiemTra.TapHSCT == null)
            {
                throw new TapHSCTChuaTuiHSCTMoiKhongDungException()
                {
                    ErrorMessage = "Chưa chọn tập hồ sơ để thêm túi hồ sơ mới"
                };
            }

            if (tuiHSCTCanKiemTra.TapHSCT.ThonXom == null)
            {
                throw new ThonXomKhongTonTaiException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm để thêm mới hồ sơ"
                };
            }
        }

        private async Task KiemTraTuiHSCTThemMoiTheoDb(TuiHSCT tuiHSCTCanKiemTra)
        {
            var thonXomDaCo = await Task.Run(() => _dataService.ThonXomRepository.FindOne(tuiHSCTCanKiemTra.TapHSCT.ThonXom.Id));
            if (thonXomDaCo == null)
            {
                throw new ThonXomKhongTonTaiException()
                {
                    ErrorMessage = "Thôn, xóm đã chọn không tồn tại"
                };
            }

            //Kiem tra tap ho so ton tai
            var tapHSCTDaCo = await Task.Run(() => _dataService.TapHSCTRepository.FindOne(tuiHSCTCanKiemTra.TapHSCT.Id));
            if (tapHSCTDaCo == null)
            {
                throw new TapHSCTChuaTuiHSCTMoiKhongDungException()
                {
                    ErrorMessage = "Tập hồ sơ đã chọn không tồn tại"
                };
            }

            // Kiem tra tui ho so da ton tai
            if (tuiHSCTCanKiemTra.HSCT.SoHSCT > 0)
            {
                var tuiHSCTDaCo = await Task.Run(() => _dataService.TuiHSCTRepository.FindOneTheoSoHSCT(tuiHSCTCanKiemTra.HSCT.SoHSCT));
                if (tuiHSCTDaCo != null)
                {
                    throw new SoHSCTKhongDungException()
                    {
                        ErrorMessage = "Số HSCT đã tồn tại trong Db"
                    };
                }
            }
        }

        #endregion
    }
}