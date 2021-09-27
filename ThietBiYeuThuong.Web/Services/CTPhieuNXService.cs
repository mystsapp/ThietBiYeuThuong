using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Repositories;

namespace ThietBiYeuThuong.Web.Services
{
    public interface ICTPhieuNXService
    {
        Task<IEnumerable<CTPhieuNX>> List_CTPhieuNX_By_PhieuNXId(string phieuNXId);

        Task Create(CTPhieuNX cTPhieuNX);
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

        public async Task<IEnumerable<CTPhieuNX>> List_CTPhieuNX_By_PhieuNXId(string phieuNXId)
        {
            return await _unitOfWork.cTPhieuNXRepository.FindIncludeOneAsync(x => x.PhieuNX, x => x.PhieuNXId == phieuNXId);
        }
    }
}