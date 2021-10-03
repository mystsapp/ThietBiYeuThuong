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
            return _unitOfWork.cTPhieuRepository.GetByIdAsNoTracking(x => x.SoPhieu == id);
        }

        public async Task UpdateAsync(CTPhieu CTPhieu)
        {
            _unitOfWork.cTPhieuRepository.Update(CTPhieu);
            await _unitOfWork.Complete();
        }
    }
}