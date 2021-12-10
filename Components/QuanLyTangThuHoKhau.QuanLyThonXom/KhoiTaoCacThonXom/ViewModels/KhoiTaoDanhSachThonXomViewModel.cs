using System;
using System.Collections.Generic;
using System.Windows.Input;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;

namespace QuanLyTangThuHoKhau.QuanLyThonXom.KhoiTaoCacThonXom.ViewModels
{
    public class KhoiTaoDanhSachThonXomViewModel: BindableBase
    {
        public KhoiTaoDanhSachThonXomViewModel()
        {
            InitCommands();
        }

        private List<DonViHanhChinhChung> _toanBoXaPhuongVietNam;

        public ICommand DonViHanhChinhPhuongXaSelectorTextChangedCommand { get; private set; }

        private void DonViHanhChinhPhuongXaSelectorTextChanged(AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suggestions = TimKiemCacXaPhuongTheoDieuKien(sender.Text);

                if (suggestions.Count > 0)
                    sender.ItemsSource = suggestions;
                else
                    sender.ItemsSource = new string[] { "No results found" };
            }
        }

        private List<DonViHanhChinhChung> TimKiemCacXaPhuongTheoDieuKien(string query)
        {
            var querySplit = query.Split(' ');
            var suggestions = _controlPages.Where(
                item =>
                {
                    // Idea: check for every word entered (separated by space) if it is in the name,  
                    // e.g. for query "split button" the only result should "SplitButton" since its the only query to contain "split" and "button" 
                    // If any of the sub tokens is not in the string, we ignore the item. So the search gets more precise with more words 
                    bool flag = true;
                    foreach (string queryToken in querySplit)
                    {
                        // Check if token is not in string 
                        if (item.Title.IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) < 0)
                        {
                            // Token is not in string, so we ignore this item. 
                            flag = false;
                        }
                    }
                    return flag;
                });
            return suggestions.OrderByDescending(i => i.Title.StartsWith(query, StringComparison.CurrentCultureIgnoreCase)).ThenBy(i => i.Title).ToList();
        }

        private void InitCommands()
        {
            DonViHanhChinhPhuongXaSelectorTextChangedCommand =
                new DelegateCommand<AutoSuggestBoxTextChangedEventArgs>(DonViHanhChinhPhuongXaSelectorTextChanged);
        }
    }
}