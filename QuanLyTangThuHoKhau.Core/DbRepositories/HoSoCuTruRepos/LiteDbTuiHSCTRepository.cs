using System.Collections.Generic;
using System.Linq;
using LiteDB;
using QuanLyTangThuHoKhau.Core.Database;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.DbRepositories.HoSoCuTruRepos
{
    public class LiteDbTuiHSCTRepository : ILiteDbTuiHSCTRepository
    {
        private readonly LiteDatabase _liteDb;

        public LiteDbTuiHSCTRepository(LiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Context;
        }

        #region Tim kiem

        public IEnumerable<TuiHSCT> FindAll()
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .Include(x => x.TapHSCT)
                .FindAll();
        }

        public TuiHSCT FindOne(int id)
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .Include(x => x.TapHSCT)
                .Find(x => x.Id == id).FirstOrDefault();
        }

        public TuiHSCT FindOneTheoSoHSCT(int soHSCTCanTim)
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .Include(x => x.TapHSCT).Include(x => x.TapHSCT.ThonXom)
                .Find(x => x.HSCT.SoHSCT == soHSCTCanTim).FirstOrDefault();
        }

        public TuiHSCT FindTuiHSCTMoiNhat()
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .Query().OrderByDescending(x => x.HSCT.SoHSCT).FirstOrDefault();
        }

        #endregion

        #region Them moi

        public bool Insert(TuiHSCT tuiHSCT)
        {
            var insertedId = _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .Insert(tuiHSCT);

            return (int)insertedId > 0;
        }

        public int InsertMany(List<TuiHSCT> cacTuiHSCT)
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .InsertBulk(cacTuiHSCT);
        }

        #endregion

        #region Chinh sua

        public bool Update(TuiHSCT tuiHSCT)
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .Update(tuiHSCT);
        }

        #endregion

        #region Xoa

        public bool Delete(int id)
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .Delete(id);
        }

        public int DeleteAll()
        {
            return _liteDb.GetCollection<TuiHSCT>(DataReposNames.CAC_TUI_HSCT)
                .DeleteAll();
        }

        #endregion
    }
}