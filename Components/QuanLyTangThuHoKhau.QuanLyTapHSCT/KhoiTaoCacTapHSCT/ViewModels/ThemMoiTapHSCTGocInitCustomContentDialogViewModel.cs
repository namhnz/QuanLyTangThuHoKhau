using System.Collections.Generic;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.ViewModels
{
    public class ThemMoiTapHSCTGocInitCustomContentDialogViewModel : BindableBase
    {
        private int _thuTuTapHSCT;

        public int ThuTuTapHSCT
        {
            get { return _thuTuTapHSCT; }
            set { SetProperty(ref _thuTuTapHSCT, value); }
        }

        private int _soHSCTBatDau;

        public int SoHSCTBatDau
        {
            get { return _soHSCTBatDau; }
            set { SetProperty(ref _soHSCTBatDau, value); }
        }

        private int _soHSCTKetThuc;

        public int SoHSCTKetThuc
        {
            get { return _soHSCTKetThuc; }
            set { SetProperty(ref _soHSCTKetThuc, value); }
        }

        private List<ThonXom> _cacThonXomChuaCacTapHSCT;

        public List<ThonXom> CacThonXomChuaCacTapHSCT
        {
            get => _cacThonXomChuaCacTapHSCT;
            set => SetProperty(ref _cacThonXomChuaCacTapHSCT, value);
        }

        private ThonXom _selectedThonXomChuaTapHSCT;

        public ThonXom SelectedThonXomChuaTapHSCT
        {
            get => _selectedThonXomChuaTapHSCT;
            set => SetProperty(ref _selectedThonXomChuaTapHSCT, value);
        }
    }
}