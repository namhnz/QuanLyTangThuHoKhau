using System.Reflection;
using Prism.Mvvm;

namespace QuanLyTangThuHoKhau.MenuPart.TroGiup.ViewModels
{
    public class GioiThieuCustomContentDialogViewModel : BindableBase
    {
        private string _phienBanUngDung;

        public string PhienBanUngDung
        {
            get => _phienBanUngDung;
            set => SetProperty(ref _phienBanUngDung, value);
        }

        public GioiThieuCustomContentDialogViewModel()
        {
            InitData();
        }

        #region Khoi tao

        private void InitData()
        {
            //Lay thong tin file version: https://edi.wang/post/2018/9/27/get-app-version-net-core
            var fileVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

            PhienBanUngDung = fileVersion;
        }

        #endregion
    }
}