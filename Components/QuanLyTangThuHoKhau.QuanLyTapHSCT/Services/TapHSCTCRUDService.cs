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

        public async Task ThemTapHSCTMoi(int thuTuTapHSCT, LoaiTapHSCT loaiTapHSCT, ThonXom thonXom)
        {
            if (thuTuTapHSCT <= 0)
            {
                throw new ThuTuTapHSCTKhongDungException()
                {
                    ErrorMessage = "Số thứ tự của tập hồ sơ thêm mới không đúng"
                };
            }

            if (thonXom == null)
            {
                throw new ChuaChonThonXomChuaTapHSCTException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm chứa tập hồ sơ"
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

            //Khong the them moi tap ho so goc co cung so thu tu
            if (loaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTGoc)
            {
                var tapHSCTGocDaTonTai = (await LietKeToanBoTapHSCT()).Any(x =>
                    x.ThuTuTapHSCT == thuTuTapHSCT && x.ThonXom.Id == thonXom.Id);

                if (tapHSCTGocDaTonTai)
                {
                    throw new ThuTuTapHSCTKhongDungException()
                    {
                        ErrorMessage = "Không thể thêm thêm tập hồ sơ gốc với cùng số thứ tự trong cùng thôn, xóm"
                    };
                }
            }


            var tapHSCTMoi = new TapHSCT()
            {
                ThuTuTapHSCT = thuTuTapHSCT,
                LoaiTapHSCT = loaiTapHSCT,
                ThonXom = thonXom
            };

            await Task.Run(() => { _dataService.TapHSCTRepository.Insert(tapHSCTMoi); });
        }

        public async Task ThemTapHSCTMoi(TapHSCT tapHSCTMoi)
        {
            if (tapHSCTMoi.ThuTuTapHSCT <= 0)
            {
                throw new ThuTuTapHSCTKhongDungException()
                {
                    ErrorMessage = "Số thứ tự của tập hồ sơ thêm mới không đúng"
                };
            }

            if (tapHSCTMoi.ThonXom == null)
            {
                throw new ChuaChonThonXomChuaTapHSCTException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm chứa tập hồ sơ"
                };
            }

            var thonXomDaCo = _dataService.ThonXomRepository.FindOne(tapHSCTMoi.ThonXom.Id);
            if (thonXomDaCo == null)
            {
                throw new ChuaChonThonXomChuaTapHSCTException()
                {
                    ErrorMessage = "Thôn, xóm đã chọn không tồn tại"
                };
            }

            //Khong the them moi tap ho so goc co cung so thu tu
            if (tapHSCTMoi.LoaiTapHSCT == LoaiTapHSCT.LoaiTapHSCTGoc)
            {
                var tapHSCTGocDaTonTai = (await LietKeToanBoTapHSCT()).Any(x =>
                    x.ThuTuTapHSCT == tapHSCTMoi.ThuTuTapHSCT && x.ThonXom.Id == tapHSCTMoi.ThonXom.Id);

                if (tapHSCTGocDaTonTai)
                {
                    throw new ThuTuTapHSCTKhongDungException()
                    {
                        ErrorMessage = "Không thể thêm thêm tập hồ sơ gốc với cùng số thứ tự trong cùng thôn, xóm"
                    };
                }
            }

            await Task.Run(() => { _dataService.TapHSCTRepository.Insert(tapHSCTMoi); });
        }

        #endregion

        //Tap ho so sau khi da tao thi khong the thay doi

        public async Task XoaTapHSCTDaCo(int idTapHSCTDaCo)
        {
            await Task.Run(() => { _dataService.ThonXomRepository.Delete(idTapHSCTDaCo); });
        }
    }
}