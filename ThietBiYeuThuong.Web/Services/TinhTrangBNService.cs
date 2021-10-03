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
    public interface ITinhTrangBNService
    {
        Task<IEnumerable<TinhTrangBN>> GetAll();

        Task<TinhTrangBN> GetById(long id);

        Task CreateAsync(TinhTrangBN tinhTrangBN);

        Task UpdateAsync(TinhTrangBN tinhTrangBN);

        TinhTrangBN GetByIdAsNoTracking(long id);

        Task<IEnumerable<TinhTrangBN>> List_TinhTrang_By_BenhNhanId(string benhNhanId);

        Task DeleteAsync(TinhTrangBN tinhTrangBN);
    }

    public class TinhTrangBNService : ITinhTrangBNService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TinhTrangBNService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(TinhTrangBN tinhTrangBN)
        {
            _unitOfWork.tinhTrangBNRepository.Create(tinhTrangBN);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(TinhTrangBN tinhTrangBN)
        {
            _unitOfWork.tinhTrangBNRepository.Delete(tinhTrangBN);
            await _unitOfWork.Complete();
        }

        public async Task<IEnumerable<TinhTrangBN>> GetAll()
        {
            return await _unitOfWork.tinhTrangBNRepository.GetAllIncludeOneAsync(x => x.BenhNhan);
        }

        public async Task<TinhTrangBN> GetById(long id)
        {
            return await _unitOfWork.tinhTrangBNRepository.GetByLongIdAsync(id);
        }

        public TinhTrangBN GetByIdAsNoTracking(long id)
        {
            return _unitOfWork.tinhTrangBNRepository.GetByIdAsNoTracking(x => x.Id == id);
        }

        public async Task<IEnumerable<TinhTrangBN>> List_TinhTrang_By_BenhNhanId(string benhNhanId)
        {
            return await _unitOfWork.tinhTrangBNRepository.FindIncludeOneAsync(x => x.BenhNhan, x => x.BenhNhanId == benhNhanId);
        }

        public async Task UpdateAsync(TinhTrangBN TinhTrangBN)
        {
            _unitOfWork.tinhTrangBNRepository.Update(TinhTrangBN);
            await _unitOfWork.Complete();
        }
    }
}