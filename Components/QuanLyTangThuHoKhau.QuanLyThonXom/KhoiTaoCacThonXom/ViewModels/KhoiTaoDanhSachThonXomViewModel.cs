using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;

namespace QuanLyTangThuHoKhau.QuanLyThonXom.KhoiTaoCacThonXom.ViewModels
{
    public class KhoiTaoDanhSachThonXomViewModel: BindableBase
    {
        private readonly IDonViHanhChinhService _dvhcService;


        public KhoiTaoDanhSachThonXomViewModel(IDonViHanhChinhService dvhcService)
        {
            _dvhcService = dvhcService;

            InitCommands();

            InitData();
        }

        private List<string> _toanBoXaPhuongVietNam;

        private string _tenXaPhuongCanTim;

        public string TenXaPhuongCanTim
        {
            get => _tenXaPhuongCanTim;
            set => SetProperty(ref _tenXaPhuongCanTim, value);
        }

        private string[] _cacXaPhuongDuocGoiY;

        public string[] CacXaPhuongDuocGoiY
        {
            get => _cacXaPhuongDuocGoiY;
            set => SetProperty(ref _cacXaPhuongDuocGoiY, value);
        }

        public ICommand DonViHanhChinhPhuongXaSelectorTextChangedCommand { get; private set; }

        private void DonViHanhChinhPhuongXaSelectorTextChanged(AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suggestions = TimKiemCacXaPhuongTheoDieuKien(TenXaPhuongCanTim);

                if (suggestions.Length > 0)
                    CacXaPhuongDuocGoiY = suggestions;
                else
                    CacXaPhuongDuocGoiY = new string[] { "Không tìm thấy xã, phường nào" };
            }
        }

        private string[] TimKiemCacXaPhuongTheoDieuKien(string query)
        {
            var querySplit = query.Split(' ');
            var suggestions = _toanBoXaPhuongVietNam.Where(
                item =>
                {
                    // Idea: check for every word entered (separated by space) if it is in the name,  
                    // e.g. for query "split button" the only result should "SplitButton" since its the only query to contain "split" and "button" 
                    // If any of the sub tokens is not in the string, we ignore the item. So the search gets more precise with more words 
                    bool flag = true;
                    foreach (string queryToken in querySplit)
                    {
                        // Check if token is not in string 
                        if (item.IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) < 0)
                        {
                            // Token is not in string, so we ignore this item. 
                            flag = false;
                        }
                    }
                    return flag;
                });
            return suggestions.OrderByDescending(i => i.StartsWith(query, StringComparison.CurrentCultureIgnoreCase)).ToArray();
        }

        public ICommand DonViHanhChinhPhuongXaSelectorQuerySubmittedCommand { get; private set; }

        private void DonViHanhChinhPhuongXaSelectorQuerySubmitted(AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null && args.ChosenSuggestion is string)
            {
                //User selected an item, take an action
                // SelectControl(args.ChosenSuggestion as ControlInfoDataItem);
                MessageBox.Show("12345");
            }
            else if (!string.IsNullOrEmpty(args.QueryText))
            {
                //Do a fuzzy search based on the text
                var suggestions = TimKiemCacXaPhuongTheoDieuKien(TenXaPhuongCanTim);
                if (suggestions.Length > 0)
                {
                    // SelectControl(suggestions.FirstOrDefault());
                    MessageBox.Show("12345");
                }
            }
        }

        public ICommand DonViHanhChinhPhuongXaSelectorSuggestionChosenCommand { get; private set; }

        private void DonViHanhChinhPhuongXaSelectorSuggestionChosen(AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is string tenXaPhuongDuocChon && tenXaPhuongDuocChon != "Không tìm thấy xã, phường nào")
            {
                TenXaPhuongCanTim = tenXaPhuongDuocChon;
            }
        }

        private void InitCommands()
        {
            DonViHanhChinhPhuongXaSelectorTextChangedCommand =
                new DelegateCommand<AutoSuggestBoxTextChangedEventArgs>(DonViHanhChinhPhuongXaSelectorTextChanged);

            DonViHanhChinhPhuongXaSelectorQuerySubmittedCommand =
                new DelegateCommand<AutoSuggestBoxQuerySubmittedEventArgs>(
                    DonViHanhChinhPhuongXaSelectorQuerySubmitted);

            DonViHanhChinhPhuongXaSelectorSuggestionChosenCommand =
                new DelegateCommand<AutoSuggestBoxSuggestionChosenEventArgs>(
                    DonViHanhChinhPhuongXaSelectorSuggestionChosen);
        }

        private async Task InitData()
        {
            _toanBoXaPhuongVietNam = (await _dvhcService.LoadToanBoXaPhuongVietNam()).Select(x => x.ToString()).ToList();
        }
    }
}