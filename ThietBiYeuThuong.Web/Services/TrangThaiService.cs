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
    public interface ITrangThaiService
    {
        Task<List<TrangThai>> GetAll();

        Task<TrangThai> GetById(int id);

        Task CreateAsync(TrangThai TrangThai);

        Task UpdateAsync(TrangThai TrangThai);

        TrangThai GetByIdAsNoTracking(int id);

        Task DeleteAsync(TrangThai TrangThai);
    }

    public class TrangThaiService : ITrangThaiService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrangThaiService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(TrangThai TrangThai)
        {
            _unitOfWork.trangThaiRepository.Create(TrangThai);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(TrangThai TrangThai)
        {
            _unitOfWork.trangThaiRepository.Delete(TrangThai);
            await _unitOfWork.Complete();
        }

        public async Task<List<TrangThai>> GetAll()
        {
            return await _unitOfWork.trangThaiRepository.GetAll().ToListAsync();
        }

        public async Task<TrangThai> GetById(int id)
        {
            return await _unitOfWork.trangThaiRepository.GetByLongIdAsync(id);
        }

        public TrangThai GetByIdAsNoTracking(int id)
        {
            return _unitOfWork.trangThaiRepository.GetByIdAsNoTracking(x => x.Id == id);
        }

        public async Task UpdateAsync(TrangThai TrangThai)
        {
            _unitOfWork.trangThaiRepository.Update(TrangThai);
            await _unitOfWork.Complete();
        }
    }
}