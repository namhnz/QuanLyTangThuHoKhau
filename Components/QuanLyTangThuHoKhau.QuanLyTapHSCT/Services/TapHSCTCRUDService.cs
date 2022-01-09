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

            // // if (thuTuTapHSCT <= 0)
            // // {
            // //     throw new ThuTuTapHSCTKhongDungException()
            // //     {
            // //         ErrorMessage = "Số thứ tự của tập hồ sơ thêm mới không đúng"
            // //     };
            // // }
            // //
            // // if (thonXom == null)
            // // {
            // //     throw new ChuaChonThonXomChuaTapHSCTException()
            // //     {
            // //         ErrorMessage = "Chưa chọn thôn, xóm chứa tập hồ sơ"
            // //     };
            // // }
            // //
            // // var thonXomDaCo = _dataService.ThonXomRepository.FindOne(thonXom.Id);
            // // if (thonXomDaCo == null)
            // // {
            // //     throw new ChuaChonThonXomChuaTapHSCTException()
            // //     {
            // //         ErrorMessage = "Thôn, xóm đã chọn không tồn tại"
            // //     };
            // // }
            // //
            // // //Khong the them moi tap ho so goc co cung so thu tu
            // // if (loaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTGoc)
            // // {
            // //     var tapHSCTGocDaTonTai = (await LietKeToanBoTapHSCT()).Any(x =>
            // //         x.ThuTuTapHSCT == thuTuTapHSCT && x.ThonXom.Id == thonXom.Id);
            // //
            // //     if (tapHSCTGocDaTonTai)
            // //     {
            // //         throw new ThuTuTapHSCTKhongDungException()
            // //         {
            // //             ErrorMessage = "Không thể thêm thêm tập hồ sơ gốc với cùng số thứ tự trong cùng thôn, xóm"
            // //         };
            // //     }
            // // }
            //
            //
            // var tapHSCTMoi = new TapHSCT()
            // {
            //     ThuTuTapHSCT = thuTuTapHSCT,
            //     LoaiTapHSCT = loaiTapHSCT,
            //     ThonXom = thonXom
            // };
            //
            // await Task.Run(() => { _dataService.TapHSCTRepository.Insert(tapHSCTMoi); });
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

        public async Task<int> ThemMoiNhieuTapHSCT(List<TapHSCT> cacTapHSCTMoi)
        {
            List<bool> themCacTapHSCTMoiResult = new List<bool>();

            foreach (var tapHSCTMoi in cacTapHSCTMoi)
            {
                // Kiem tra kieu du lieu
                KiemTraTapHSCTTheoKieuDuLieu(tapHSCTMoi);

                // Kiem tra trong db xem da ton tai hay chua
                await KiemTraTapHSCTTheoDb(tapHSCTMoi);

                // Them tap ho so vao Db
                var insertResult = await Task.Run(() => _dataService.TapHSCTRepository.Insert(tapHSCTMoi));
                themCacTapHSCTMoiResult.Add(insertResult);
            }

            return themCacTapHSCTMoiResult.Count(x => x == true);
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
            if (tapHSCTCanKiemTra.ThuTuTapHSCT <= 0)
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
        }

        #endregion
    }
}