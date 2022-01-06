using System.Collections.Generic;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types
{
    public class ThonXomKemTheoTapHSCT
    {
        public ThonXom ThonXom { get; set; }
        public List<TapHSCTGocInitModel> CacTapHSCTGoc { get; set; }

        public ThonXomKemTheoTapHSCT(ThonXom thonXom, List<TapHSCTGocInitModel> cacTapHSCTGoc)
        {
            ThonXom = thonXom;
            CacTapHSCTGoc = cacTapHSCTGoc;
        }
    }
}