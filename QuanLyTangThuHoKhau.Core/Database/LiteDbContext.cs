using System;
using LiteDB;

namespace QuanLyTangThuHoKhau.Core.Database
{
    public class LiteDbContext
    {
        public readonly LiteDatabase Context;

        public LiteDbContext(LiteDbConfig config)
        {
            try
            {
                var db = new LiteDatabase(config.DatabasePath);
                Context = db;
            }
            catch (Exception ex)
            {
                throw new Exception("Can find or create LiteDb database", ex);
            }
        }
    }
}