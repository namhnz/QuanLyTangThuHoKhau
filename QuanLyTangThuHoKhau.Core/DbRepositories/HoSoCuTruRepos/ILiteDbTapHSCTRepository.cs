using System.Collections.Generic;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.DbRepositories.HoSoCuTruRepos
{
    public interface ILiteDbTapHSCTRepository
    {
        #region Tim kiem

        public IEnumerable<TapHSCT> FindAll();

        public TapHSCT FindOne(int id);

        #endregion

        #region Them moi

        public bool Insert(TapHSCT tapHSCT);
        public int InsertMany(List<TapHSCT> cacTapHSCT);

        #endregion

        #region Chinh sua

        public bool Update(TapHSCT tapHSCT);

        #endregion

        #region Xoa

        public bool Delete(int id);
        public int DeleteAll();

        #endregion
    }
}