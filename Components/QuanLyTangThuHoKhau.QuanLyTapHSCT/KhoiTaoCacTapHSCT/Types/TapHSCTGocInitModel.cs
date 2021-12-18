﻿using QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.Exceptions;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types
{
    public class TapHSCTGocInitModel: TapHSCT
    {
        public int SoHSCTBatDau { get; private set; }
        public int SoHSCTKetThuc { get; private set; }

        public TapHSCTGocInitModel()
        {
            LoaiTapHSCT = LoaiTapHSCT.LoaiTapHSCTGoc;
        }

        public void KhoiTaoCacHSCT(uint soHSCTBatDau, uint soHSCTKetThuc)
        {
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