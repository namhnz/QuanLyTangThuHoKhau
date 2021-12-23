using System;
using System.Diagnostics;
using System.Reflection;
using Prism.Mvvm;
using QuanLyTangThuHoKhau.Core.Exceptions.SourceClassOfCallingExceptions;

namespace QuanLyTangThuHoKhau.Core.Ultis
{
    //https://stackoverflow.com/questions/2113069/c-sharp-getting-its-own-class-name
    public static class ViewModelExtensionMethod
    {
        public static string GetViewName(this BindableBase viewModel)
        {
            //Lay ten cua class goi method nay
            var viewModelClassName = viewModel.GetType().Name;

            if (!viewModelClassName.EndsWith("ViewModel"))
            {
                throw new NotAViewModelException()
                {
                    ErrorMessage = "Phương thức này cần phải được gọi từ một ViewModel"
                };
            }

            return viewModelClassName.Replace("ViewModel", "View");
        }
    }
}