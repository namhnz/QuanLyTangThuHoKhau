using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.DbDataSerivces;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyThonXom.Exceptions;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.Exceptions;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.Services
{
    public class TuiHSCTCRUDService: ITuiHSCTCRUDService
    {
        private readonly ILiteDbDataService _dataService;

        public TuiHSCTCRUDService(ILiteDbDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<TuiHSCT>> LietKeToanBoTuiHSCTTheoThonXom(ThonXom thonXom)
        {
            if (thonXom == null)
            {
                throw new ChuaChonThonXomCuaTuiHSCTException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm để lấy các túi hồ sơ"
                };
            }

            var thonXomDaCo = _dataService.ThonXomRepository.FindOne(thonXom.Id);
            if (thonXomDaCo == null)
            {
                throw new ChuaChonThonXomCuaTuiHSCTException()
                {
                    ErrorMessage = "Thôn, xóm đã chọn không tồn tại"
                };
            }

            var cacTapHSCTCuaThonXom =
                _dataService.TapHSCTRepository.FindAll().Where(x => x.ThonXom.Id == thonXom.Id).ToList();

            var toanBoThonXom = await Task.Run(() => _dataService.TuiHSCTRepository.FindAll().ToList());
            return toanBoThonXom;
        }

        public async Task ThemTuiHSCTMoi(HSCT hsct, TapHSCT tapHSCT, int viTriTui)
        {
            tenThonXom = tenThonXom.Trim();

            if (string.IsNullOrEmpty(tenThonXom))
            {
                throw new TenThonXomKhongDungException()
                {
                    ErrorMessage = "Tên thôn, xóm thêm mới không đúng"
                };
            }

            if (donViHanhChinhXaPhuong == null || donViHanhChinhXaPhuong.LoaiCapDonVi != CapDonViHanhChinh.PhuongXa)
            {
                throw new DonViHanhChinhXaPhuongKhongDungException()
                {
                    ErrorMessage = "Lựa chọn đơn vị hành chính của thôn, xóm thêm mới không phải cấp xã, phường"
                };
            }

            var thonXomMoi = new ThonXom()
            {
                TenThonXom = tenThonXom,
                DonViHanhChinhPhuongXa = donViHanhChinhXaPhuong
            };

            await Task.Run(() =>
            {
                _dataService.ThonXomRepository.Insert(thonXomMoi);
            });
        }

        private void TaoHSCTMoi

        public async Task ThayDoiTenThonXomDaCo(int idThonXomDaCo, string tenThonXom)
        {
            tenThonXom = tenThonXom.Trim();

            if (string.IsNullOrEmpty(tenThonXom))
            {
                throw new TenThonXomKhongDungException()
                {
                    ErrorMessage = "Tên thôn, xóm không đúng"
                };
            }

            await Task.Run(() =>
            {
                var thonXomCanDoiTen = _dataService.ThonXomRepository.FindOne(idThonXomDaCo);

                if (thonXomCanDoiTen == null)
                {
                    throw new ThonXomKhongTonTaiException()
                    {
                        ErrorMessage = "Thôn, xóm cần chỉnh sửa không tồn tại"
                    };
                }

                thonXomCanDoiTen.TenThonXom = tenThonXom;

                _dataService.ThonXomRepository.Update(thonXomCanDoiTen);
            });
        }

        public async Task XoaTuiHSCT(int idTuiHSCT)
        {
            await Task.Run(() =>
            {
                _dataService.TuiHSCTRepository.Delete(idTuiHSCT);
            });
        }
    }
}