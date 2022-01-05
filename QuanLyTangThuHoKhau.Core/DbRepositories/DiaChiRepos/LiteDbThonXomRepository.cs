using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using QuanLyTangThuHoKhau.Core.Database;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.DbRepositories.DiaChiRepos
{
    public class LiteDbThonXomRepository : ILiteDbThonXomRepository
    {
        private readonly LiteDatabase _liteDb;

        public LiteDbThonXomRepository(LiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Context;
        }

        #region Tim kiem

        public IEnumerable<ThonXom> FindAll()
        {
            return _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM)
                .FindAll();
        }

        public ThonXom FindOne(int id)
        {
            return _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM)
                .Find(x => x.Id == id).FirstOrDefault();
        }

        #endregion

        #region Them moi

        public bool Insert(ThonXom thonXom)
        {
            var insertedId = _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM)
                .Insert(thonXom);
            return (int)insertedId > 0;
        }

        public int InsertMany(List<ThonXom> cacThonXom)
        {
            return _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM)
                .InsertBulk(cacThonXom);
        }

        #endregion

        #region Chinh sua

        public bool Update(ThonXom thonXom)
        {
            return _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM)
                .Update(thonXom);
        }

        #endregion

        #region Xoa

        public bool Delete(int id)
        {
            return _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM)
                .Delete(id);
        }

        // Xoa tat ca moi thu trong du lieu
        public int DeleteAll()
        {
            return _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM).DeleteAll();
        }

        #endregion
    }
}