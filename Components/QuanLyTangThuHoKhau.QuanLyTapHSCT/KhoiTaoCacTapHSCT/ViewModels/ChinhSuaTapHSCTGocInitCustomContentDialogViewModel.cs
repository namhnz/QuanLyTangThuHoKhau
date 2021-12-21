using Prism.Mvvm;

namespace QuanLyTangThuHoKhau.QuanLyTapHSCT.KhoiTaoCacTapHSCT.ViewModels
{
    public class ChinhSuaTapHSCTGocInitCustomContentDialogViewModel : BindableBase
    {
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
    }
}