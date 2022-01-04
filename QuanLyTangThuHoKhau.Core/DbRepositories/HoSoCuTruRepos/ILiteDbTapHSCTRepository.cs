using System.Collections.Generic;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.DbRepositories.HoSoCuTruRepos
{
    public interface ILiteDbTapHSCTRepository
    {
        #region Tim kiem theo dieu kien

        public IEnumerable<TapHSCT> FindAll();

        public TapHSCT FindOne(int id);

        #endregion

        public bool Insert(TapHSCT tapHSCT);

        public bool Update(TapHSCT tapHSCT);

        public bool Delete(int id);
        public void DeleteAll();
    }
}