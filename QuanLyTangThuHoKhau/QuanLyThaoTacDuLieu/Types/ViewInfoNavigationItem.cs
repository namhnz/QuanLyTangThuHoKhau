using System;

namespace QuanLyTangThuHoKhau.QuanLyThaoTacDuLieu.Types
{
    public class ViewInfoNavigationItem
    {
        public ViewInfoNavigationItem(Type viewType, string navigationTitle)
        {
            ViewType = viewType;
            NavigationTitle = navigationTitle;
        }

        public Type ViewType { get; }

        public string NavigationTitle { get; }
        public override string ToString()
        {
            return NavigationTitle;
        }
    }
}