using Prism.Mvvm;

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