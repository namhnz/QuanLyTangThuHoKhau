using System.Collections.Generic;
using QuanLyTangThuHoKhau.QuanLyTuiHSCT.QuanLyDuLieuTuiHSCT.Views;

namespace QuanLyTangThuHoKhau.QuanLyThaoTacDuLieu.Types
{
    public class ViewNavigationListData: List<ViewInfoNavigationItem>
    {
        public ViewNavigationListData()
        {
            Add(new ViewInfoNavigationItem(typeof(TimKiemTuiHSCTView), "Tìm kiếm hộ thường trú"));
            Add(new ViewInfoNavigationItem(typeof(ThemMoiTuiHSCTView), "Thêm hộ thường trú mới"));
            Add(new ViewInfoNavigationItem(typeof(XemCacTuiHSCTView), "Xem toàn bộ HSCT"));
        }
    }
}