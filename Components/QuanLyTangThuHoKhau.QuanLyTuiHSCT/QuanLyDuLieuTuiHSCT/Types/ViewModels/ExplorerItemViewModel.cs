using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types.ViewModels
{
    public class ExplorerItemViewModel: BindableBase
    {
        private int _sourceId;

        public int SourceId
        {
            get => _sourceId;
            set => SetProperty(ref _sourceId, value);
        }

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

        private ObservableCollection<ExplorerItemViewModel> _children;

        public ObservableCollection<ExplorerItemViewModel> Children
        {
            get => _children;
            set => SetProperty(ref _children, value);
        }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}