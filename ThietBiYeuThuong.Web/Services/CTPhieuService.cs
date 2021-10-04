using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Dtos;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Repositories;
using ThietBiYeuThuong.Data.Utilities;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Services
{
    public interface ICTPhieuService
    {
        Task<List<CTPhieu>> GetAll();

        Task<CTPhieu> GetById(string id);

        Task CreateAsync(CTPhieu CTPhieu);

        Task UpdateAsync(CTPhieu CTPhieu);

        CTPhieu GetByIdAsNoTracking(string id);

        Task DeleteAsync(CTPhieu cTPhieu);

        Task<IEnumerable<CTPhieu>> List_CTPhieu_By_PhieuNhapId(string phieuNhapId);

        string GetSoPhieuCT(string param);
    }

    public class CTPhieuService : ICTPhieuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CTPhieuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(CTPhieu CTPhieu)
        {
            _unitOfWork.cTPhieuRepository.Create(CTPhieu);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(CTPhieu cTPhieu)
        {
            _unitOfWork.cTPhieuRepository.Delete(cTPhieu);
            await _unitOfWork.Complete();
        }

        public async Task<List<CTPhieu>> GetAll()
        {
            return await _unitOfWork.cTPhieuRepository.GetAll().ToListAsync();
        }

        public async Task<CTPhieu> GetById(string id)
        {
            return await _unitOfWork.cTPhieuRepository.GetByIdAsync(id);
        }

        public CTPhieu GetByIdAsNoTracking(string id)
        {
            var abc = _unitOfWork.cTPhieuRepository.GetByIdAsNoTracking(x => x.SoPhieuCT == id);
            return abc;
        }

        public string GetSoPhieuCT(string param)
        {
            var currentYear = DateTime.Now.Year; // ngay hien tai
            var subfix = param + currentYear.ToString(); // QT2021? ?QC2021? ?NT2021? ?NC2021?
            var cTPhieus = _unitOfWork.cTPhieuRepository
                                   .Find(x => x.SoPhieuCT.Trim()
                                   .Contains(subfix)).ToList();// chi lay nhung SoPhieu cung param: N, X + năm
            var cTPhieu = new CTPhieu();
            if (cTPhieus.Count() > 0)
            {
                cTPhieu = cTPhieus.OrderByDescending(x => x.SoPhieuCT).FirstOrDefault();
            }

            if (cTPhieu == null || string.IsNullOrEmpty(cTPhieu.SoPhieuCT))
            {
                return GetNextId.NextID_Phieu("", "") + subfix; // 000001PN2021
            }
            else
            {
                var oldYear = cTPhieu.SoPhieuCT.Substring(8, 4);

                // cung nam
                if (oldYear == currentYear.ToString())
                {
                    var oldSoCT = cTPhieu.SoPhieuCT.Substring(0, 6);
                    return GetNextId.NextID_Phieu(oldSoCT, "") + subfix;
                }
                else
                {
                    // sang nam khac' chay lai tu dau
                    return GetNextId.NextID_Phieu("", "") + subfix; // 000001PN2021
                }
            }
        }

        public async Task<IEnumerable<CTPhieu>> List_CTPhieu_By_PhieuNhapId(string phieuNhapId)
        {
            return await _unitOfWork.cTPhieuRepository.FindIncludeOneAsync(tb => tb.ThietBi, x => x.SoPhieu == phieuNhapId);
        }

        public async Task UpdateAsync(CTPhieu CTPhieu)
        {
            _unitOfWork.cTPhieuRepository.Update(CTPhieu);
            await _unitOfWork.Complete();
        }
    }
}