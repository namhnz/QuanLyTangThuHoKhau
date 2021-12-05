using System.Collections.Generic;
using System.Linq;
using LiteDB;
using QuanLyTangThuHoKhau.Core.Database;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.DbRepositories.HoSoCuTruRepos
{
    public class LiteDbTuiHSCTRepository : ILiteDbTuiHSCTRepository
    {
        private LiteDatabase _liteDb;

        public LiteDbTuiHSCTRepository(LiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Context;
        }

        #region Tim kiem theo dieu kien

        public IEnumerable<TuiHSCT> FindAll()
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .FindAll();
        }

        public TuiHSCT FindOne(int id)
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .Find(x => x.Id == id).FirstOrDefault();
        }

        #endregion

        public bool Insert(TuiHSCT tuiHSCT)
        {
            var insertedId = _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .Insert(tuiHSCT);

            return insertedId != null;
        }

        public bool Update(TuiHSCT tuiHSCT)
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .Update(tuiHSCT);
        }

        public bool Delete(int id)
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .Delete(id);
        }
    }
}