﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;

namespace QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices
{
    public class DonViHanhChinhService: IDonViHanhChinhService
    {
        public async Task<List<DonViHanhChinhChung>> LoadCacDonViHanhChinhVietNam()
        {
            var filePath = @"Assets/HanhChinhVietNam/dvhcvn.json";

            if (File.Exists(filePath))
            {

                var fileContent = await File.ReadAllTextAsync(filePath);
                JObject fileObj = JObject.Parse(fileContent);

                var jsonDataFile = fileObj.ToObject<DVHCVNJsonFileRootDataType>();

                if (jsonDataFile != null)
                {
                    var toanBoDonViHanhChinhCapTinhThanh = jsonDataFile.ExportContent();

                    return toanBoDonViHanhChinhCapTinhThanh;
                }
            }

            return null;
        }
    }
}