using System.Collections.Generic;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.ViewModels
{
    public class ChinhSuaTuiHSCTCustomContentDialogViewModel: BindableBase
    {
        private string _soHSCT;

        public string SoHSCT
        {
            get => _soHSCT;
            set => SetProperty(ref _soHSCT, value);
        }

        private List<ThonXom> _danhSachToanBoThonXom;

        public List<ThonXom> DanhSachToanBoThonXom
        {
            get => _danhSachToanBoThonXom;
            set => SetProperty(ref _danhSachToanBoThonXom, value);
        }

        private ThonXom _selectedThonXomChuaTuiHSCT;

        public ThonXom SelectedThonXomChuaTuiHSCT
        {
            get => _selectedThonXomChuaTuiHSCT;
            set => SetProperty(ref _selectedThonXomChuaTuiHSCT, value);
        }

        private string _chuHo;

        public string ChuHo
        {
            get => _chuHo;
            set => SetProperty(ref _chuHo, value);
        }

        private bool _dangThuongTru;

        public bool DangThuongTru
        {
            get => _dangThuongTru;
            set => SetProperty(ref _dangThuongTru, value);
        }

    }
}