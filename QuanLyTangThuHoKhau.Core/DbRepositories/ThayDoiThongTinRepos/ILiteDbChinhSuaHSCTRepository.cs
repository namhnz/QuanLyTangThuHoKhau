using System.Collections.Generic;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.DbRepositories.ThayDoiThongTinRepos
{
    public interface ILiteDbChinhSuaHSCTRepository
    {
        #region Tim kiem

        public IEnumerable<ChinhSuaHSCT> FindAll();

        public ChinhSuaHSCT FindOne(int id);

        #endregion

        #region Them moi

        public bool Insert(ChinhSuaHSCT chinhSuaHSCT);
        // public int InsertMany(List<ChinhSuaHSCT> cacChinhSuaHSCT);

        #endregion

        #region Chinh sua

        // public bool Update(ChinhSuaHSCT chinhSuaHSCT);

        #endregion

        #region Xoa

        // public bool Delete(int id);
        public int DeleteAll();

        #endregion
    }
}