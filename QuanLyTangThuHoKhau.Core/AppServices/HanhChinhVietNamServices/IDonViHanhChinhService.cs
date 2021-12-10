using System.Collections.Generic;
using System.Threading.Tasks;
using QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices.Types;

namespace QuanLyTangThuHoKhau.Core.AppServices.HanhChinhVietNamServices
{
    //https://github.com/daohoangson/dvhcvn

    public interface IDonViHanhChinhService
    {
        public Task<List<DonViHanhChinhChung>> LoadCacDonViHanhChinhVietNam();

        public Task<List<DonViHanhChinhChung>> LoadToanBoXaPhuongVietNam();
    }
}