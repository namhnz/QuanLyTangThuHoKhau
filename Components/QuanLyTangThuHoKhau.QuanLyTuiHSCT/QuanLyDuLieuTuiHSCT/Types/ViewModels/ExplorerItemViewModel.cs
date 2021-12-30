using Prism.Mvvm;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types.ViewModels
{
    public class ExplorerItemViewModel: BindableBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ExplorerItemType _type;

        public ExplorerItemType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

    }
}