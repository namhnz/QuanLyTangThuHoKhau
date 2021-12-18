using System.Collections.ObjectModel;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types
{
    public class ThonXomKemTheoTapHSCTViewModel
    {
        public ThonXom ThonXom { get; set; }
        public ObservableCollection<TapHSCTGocInitModel> CacTapHSCTGoc { get; set; }

        public ThonXomKemTheoTapHSCTViewModel(ThonXom thonXom, ObservableCollection<TapHSCTGocInitModel> cacTapHSCTGoc)
        {
            ThonXom = thonXom;
            CacTapHSCTGoc = cacTapHSCTGoc;
        }
    }
}