﻿using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;

namespace QuanLyTangThuHoKhau.Core.AppServices.SampleDataServices
{
    public class XaPhuongSampleData
    {
        private readonly DonViHanhChinhChung _xaQuynhHoa;
        private readonly DonViHanhChinhChung _huyenQuynhLuu;
        private readonly DonViHanhChinhChung _tinhNgheAn;

        public XaPhuongSampleData()
        {
            var _xaQuynhHoa = new DonViHanhChinhPhuongXa()
            {
                CacDonViHanhChinhCapDuoi = null,
                MaDonVi = "17152",
                TenDonVi = "Xã Quỳnh Hoa",
                TenDonViDuCap = "Xã Quỳnh Hoa, Huyện Quỳnh Lưu, Tỉnh Nghệ An",
                LoaiCapDonVi = CapDonViHanhChinh.PhuongXa,
                TenLoaiCapDonVi = "Xã"
            };

            var _huyenQuynhLuu = new DonViHanhChinhQuanHuyen()
            {
                CacDonViHanhChinhCapDuoi = new List<DonViHanhChinhChung>()
                {
                    _xaQuynhHoa
                },
                MaDonVi = "421",
                TenDonVi = "Huyện Quỳnh Lưu",
                TenDonViDuCap = "Huyện Quỳnh Lưu, Tỉnh Nghệ An",
                LoaiCapDonVi = CapDonViHanhChinh.QuanHuyen,
                TenLoaiCapDonVi = "Huyện"
            };

            var _tinhNgheAn = new DonViHanhChinhTinhThanh()
            {
                CacDonViHanhChinhCapDuoi = new List<DonViHanhChinhChung>()
                {
                    _huyenQuynhLuu
                },
                MaDonVi = "40",
                TenDonVi = "Tỉnh Nghệ An",
                TenDonViDuCap = "Tỉnh Nghệ An",
                LoaiCapDonVi = CapDonViHanhChinh.TinhThanh,
                TenLoaiCapDonVi = "Tỉnh"
            };
        }

        private static XaPhuongSampleData _sampleData;

        private static XaPhuongSampleData SampleData
        {
            get
            {
                if (_sampleData == null)
                {
                    _sampleData = new XaPhuongSampleData();
                }

                return _sampleData;
            }
            
        }

        public static DonViHanhChinhChung XaQuynhHoa()
        {
            return SampleData._xaQuynhHoa;
        }

        public static DonViHanhChinhChung HuyenQuynhLuu()
        {
            return SampleData._huyenQuynhLuu;
        }

        public static DonViHanhChinhChung TinhNgheAn()
        {
            return SampleData._tinhNgheAn;
        }
    }
}