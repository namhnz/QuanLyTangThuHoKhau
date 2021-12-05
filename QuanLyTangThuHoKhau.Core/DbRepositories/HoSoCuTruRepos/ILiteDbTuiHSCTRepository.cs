using System.Collections.Generic;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.DbRepositories.HoSoCuTruRepos
{
    public interface ILiteDbTuiHSCTRepository
    {
        #region Tim kiem theo dieu kien

        public IEnumerable<TuiHSCT> FindAll();

        public TuiHSCT FindOne(int id);

        #endregion

        public bool Insert(TuiHSCT tuiHSCT);

        public bool Update(TuiHSCT tuiHSCT);

        public bool Delete(int id);
    }
}