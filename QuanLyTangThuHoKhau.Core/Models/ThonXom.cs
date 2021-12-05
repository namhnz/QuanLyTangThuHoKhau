﻿using LiteDB;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;

namespace QuanLyTangThuHoKhau.Core.Models
{
    public class ThonXom
    {
        [BsonId] public int Id { get; set; }
        public string TenThonXom { get; set; }
        public DonViHanhChinhChung DonViHanhChinhPhuongXa { get; set; }
    }
}