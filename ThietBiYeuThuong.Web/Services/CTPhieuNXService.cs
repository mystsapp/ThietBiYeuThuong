using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Repositories;
using ThietBiYeuThuong.Data.Utilities;

namespace ThietBiYeuThuong.Web.Services
{
    public interface ICTPhieuNXService
    {
        Task<IEnumerable<CTPhieuNX>> List_CTPhieuNX_By_PhieuNXId(string phieuNXId);

        Task Create(CTPhieuNX cTPhieuNX);

        string GetSoPhieuCT(string param);

        Task<List<CTPhieuNX>> GetCTTrongNgay();
    }

    public class CTPhieuNXService : ICTPhieuNXService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CTPhieuNXService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(CTPhieuNX cTPhieuNX)
        {
            _unitOfWork.cTPhieuNXRepository.Create(cTPhieuNX);
            await _unitOfWork.Complete();
        }

        public async Task<List<CTPhieuNX>> GetCTTrongNgay()
        {
            var cTPhieuNXes = _unitOfWork.cTPhieuNXRepository.GetAll();//.FindAsync(x => x.NgayNhap.Value.ToShortDateString() == DateTime.Now.ToShortDateString());
            if (cTPhieuNXes.Count() == 0) return null;
            else
            {
                return cTPhieuNXes.Where(x => x.NgayNhap.Value.ToShortDateString() == DateTime.Now.ToShortDateString()).ToList();
            }
        }

        public string GetSoPhieuCT(string param)
        {
            var currentYear = DateTime.Now.Year; // ngay hien tai
            var subfix = param + currentYear.ToString(); // QT2021? ?QC2021? ?NT2021? ?NC2021?
            var cTPhieuNXes = _unitOfWork.cTPhieuNXRepository
                                   .Find(x => x.SoPhieuCT.Trim()
                                   .Contains(subfix)).ToList();// chi lay nhung SoPhieu cung param: N, X + năm
            var cTPhieuNX = new CTPhieuNX();
            if (cTPhieuNXes.Count() > 0)
            {
                cTPhieuNX = cTPhieuNXes.OrderByDescending(x => x.SoPhieuCT).FirstOrDefault();
            }

            if (cTPhieuNX == null || string.IsNullOrEmpty(cTPhieuNX.SoPhieuCT))
            {
                return GetNextId.NextID("", "") + subfix; // 0001
            }
            else
            {
                var oldYear = cTPhieuNX.SoPhieuCT.Substring(6, 4);

                // cung nam
                if (oldYear == currentYear.ToString())
                {
                    var oldSoCT = cTPhieuNX.SoPhieuCT.Substring(0, 4);
                    return GetNextId.NextID(oldSoCT, "") + subfix;
                }
                else
                {
                    // sang nam khac' chay lai tu dau
                    return GetNextId.NextID("", "") + subfix; // 0001
                }
            }
        }

        public async Task<IEnumerable<CTPhieuNX>> List_CTPhieuNX_By_PhieuNXId(string phieuNXId)
        {
            return await _unitOfWork.cTPhieuNXRepository.FindIncludeOneAsync(x => x.PhieuNX, x => x.PhieuNXId == phieuNXId);
        }
    }
}