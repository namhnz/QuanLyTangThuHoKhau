using System;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types
{
    [Serializable]
    public class HSCT
    {
        public int SoHSCT { get; set; }
        public string ChuHo { get; set; }
        public string MaHSCTDayDu { get; private set; }
        public DateTime? NgayDangKy { get; set; }
        public bool DangThuongTru { get; set; }

        public HSCT(uint soHSCT, ThonXom thonXom, DateTime? ngayDangKy, string chuHo = "", bool dangThuongTru = true)
        {
            SoHSCT = (int)soHSCT;
            NgayDangKy = ngayDangKy;
            ChuHo = chuHo;
            MaHSCTDayDu = $"{thonXom.DonViHanhChinhPhuongXa.MaDonVi}-{soHSCT:D6}";
            NgayDangKy = ngayDangKy;
            DangThuongTru = dangThuongTru;
        }
        
    }
}