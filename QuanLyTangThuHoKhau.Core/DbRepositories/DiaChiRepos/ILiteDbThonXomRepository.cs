using System.Collections.Generic;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.DbRepositories.DiaChiRepos
{
    public interface ILiteDbThonXomRepository
    {
        #region Tim kiem

        public IEnumerable<ThonXom> FindAll();

        public ThonXom FindOne(int id);

        #endregion

        #region Them thon xom moi

        public bool Insert(ThonXom thonXom);

        public int InsertMany(List<ThonXom> cacThonXom);

        #endregion

        public bool Update(ThonXom thonXom);

        public bool Delete(int id);

        public int DeleteAll();
    }
}