using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;
using QuanLyTangThuHoKhau.Core.AppServices.SampleDataServices;
using QuanLyTangThuHoKhau.Core.DbDataSerivces;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyThonXom.Exceptions;

namespace QuanLyTangThuHoKhau.QuanLyThonXom.Services
{
    public class ThonXomCRUDServiceSampleData: IThonXomCRUDService
    {

        public async Task<List<ThonXom>> LietKeToanBoThonXom()
        {
            var toanBoThonXom = await Task.Run(() => ThonXomSampleData.ToanBoThonXom().ToList());
            return toanBoThonXom;
        }

        public async Task ThemThonXomMoi(string tenThonXom, DonViHanhChinhChung donViHanhChinhXaPhuong)
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
                ThonXomSampleData.ThemThonXomMoi(thonXomMoi);
            });
        }

        public Task<int> ThemNhieuThonXomMoi(List<ThonXom> cacThonXomMoi)
        {
            throw new NotImplementedException();
        }

        public async Task ThemThonXomMoi(ThonXom thonXomMoi)
        {
            if (thonXomMoi == null || string.IsNullOrEmpty(thonXomMoi.TenThonXom))
            {
                throw new TenThonXomKhongDungException()
                {
                    ErrorMessage = "Tên thôn, xóm thêm mới không đúng"
                };
            }

            if (thonXomMoi.DonViHanhChinhPhuongXa == null || thonXomMoi.DonViHanhChinhPhuongXa.LoaiCapDonVi != CapDonViHanhChinh.PhuongXa)
            {
                throw new DonViHanhChinhXaPhuongKhongDungException()
                {
                    ErrorMessage = "Lựa chọn đơn vị hành chính của thôn, xóm thêm mới không phải cấp xã, phường"
                };
            }

            await Task.Run(() =>
            {
                ThonXomSampleData.ThemThonXomMoi(thonXomMoi);
            });
        }

        public async Task ThayDoiTenThonXomDaCo(int idThonXomDaCo, string tenThonXom)
        {
            // tenThonXom = tenThonXom.Trim();
            //
            // if (string.IsNullOrEmpty(tenThonXom))
            // {
            //     throw new TenThonXomKhongDungException()
            //     {
            //         ErrorMessage = "Tên thôn, xóm không đúng"
            //     };
            // }
            //
            // await Task.Run(() =>
            // {
            //     var thonXomCanDoiTen = _dataService.ThonXomRepository.FindOne(idThonXomDaCo);
            //
            //     if (thonXomCanDoiTen == null)
            //     {
            //         throw new ThonXomKhongTonTaiException()
            //         {
            //             ErrorMessage = "Thôn, xóm cần chỉnh sửa không tồn tại"
            //         };
            //     }
            //
            //     thonXomCanDoiTen.TenThonXom = tenThonXom;
            //
            //     _dataService.ThonXomRepository.Update(thonXomCanDoiTen);
            // });
        }

        public async Task XoaThonXomDaCo(int idThonXomDaCo)
        {
            // //Kiem tra co ton tai nhung tui/tap ho so lien quan den thon do khong
            //
            // await Task.Run(() =>
            // {
            //     _dataService.ThonXomRepository.Delete(idThonXomDaCo);
            // });
        }

        public async Task XoaTatCaDuLieu()
        {
            throw new NotImplementedException("Sample data không hỗ trợ các phương thức khởi tạo dữ liệu mà tự tạo dữ liệu có sẵn");
        }
    }
}