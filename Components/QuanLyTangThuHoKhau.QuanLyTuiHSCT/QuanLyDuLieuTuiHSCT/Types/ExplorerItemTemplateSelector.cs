using System.Windows;
using System.Windows.Controls;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types.ViewModels;

namespace QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Types
{
    public class ExplorerItemTemplateSelector: DataTemplateSelector
    {
        public DataTemplate ThonXomTemplate { get; set; }
        public DataTemplate TapHSCTTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var explorerItem = (ExplorerItemViewModel)item;
            return explorerItem.Type == ExplorerItemType.ThonXom ? ThonXomTemplate : TapHSCTTemplate;
        }
    }
}