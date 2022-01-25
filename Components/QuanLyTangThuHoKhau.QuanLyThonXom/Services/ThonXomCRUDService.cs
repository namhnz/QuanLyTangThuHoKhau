using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;
using QuanLyTangThuHoKhau.Core.DbDataSerivces;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.Core.Ultis;
using QuanLyTangThuHoKhau.QuanLyThonXom.Exceptions;

namespace QuanLyTangThuHoKhau.QuanLyThonXom.Services
{
    public class ThonXomCRUDService : IThonXomCRUDService
    {
        #region Cac phu thuoc

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ILiteDbDataService _dataService;

        #endregion

        public ThonXomCRUDService(ILiteDbDataService dataService)
        {
            _dataService = dataService;
        }

        #region Tim kiem

        public async Task<List<ThonXom>> LietKeToanBoThonXom()
        {
            var toanBoThonXom = await Task.Run(() => _dataService.ThonXomRepository.FindAll().ToList());
            return toanBoThonXom;
        }

        #endregion

        #region Them moi

        public async Task ThemThonXomMoi(string tenThonXom, DonViHanhChinhChung donViHanhChinhXaPhuong)
        {
            tenThonXom = tenThonXom.Trim();

            var thonXomMoi = new ThonXom()
            {
                TenThonXom = tenThonXom,
                DonViHanhChinhPhuongXa = donViHanhChinhXaPhuong
            };

            // Them thon xom moi
            await ThemThonXomMoi(thonXomMoi);

            await Task.Run(() => { _dataService.ThonXomRepository.Insert(thonXomMoi); });
        }

        public async Task ThemThonXomMoi(ThonXom thonXomMoi)
        {
            // Kiem tra xem cac thong tin trong thon, xom da dung hay chua
            KiemTraThonXomThemMoiTheoKieuDuLieu(thonXomMoi);

            // Kiem tra thon xom da ton tai trong db chua
            await KiemTraThonXomThemMoiTheoDb(thonXomMoi);

            await Task.Run(() => { _dataService.ThonXomRepository.Insert(thonXomMoi); });
        }

        // Phuong thuc chi dung trong khoi tao du lieu
        public async Task<int> ThemNhieuThonXomMoi(List<ThonXom> cacThonXomMoi)
        {
            // Kiem tra cac thon xom trong danh sach co thon xom nao giong nhau hay khong
            var cacTenThonXomCoSuTrungNhau = cacThonXomMoi.GroupBy(x => x.TenThonXom)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key);
            if (cacTenThonXomCoSuTrungNhau.Any())
            {
                throw new TenThonXomKhongDungException()
                {
                    ErrorMessage = "Tên các thôn, xóm thêm mới không được trùng nhau"
                };
            }

            //Kiem tra thong tin cac thon, xom
            //Do so luong cac thon, com it nen van se kiem tra trong Db da ton tai hay chua
            foreach (var thonXomMoi in cacThonXomMoi)
            {
                // Kiem tra xem cac thong tin trong thon, xom da dung hay chua
                KiemTraThonXomThemMoiTheoKieuDuLieu(thonXomMoi);

                // Khong can kiem tra thon xom da ton tai trong db chua do truoc khi khoi tao da clear du lieu
                // await KiemTraThonXomThemMoiTheoDb(thonXomMoi);
            }

            // Them cac thon, xom moi vao Db
            return await Task.Run(() => _dataService.ThonXomRepository.InsertMany(cacThonXomMoi));
        }

        #endregion

        #region Chinh sua

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

        #endregion

        #region Xoa

        public async Task XoaThonXomDaCo(int idThonXomDaCo)
        {
            //Kiem tra co ton tai nhung tui/tap ho so lien quan den thon do khong

            await Task.Run(() => { _dataService.ThonXomRepository.Delete(idThonXomDaCo); });
        }

        public async Task XoaTatCaDuLieu()
        {
            await Task.Run(() =>
            {
                _dataService.ChinhSuaHSCTRepository.DeleteAll();
                _dataService.TuiHSCTRepository.DeleteAll();
                _dataService.TapHSCTRepository.DeleteAll();
                _dataService.ThonXomRepository.DeleteAll();
            });
        }

        #endregion

        #region Cac phuong thuc ho tro

        private void KiemTraThonXomThemMoiTheoKieuDuLieu(ThonXom thonXomCanKiemTra)
        {
            if (thonXomCanKiemTra == null || string.IsNullOrEmpty(thonXomCanKiemTra.TenThonXom.Trim()))
            {
                throw new TenThonXomKhongDungException()
                {
                    ErrorMessage = "Tên thôn, xóm thêm mới không đúng"
                };
            }

            if (thonXomCanKiemTra.DonViHanhChinhPhuongXa == null ||
                thonXomCanKiemTra.DonViHanhChinhPhuongXa.LoaiCapDonVi != CapDonViHanhChinh.PhuongXa)
            {
                throw new DonViHanhChinhXaPhuongKhongDungException()
                {
                    ErrorMessage =
                        "Lựa chọn đơn vị hành chính trực thuộc của thôn, xóm thêm mới không phải cấp xã, phường"
                };
            }
        }

        private async Task KiemTraThonXomThemMoiTheoDb(ThonXom thonXomCanKiemTra)
        {
            // TODO: Kiem tra don vi xa, phuong cua thon, xom them moi co trung voi cac thon, xom khac hay khong

            var thonXomDaCoTrongDb =
                (await Task.Run(() => _dataService.ThonXomRepository.FindAll())).Any(x =>
                    x.TenThonXom == thonXomCanKiemTra.TenThonXom);

            if (thonXomDaCoTrongDb)
            {
                throw new ThonXomKhongTonTaiException()
                {
                    ErrorMessage = "Thôn, xóm cần cần thêm mới đã có trong Db"
                };
            }
        }

        #endregion
    }
}