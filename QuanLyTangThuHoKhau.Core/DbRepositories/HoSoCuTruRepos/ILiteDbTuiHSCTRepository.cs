using System.Collections.Generic;
using QuanLyTangThuHoKhau.Core.Models;

namespace QuanLyTangThuHoKhau.Core.DbRepositories.HoSoCuTruRepos
{
    public interface ILiteDbTuiHSCTRepository
    {
        #region Tim kiem theo dieu kien

        public IEnumerable<TuiHSCT> FindAll();

        public TuiHSCT FindOne(int id);

        public TuiHSCT FindOneTheoSoHSCT(int soHSCTCanTim);

        public TuiHSCT FindTuiHSCTMoiNhat();

        #endregion

        #region Them moi, chinh sua

        public bool Insert(TuiHSCT tuiHSCT);
        public int InsertMany(List<TuiHSCT> cacTuiHSCT);

        public bool Update(TuiHSCT tuiHSCT);

        #endregion

        #region Xoa

        public bool Delete(int id);
        public int DeleteAll();

        #endregion
    }
}