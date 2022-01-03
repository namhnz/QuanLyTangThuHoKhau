using System;
using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Exceptions;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types
{
    [Serializable]
    public class TapHSCTGocInitModel: TapHSCT
    {
        public int SoHSCTBatDau { get; private set; }
        public int SoHSCTKetThuc { get; private set; }

        public TapHSCTGocInitModel()
        {
            LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTGoc;
        }

        public void KhoiTaoCacGiaTriCuaTapHSCT(ThonXom thonXomChuaTapHSCT, uint thuTuTapHSCT, uint soHSCTBatDau, uint soHSCTKetThuc)
        {
            if (thonXomChuaTapHSCT == null)
            {
                throw new ChuaChonThonXomChuaTapHSCTException()
                {
                    ErrorMessage = "Chưa chọn thôn, xóm chứa tập hồ sơ"
                };
            }
            ThonXom = thonXomChuaTapHSCT;

            if (thuTuTapHSCT == 0)
            {
                throw new ThuTuTapHSCTKhongDungException()
                {
                    ErrorMessage = "Thứ tự của tập hồ sơ phải lớn hơn 0"
                };
            }
            ThuTuTapHSCT = (int)thuTuTapHSCT;

            if (soHSCTBatDau == 0)
            {
                throw new KhoangSoHSCTKhongDungException()
                {
                    ErrorMessage =
                        "Số hồ sơ bắt đầu phải lớn hơn 0"
                };
            }

            if (soHSCTBatDau > soHSCTKetThuc)
            {
                throw new KhoangSoHSCTKhongDungException()
                {
                    ErrorMessage = "Số hồ sơ bắt đầu không được lớn hơn số kết thúc trong tập hồ sơ"
                };
            }

            SoHSCTBatDau = (int)soHSCTBatDau;
            SoHSCTKetThuc = (int)soHSCTKetThuc;
        }

        public void CapNhatKhoangSoHSCT(uint soHSCTBatDau, uint soHSCTKetThuc)
        {
            if (soHSCTBatDau == 0)
            {
                throw new KhoangSoHSCTKhongDungException()
                {
                    ErrorMessage =
                        "Số hồ sơ bắt đầu phải lớn hơn 0"
                };
            }

            if (soHSCTBatDau > soHSCTKetThuc)
            {
                throw new KhoangSoHSCTKhongDungException()
                {
                    ErrorMessage = "Số hồ sơ bắt đầu không được lớn hơn số kết thúc trong tập hồ sơ"
                };
            }

            SoHSCTBatDau = (int)soHSCTBatDau;
            SoHSCTKetThuc = (int)soHSCTKetThuc;
        }
        
    }
}