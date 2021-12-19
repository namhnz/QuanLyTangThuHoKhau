using System.Collections.Generic;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.ViewModels
{
    public class ThemMoiTapHSCTGocInitCustomContentDialogViewModel : BindableBase
    {
        private int _soTapHSCT;

        public int SoTapHSCT
        {
            get { return _soTapHSCT; }
            set { SetProperty(ref _soTapHSCT, value); }
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