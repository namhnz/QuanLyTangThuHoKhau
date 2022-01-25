using Prism.Mvvm;

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

        private string _chuHo;

        private bool _viTriTuiHSCT;

        public bool ViTriTuiHSCT
        {
            get => _viTriTuiHSCT;
            set => SetProperty(ref _viTriTuiHSCT, value);
        }

    }
}