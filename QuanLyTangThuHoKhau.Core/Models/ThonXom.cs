using System;
using LiteDB;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;

namespace QuanLyTangThuHoKhau.Core.Models
{
    [Serializable]
    public class ThonXom
    {
        [BsonId] public int Id { get; set; }
        public string TenThonXom { get; set; }
        public DonViHanhChinhChung DonViHanhChinhPhuongXa { get; set; }

        public override string ToString()
        {
            return $"{TenThonXom}, {DonViHanhChinhPhuongXa.TenDonViDuCap}";
        }
        
    }
}