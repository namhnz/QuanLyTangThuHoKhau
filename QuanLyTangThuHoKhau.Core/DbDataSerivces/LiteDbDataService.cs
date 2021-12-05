using System;
using QuanLyTangThuHoKhau.Core.Database;
using QuanLyTangThuHoKhau.Core.DbRepositories.DiaChiRepos;
using QuanLyTangThuHoKhau.Core.DbRepositories.HoSoCuTruRepos;

namespace QuanLyTangThuHoKhau.Core.DbDataSerivces
{
    //Tao Data service: https://www.tpisoftware.com/tpu/articleDetails/1961
    public class LiteDbDataService : ILiteDbDataService, IDisposable
    {
        private LiteDbConfig _config;

        private LiteDbContext _context;
        private bool _disposed;

        private LiteDbTuiHSCTRepository _tuiHSCTRepository;
        private LiteDbTapHSCTRepository _tapHSCTRepository;
        private LiteDbThonXomRepository _thonXomRepository;

        public LiteDbDataService()
        {
            string connStr = LiteDbConfig.ConnectionString;

            if (string.IsNullOrEmpty(connStr))
                throw new ArgumentNullException("connStr", "Chua co DB Connection string");

            _config = new LiteDbConfig() { DatabasePath = connStr };
        }

        private LiteDbContext Context
        {
            get { return _context ??= new LiteDbContext(_config); }
        }

        public ILiteDbTuiHSCTRepository TuiHSCTRepository
        {
            get { return _tuiHSCTRepository ??= new LiteDbTuiHSCTRepository(Context); }
        }

        public ILiteDbTapHSCTRepository TapHSCTRepository
        {
            get { return _tapHSCTRepository ??= new LiteDbTapHSCTRepository(Context); }
        }

        public ILiteDbThonXomRepository ThonXomRepository
        {
            get { return _thonXomRepository ??= new LiteDbThonXomRepository(Context); }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) _context.Context?.Dispose();
                _disposed = true;
            }
        }

        ~LiteDbDataService()
        {
            Dispose(false);
        }
    }
}