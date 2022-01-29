using System.Collections.Generic;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.ViewModels
{
    public class ChinhSuaViTriTuiHSCTCustomContentDialogViewModel: BindableBase
    {
        private string _soHSCT;

        public string SoHSCT
        {
            get => _soHSCT;
            set => SetProperty(ref _soHSCT, value);
        }

        private int _viTriCuTuiHSCT;

        public int ViTriCuTuiHSCT
        {
            get => _viTriCuTuiHSCT;
            set => SetProperty(ref _viTriCuTuiHSCT, value);
        }

        private int _viTriMoiTuiHSCT;

        public int ViTriMoiTuiHSCT
        {
            get => _viTriMoiTuiHSCT;
            set => SetProperty(ref _viTriMoiTuiHSCT, value);
        }

    }
}