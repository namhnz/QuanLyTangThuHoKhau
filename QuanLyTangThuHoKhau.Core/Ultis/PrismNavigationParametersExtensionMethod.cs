using Prism.Mvvm;
using Prism.Regions;

namespace QuanLyTangThuHoKhau.Core.Ultis
{
    public static class PrismNavigationParametersExtensionMethod
    {
        public static void AddNavigationSource(this NavigationParameters navigationParameters, BindableBase viewModel)
        {
            var sourceViewName = viewModel.GetViewName();
            navigationParameters.Add("NavigationSourceViewName", sourceViewName);
        }

        public static string GetNavigationSource(this NavigationParameters navigationParameters)
        {
            var sourceViewName = (string)navigationParameters["NavigationSourceViewName"];
            return sourceViewName;
        }
    }
}