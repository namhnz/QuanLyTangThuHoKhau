using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.AppServices.HoSoCuTruServices.Types
{
    public class HSCT
    {
        public uint SoHSCT { get; set; }
        public string ChuHo { get; set; }
        public string MaHSCTDayDu { get; private set; }

        public HSCT(uint soHSCT, ThonXom thonXom, string chuHo = "")
        {
            SoHSCT = soHSCT;
            ChuHo = chuHo;
            MaHSCTDayDu = $"{thonXom.DonViHanhChinhPhuongXa.MaDonVi}-{soHSCT:D6}";
        }
    }
}