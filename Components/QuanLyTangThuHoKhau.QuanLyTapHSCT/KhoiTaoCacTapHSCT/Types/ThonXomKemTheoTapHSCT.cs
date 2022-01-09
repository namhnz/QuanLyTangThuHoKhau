using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types
{
    public class ThonXomKemTheoTapHSCT: BindableBase
    {
        private ThonXom _thonXom;

        public ThonXom ThonXom
        {
            get => _thonXom;
            set => SetProperty(ref _thonXom, value);
        }


        public ObservableCollection<TapHSCTGocInitModel> CacTapHSCTGoc { get; set; }

        public ThonXomKemTheoTapHSCT(ThonXom thonXom, List<TapHSCTGocInitModel> cacTapHSCTGoc)
        {
            ThonXom = thonXom;
            CacTapHSCTGoc = new ObservableCollection<TapHSCTGocInitModel>(cacTapHSCTGoc);
        }
    }
}