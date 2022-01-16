using System;
using System.Globalization;
using System.Windows.Data;

namespace QuanLyTangThuHoKhau.MenuPart.TepTin.Ultis
{
    public class EqualsConverter : IValueConverter
    {
        public object Value { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, Value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}