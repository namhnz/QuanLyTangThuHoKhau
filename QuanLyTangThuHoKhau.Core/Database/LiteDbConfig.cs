namespace QuanLyTangThuHoKhau.Core.Database
{
    //Use LiteDB 1: https://codehaks.github.io/2018/10/01/injecting-litedb-as-a-service-in-asp.net-core.html/

    public class LiteDbConfig
    {
        public const string ConnectionString = @"Filename=data\dl_qltangthuhk.db;Password=cccd@2020;Connection=shared;";

        public string DatabasePath { get; set; }
    }
}