using System.Collections.Generic;
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
        public DonViHanhChinhService()
        {
            
        }

        private List<DonViHanhChinhChung> _toanBoDonViHanhChinhCapTinhThanhCache;
        private List<DonViHanhChinhChung> _toanBoDonViHanhChinhCapXaPhuongCache;

        public async Task<List<DonViHanhChinhChung>> LoadCacDonViHanhChinhVietNam()
        {
            if (_toanBoDonViHanhChinhCapTinhThanhCache != null)
            {
                return _toanBoDonViHanhChinhCapTinhThanhCache;
            }

            var filePath = @"Assets/HanhChinhVietNam/dvhcvn.json";

            if (File.Exists(filePath))
            {

                var fileContent = await File.ReadAllTextAsync(filePath);
                JObject fileObj = JObject.Parse(fileContent);

                var jsonDataFile = fileObj.ToObject<DVHCVNJsonFileRootDataType>();

                if (jsonDataFile != null)
                {
                    var toanBoDonViHanhChinhCapTinhThanh = jsonDataFile.ExportContent();
                    _toanBoDonViHanhChinhCapTinhThanhCache = toanBoDonViHanhChinhCapTinhThanh;

                    return toanBoDonViHanhChinhCapTinhThanh;
                }
            }

            return null;
        }

        public async Task<List<DonViHanhChinhChung>> LoadToanBoXaPhuongVietNam()
        {
            if (_toanBoDonViHanhChinhCapXaPhuongCache != null)
            {
                return _toanBoDonViHanhChinhCapXaPhuongCache;
            }

            var toanBoDonViHanhChinhCapTinhThanh = await LoadCacDonViHanhChinhVietNam();
            var toanBoDonViHanhChinhCapXaPhuong = toanBoDonViHanhChinhCapTinhThanh.SelectMany(x =>
                x.CacDonViHanhChinhCapDuoi.SelectMany(y => y.CacDonViHanhChinhCapDuoi)).ToList();

            _toanBoDonViHanhChinhCapXaPhuongCache = toanBoDonViHanhChinhCapXaPhuong;

            // foreach (var xaPhuong in toanBoDonViHanhChinhCapXaPhuong)
            // {
            //     Debug.WriteLine(xaPhuong);
            // }

            return toanBoDonViHanhChinhCapXaPhuong;
        }
    }
}