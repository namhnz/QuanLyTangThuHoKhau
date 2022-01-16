using Prism.Mvvm;
using Prism.Regions;

namespace QuanLyTangThuHoKhau.Core.Types.ViewModels
{
    public class YesNoConfirmCustomContentDialogViewModel: BindableBase, INavigationAware
    {
        private string _noiDungXacNhan;

        public string NoiDungXacNhan
        {
            get => _noiDungXacNhan;
            set => SetProperty(ref _noiDungXacNhan, value);
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