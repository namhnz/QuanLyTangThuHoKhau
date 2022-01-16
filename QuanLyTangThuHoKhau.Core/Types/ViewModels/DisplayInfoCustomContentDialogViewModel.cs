using Prism.Mvvm;
using Prism.Regions;

namespace QuanLyTangThuHoKhau.Core.Types.ViewModels
{
    public class DisplayInfoCustomContentDialogViewModel: BindableBase, INavigationAware
    {
        private string _noiDungThongBao;

        public string NoiDungThongBao
        {
            get => _noiDungThongBao;
            set => SetProperty(ref _noiDungThongBao, value);
        }

        #region Dieu huong

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        #endregion
    }
}