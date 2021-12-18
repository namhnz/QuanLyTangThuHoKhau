using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices;
using QuanLyTangThuHoKhau.Core.Models;
using QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.Types;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.ViewModels
{
    public class KhoiTaoCacTapHSCTViewModel: BindableBase
    {
        private IDonViHanhChinhService _dvhcService;

        private List<ThonXomKemTheoTapHSCTViewModel> _cacThonXomKemTheoTapHSCTViewModel;

        public List<ThonXomKemTheoTapHSCTViewModel> CacThonXomKemTheoTapHSCTViewModel
        {
            get => _cacThonXomKemTheoTapHSCTViewModel;
            set => SetProperty(ref _cacThonXomKemTheoTapHSCTViewModel, value);
        }

        public KhoiTaoCacTapHSCTViewModel(IDonViHanhChinhService dvhcService)
        {
            _dvhcService = dvhcService;

            InitSampleData();

        }

        private async void InitSampleData()
        {
            var xaPhuongHienDangQuanLy =
                (await _dvhcService.LoadToanBoXaPhuongVietNam()).First(x => x.TenDonVi.Contains("Quỳnh Hoa"));

            var cacThonXomKhoiTao = new List<ThonXom>();
            for (int i = 0; i < 6; i++)
            {
                cacThonXomKhoiTao.Add(new ThonXom()
                {
                    TenThonXom = $"Thôn {i + 1}",
                    DonViHanhChinhPhuongXa = xaPhuongHienDangQuanLy
                });
            }

            CacThonXomKemTheoTapHSCTViewModel = new List<ThonXomKemTheoTapHSCTViewModel>();
            foreach (var thonXom in cacThonXomKhoiTao)
            {
                var tapHSCTGoc1 = new TapHSCTGocInitModel() { ThonXom = thonXom, ThuTuTapHSCT = 1 };
                tapHSCTGoc1.KhoiTaoCacHSCT(1, 255);
                var tapHSCTGoc2 = new TapHSCTGocInitModel() { ThonXom = thonXom, ThuTuTapHSCT = 2 };
                tapHSCTGoc2.KhoiTaoCacHSCT(256, 320);
                var tapHSCTGoc3 = new TapHSCTGocInitModel() { ThonXom = thonXom, ThuTuTapHSCT = 3 };
                tapHSCTGoc3.KhoiTaoCacHSCT(320, 330);

                CacThonXomKemTheoTapHSCTViewModel.Add(
                    new ThonXomKemTheoTapHSCTViewModel(thonXom, new ObservableCollection<TapHSCTGocInitModel>()
                    {
                        tapHSCTGoc1,
                        tapHSCTGoc2,
                        tapHSCTGoc3
                    }));
            }
        }
    }
}