using System;

namespace QuanLyTangThuHoKhau.Core.Exceptions
{
    public class BaseException: Exception
    {
        public string ErrorMessage { get; set; }
    }
}