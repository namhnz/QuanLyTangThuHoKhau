using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModernWpf.Controls;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;
using QuanLyTangThuHoKhau.QuanLyThonXom.KhoiTaoCacThonXom.ViewModels;

namespace QuanLyTangThuHoKhau.QuanLyThonXom.KhoiTaoCacThonXom.Views
{
    /// <summary>
    /// Interaction logic for KhoiTaoDanhSachThonXomView.xaml
    /// </summary>
    public partial class KhoiTaoDanhSachThonXomView : UserControl
    {
        public KhoiTaoDanhSachThonXomView()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// This event gets fired when:
        ///     * a user presses Enter while focus is in the TextBox
        ///     * a user clicks or tabs to and invokes the query button (defined using the QueryIcon API)
        ///     * a user presses selects (clicks/taps/presses Enter) a suggestion
        /// </summary>
        /// <param name="sender">The AutoSuggestBox that fired the event.</param>
        /// <param name="args">The args contain the QueryText, which is the text in the TextBox,
        /// and also ChosenSuggestion, which is only non-null when a user selects an item in the list.</param>
        private void ChonXaPhuongQuanLyAutoSuggestionBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var viewModel = this.DataContext as KhoiTaoDanhSachThonXomViewModel;
            
            if (args.ChosenSuggestion != null && args.ChosenSuggestion is DonViHanhChinhChung)
            {
                var donViXaPhuongDuocChon = args.ChosenSuggestion as DonViHanhChinhChung;
                //User selected an item, take an action
                viewModel.XacDinhDonViXaPhuongDaChon(donViXaPhuongDuocChon);
                sender.Text = donViXaPhuongDuocChon.ToString();
            }
            else if (!string.IsNullOrEmpty(args.QueryText))
            {
                //Do a fuzzy search based on the text
                var suggestions = viewModel.TimKiemCacXaPhuongTheoDieuKien(sender.Text);
                if (suggestions.Count > 0)
                {
                    var donViXaPhuongDuocChon = suggestions.FirstOrDefault();
                    viewModel.XacDinhDonViXaPhuongDaChon(donViXaPhuongDuocChon);
                    sender.Text = donViXaPhuongDuocChon.ToString();
                }
            }
        }

        /// <summary>
        /// This event gets fired as the user keys through the list, or taps on a suggestion.
        /// This allows you to change the text in the TextBox to reflect the item in the list.
        /// Alternatively you can use TextMemberPath.
        /// </summary>
        /// <param name="sender">The AutoSuggestBox that fired the event.</param>
        /// <param name="args">The args contain SelectedItem, which contains the data item of the item that is currently highlighted.</param>
        private void ChonXaPhuongQuanLyAutoSuggestionBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //Don't autocomplete the TextBox when we are showing "no results"
            if (args.SelectedItem is DonViHanhChinhChung control)
            {
                sender.Text = control.TenDonViDuCap;
            }
        }

        private void ChonXaPhuongQuanLyAutoSuggestionBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var viewModel = this.DataContext as KhoiTaoDanhSachThonXomViewModel;

            //We only want to get results when it was a user typing,
            //otherwise we assume the value got filled in by TextMemberPath
            //or the handler for SuggestionChosen
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suggestions = viewModel.TimKiemCacXaPhuongTheoDieuKien(sender.Text);

                if (suggestions.Count > 0)
                    sender.ItemsSource = suggestions;
                else
                    sender.ItemsSource = new string[] { "Không tìm thấy xã, phường nào" };
            }
        }
    }
}
