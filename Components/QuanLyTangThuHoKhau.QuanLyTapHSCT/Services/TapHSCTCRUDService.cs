using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.DbDataSerivces;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Exceptions;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.Services
{
    public class TapHSCTCRUDService : ITapHSCTCRUDService
    {
        private readonly ILiteDbDataService _dataService;

        public TapHSCTCRUDService(ILiteDbDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<TapHSCT>> LietKeToanBoTapHSCT()
        {
            var toanBoTapHSCT = await Task.Run(() => _dataService.TapHSCTRepository.FindAll().ToList());
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

            var thonXomDaCo = _dataService.ThonXomRepository.FindOne(thonXom.Id);
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
            if (thonXom == null)
            {
                throw new ChuaChonThonXomChuaTapHSCTException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm để lấy các tập hồ sơ"
                };
            }

            var thonXomDaCo = _dataService.ThonXomRepository.FindOne(thonXom.Id);
            if (thonXomDaCo == null)
            {
                throw new ChuaChonThonXomChuaTapHSCTException()
                {
                    ErrorMessage = "Thôn, xóm đã chọn không tồn tại"
                };
            }

            var toanBoTapHSCT = await LietKeToanBoTapHSCT();

            var tapHSCTBoSungCuaThonXom = toanBoTapHSCT
                .Where(x => x.ThonXom.Id == thonXom.Id)
                .First(x => x.LoaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTBoSung);

            return tapHSCTBoSungCuaThonXom;
        }

        #region Them moi tap ho so

        public Task ThemTapHSCTMoi(int thuTuTapHSCT, LoaiTapHSCT loaiTapHSCT, ThonXom thonXom)
        {
            var tapHSCTMoi = new TapHSCT()
            {
                LoaiTapHSCT = loaiTapHSCT,
                ThonXom = thonXom,
                ThuTuTapHSCT = thuTuTapHSCT
            };

            return ThemTapHSCTMoi(tapHSCTMoi);
        }

        public async Task ThemTapHSCTMoi(TapHSCT tapHSCTMoi)
        {
            // Kiem tra kieu du lieu
            KiemTraTapHSCTTheoKieuDuLieu(tapHSCTMoi);

            // Kiem tra trong db xem da ton tai hay chua
            await KiemTraTapHSCTTheoDb(tapHSCTMoi);

            // Them tap ho so vao Db
            await Task.Run(() => _dataService.TapHSCTRepository.Insert(tapHSCTMoi));
        }

        public async Task<int> ThemNhieuTapHSCTMoi(List<TapHSCT> cacTapHSCTMoi)
        {
            foreach (var tapHSCTMoi in cacTapHSCTMoi)
            {
                // Kiem tra kieu du lieu
                KiemTraTapHSCTTheoKieuDuLieu(tapHSCTMoi);

                // Kiem tra trong db xem da ton tai hay chua
                await KiemTraTapHSCTTheoDb(tapHSCTMoi);
            }

            // Kiem tra cac tap ho so trong danh sach co tap ho so nao giong nhau hay khong
            var cacTapHSCTCoSuTrungNhau = cacTapHSCTMoi
                .GroupBy(x => new { x.ThonXom.Id, x.LoaiTapHSCT, x.ThuTuTapHSCT })
                .Where(g => g.Count() > 1)
                .Select(y => y.Key);

            if (cacTapHSCTCoSuTrungNhau.Any())
            {
                throw new ThuTuTapHSCTKhongDungException()
                {
                    ErrorMessage = "Số thứ tự của tập hồ sơ thêm mới không được trùng nhau"
                };
            }

            // Them cac thon, xom moi vao Db
            return await Task.Run(() => _dataService.TapHSCTRepository.InsertMany(cacTapHSCTMoi));
        }

        public async Task ThemTapHSCTBoSung(ThonXom thonXom)
        {
            var cacTapHSCTGocTheoThonXom =
                (await LietKeToanBoTapHSCTTheoThonXom(thonXom)).Where(x => x.LoaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTGoc)
                .ToList();

            int thuTuTapHSCTBoSung;

            thuTuTapHSCTBoSung =
                !cacTapHSCTGocTheoThonXom.Any() ? 1 : cacTapHSCTGocTheoThonXom.Max(x => x.ThuTuTapHSCT);

            var tapHSCTMoi = new TapHSCT()
            {
                LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTBoSung,
                ThonXom = thonXom,
                ThuTuTapHSCT = thuTuTapHSCTBoSung
            };

            await Task.Run(() => ThemTapHSCTMoi(tapHSCTMoi));
        }

        #endregion

        //Tap ho so sau khi da tao thi khong the thay doi

        #region Xoa tap ho so

        public async Task XoaTapHSCTDaCo(int idTapHSCTDaCo)
        {
            await Task.Run(() => { _dataService.ThonXomRepository.Delete(idTapHSCTDaCo); });
        }

        #endregion

        #region Cac phuong thuc ho tro

        private void KiemTraTapHSCTTheoKieuDuLieu(TapHSCT tapHSCTCanKiemTra)
        {
            if (tapHSCTCanKiemTra == null || tapHSCTCanKiemTra.ThuTuTapHSCT <= 0)
            {
                throw new ThuTuTapHSCTKhongDungException()
                {
                    ErrorMessage = "Số thứ tự của tập hồ sơ thêm mới không đúng"
                };
            }

            if (tapHSCTCanKiemTra.ThonXom == null)
            {
                throw new ChuaChonThonXomChuaTapHSCTException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm chứa tập hồ sơ"
                };
            }

            var thonXomDaCo = _dataService.ThonXomRepository.FindOne(tapHSCTCanKiemTra.ThonXom.Id);
            if (thonXomDaCo == null)
            {
                throw new ChuaChonThonXomChuaTapHSCTException()
                {
                    ErrorMessage = "Thôn, xóm đã chọn không tồn tại"
                };
            }
        }

        private async Task KiemTraTapHSCTTheoDb(TapHSCT tapHSCTCanKiemTra)
        {
            //Khong the them moi tap ho so goc co cung so thu tu
            if (tapHSCTCanKiemTra.LoaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTGoc)
            {
                var tapHSCTGocDaTonTai = (await LietKeToanBoTapHSCT()).Any(x =>
                    x.ThuTuTapHSCT == tapHSCTCanKiemTra.ThuTuTapHSCT && x.ThonXom.Id == tapHSCTCanKiemTra.ThonXom.Id);

                if (tapHSCTGocDaTonTai)
                {
                    throw new ThuTuTapHSCTKhongDungException()
                    {
                        ErrorMessage = "Không thể thêm thêm tập hồ sơ gốc với cùng số thứ tự trong cùng thôn, xóm"
                    };
                }
            }

            if (tapHSCTCanKiemTra.LoaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTBoSung)
            {
                var tapHSCTBoSungDaTonTai =
                    (await LietKeToanBoTapHSCT()).Any(x => x.LoaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTBoSung);
                if (tapHSCTBoSungDaTonTai)
                {
                    throw new LoaiTapHSCTKhongDungException()
                    {
                        ErrorMessage = "Mỗi thôn, xóm chỉ có duy nhất một tập hồ sơ bổ sung"
                    };
                }
            }
        }

        #endregion
    }
}