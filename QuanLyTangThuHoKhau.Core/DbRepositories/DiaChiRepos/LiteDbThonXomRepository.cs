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
        private LiteDatabase _liteDb;

        public LiteDbThonXomRepository(LiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Context;
        }

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

        public bool Insert(ThonXom thonXom)
        {
            var insertedId = _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM)
                .Insert(thonXom);
            return insertedId != null;
        }

        public int InsertMany(IEnumerable<ThonXom> cacThonXom)
        {
            return _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM)
                .InsertBulk(cacThonXom);
        }

        public bool Update(ThonXom thonXom)
        {
            return _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM)
                .Update(thonXom);
        }

        public bool Delete(int id)
        {
            return _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM)
                .Delete(id);
        }

        // Xoa tat ca moi thu trong du lieu
        public void DeleteAll()
        {
            _liteDb.GetCollection<ThonXom>(DataReposNames.CAC_THON_XOM).DeleteAll();
        }
    }
}