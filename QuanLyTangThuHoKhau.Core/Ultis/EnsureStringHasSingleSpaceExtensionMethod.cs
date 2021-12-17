using System.Text.RegularExpressions;

namespace QuanLyTangThuHoKhau.Core.Ultis
{
    public static class EnsureStringHasSingleSpaceExtensionMethod
    {
        public static string EnsureStringHasSingleSpace(this string originalString)
        {
            var singleSpaceString = Regex.Replace(originalString, @"\s+", " ");
            return singleSpaceString;
        }
    }
}