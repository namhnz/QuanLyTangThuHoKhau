using System.Collections.Generic;
using System.Linq;
using LiteDB;
using QuanLyTangThuHoKhau.Core.Database;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.DbRepositories.HoSoCuTruRepos
{
    public class LiteDbTapHSCTRepository : ILiteDbTapHSCTRepository
    {
        private LiteDatabase _liteDb;

        public LiteDbTapHSCTRepository(LiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Context;
        }

        #region Tim kiem theo dieu kien

        public IEnumerable<TapHSCT> FindAll()
        {
            return _liteDb.GetCollection<TapHSCT>(DataReposNames.CAC_TAP_HSCT)
                .Include(x => x.ThonXom)
                .FindAll();
        }

        public TapHSCT FindOne(int id)
        {
            return _liteDb.GetCollection<TapHSCT>(DataReposNames.CAC_TAP_HSCT)
                .Include(x => x.ThonXom)
                .Find(x => x.Id == id).FirstOrDefault();
        }

        #endregion

        #region Them moi tap ho so

        public bool Insert(TapHSCT tapHSCT)
        {
            var insertedId = _liteDb.GetCollection<TapHSCT>(DataReposNames.CAC_TAP_HSCT)
                .Insert(tapHSCT);

            return (int)insertedId > 0;
        }

        public int InsertMany(List<TapHSCT> cacTapHSCT)
        {
            return _liteDb.GetCollection<TapHSCT>(DataReposNames.CAC_TAP_HSCT)
                .InsertBulk(cacTapHSCT);
        }

        #endregion

        #region Chinh sua

        public bool Update(TapHSCT tapHSCT)
        {
            return _liteDb.GetCollection<TapHSCT>(DataReposNames.CAC_TAP_HSCT)
                .Update(tapHSCT);
        }

        #endregion

        #region Xoa

        public bool Delete(int id)
        {
            return _liteDb.GetCollection<TapHSCT>(DataReposNames.CAC_TAP_HSCT)
                .Delete(id);
        }

        public int DeleteAll()
        {
            return _liteDb.GetCollection<TapHSCT>(DataReposNames.CAC_TAP_HSCT)
                .DeleteAll();
        }

        #endregion
    }
}