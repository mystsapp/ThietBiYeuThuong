using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Dtos;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Repositories;
using ThietBiYeuThuong.Data.Utilities;
using ThietBiYeuThuong.Data.ViewModels;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Services
{
    public interface IThietBiService
    {
        Task<List<ThietBi>> GetAll();

        Task<ThietBi> GetById(string id);

        Task CreateAsync(ThietBi ThietBi);

        Task CreateRangeAsync(List<ThietBi> thietBis);

        Task UpdateAsync(ThietBi ThietBi);

        ThietBi GetByIdAsNoTracking(string id);

        Task DeleteAsync(ThietBi ThietBi);

        Task<IEnumerable<ThietBi>> List_ThietBi_By_LoaiThietBiId(int LoaiThietBiId);

        string GetMaTB(string param);

        Task<IEnumerable<ListViewModel>> Get_Day_VuaBomVe();

        Task<IEnumerable<ThietBi>> SearchThietBis_Code(string code);

        ThietBi GetThietBiByCode(string code);

        Task<IEnumerable<BenhNhanThietBi>> SearchThietBis_ThuHoi(string code);

        Task<IEnumerable<ThietBi>> SearchThietBis_VuaBomVe(string code);
    }

    public class ThietBiService : IThietBiService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ThietBiService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(ThietBi ThietBi)
        {
            _unitOfWork.thietBiRepository.Create(ThietBi);
            await _unitOfWork.Complete();
        }

        public async Task CreateRangeAsync(List<ThietBi> thietBis)
        {
            await _unitOfWork.thietBiRepository.CreateRange(thietBis);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(ThietBi ThietBi)
        {
            _unitOfWork.thietBiRepository.Delete(ThietBi);
            await _unitOfWork.Complete();
        }

        public async Task<List<ThietBi>> GetAll()
        {
            return await _unitOfWork.thietBiRepository.GetAll().ToListAsync();
        }

        public async Task<ThietBi> GetById(string id)
        {
            return await _unitOfWork.thietBiRepository.GetByIdAsync(id);
        }

        public ThietBi GetByIdAsNoTracking(string id)
        {
            return _unitOfWork.thietBiRepository.GetByIdAsNoTracking(x => x.MaTB == id);
        }

        public string GetMaTB(string param)
        {
            var currentYear = DateTime.Now.Year; // ngay hien tai
            var subfix = param + currentYear.ToString(); // QT2021? ?QC2021? ?NT2021? ?NC2021?
            var thietBis = _unitOfWork.thietBiRepository
                                   .Find(x => x.MaTB.Trim()
                                   .Contains(subfix)).ToList();// chi lay nhung SoPhieu cung param: N, X + năm
            var thietBi = new ThietBi();
            if (thietBis.Count() > 0)
            {
                thietBi = thietBis.OrderByDescending(x => x.MaTB).FirstOrDefault();
            }

            if (thietBi == null || string.IsNullOrEmpty(thietBi.MaTB))
            {
                return GetNextId.NextID_Phieu("", "") + subfix; // 000001PN2021
            }
            else
            {
                var oldYear = thietBi.MaTB.Substring(8, 4);

                // cung nam
                if (oldYear == currentYear.ToString())
                {
                    var oldSoCT = thietBi.MaTB.Substring(0, 6);
                    return GetNextId.NextID_Phieu(oldSoCT, "") + subfix;
                }
                else
                {
                    // sang nam khac' chay lai tu dau
                    return GetNextId.NextID_Phieu("", "") + subfix; // 000001PN2021
                }
            }
        }

        public ThietBi GetThietBiByCode(string code)
        {
            code ??= "";
            return _unitOfWork.thietBiRepository.GetById(code);
        }

        public async Task<IEnumerable<ListViewModel>> Get_Day_VuaBomVe()
        {
            var thietBis = await _unitOfWork.thietBiRepository
                .FindIncludeOneAsync(tt => tt.TrangThai, x => !x.TinhTrang && (x.TrangThaiId == 1 || x.TrangThaiId == 3));
            return thietBis.Select(x => x.TrangThai.Name).Distinct().Select(x => new ListViewModel() { Name = x });
        }

        public async Task<IEnumerable<ThietBi>> List_ThietBi_By_LoaiThietBiId(int LoaiThietBiId)
        {
            return await _unitOfWork.thietBiRepository.FindIncludeOneAsync(tb => tb.LoaiThietBi, x => x.LoaiTBId == LoaiThietBiId);
        }

        public async Task<IEnumerable<ThietBi>> SearchThietBis_Code(string code)
        {
            code ??= "";
            var thietBis = await _unitOfWork.thietBiRepository.GetAllIncludeAsync(ltb => ltb.LoaiThietBi, tt => tt.TrangThai);
            thietBis = thietBis.Where(x => x.MaTB.Trim().ToLower().Contains(code.Trim().ToLower()) ||
                                     (!string.IsNullOrEmpty(x.TenTB) && x.TenTB.Trim().ToLower().Contains(code.Trim().ToLower()))).ToList();
            thietBis = thietBis.Where(x => x.TrangThaiId == 1 || x.TrangThaiId == 3).Where(x => x.TinhTrang != false); // đầy || vừa bơm về va tinhtrang true
            return thietBis;
        }

        public async Task<IEnumerable<BenhNhanThietBi>> SearchThietBis_ThuHoi(string code)
        {
            code ??= "";
            var benhNhanThietBis = await _unitOfWork.benhNhanThietBiRepository.GetAllIncludeAsync(tb => tb.ThietBi, bn => bn.BenhNhan);
            return benhNhanThietBis.Where(x => x.ThietBiId.ToLower().Contains(code.Trim().ToLower()) ||
                                               x.ThietBi.TenTB.Trim().ToLower().Contains(code.Trim().ToLower()));
        }

        public async Task<IEnumerable<ThietBi>> SearchThietBis_VuaBomVe(string code)
        {
            code ??= "";
            var thietBis = await _unitOfWork.thietBiRepository.GetAllIncludeAsync(ltb => ltb.LoaiThietBi, tt => tt.TrangThai);
            thietBis = thietBis.Where(x => x.TrangThaiId == 4).Where(x => x.TinhTrang != false); // gửi bơm va tinhtrang true
            if (!string.IsNullOrEmpty(code))
            {
                thietBis = thietBis.Where(x => x.MaTB.Trim().ToLower().Contains(code.Trim().ToLower()) ||
                                     (!string.IsNullOrEmpty(x.TenTB) && x.TenTB.Trim().ToLower().Contains(code.Trim().ToLower()))).ToList();
            }

            return thietBis;
        }

        public async Task UpdateAsync(ThietBi ThietBi)
        {
            _unitOfWork.thietBiRepository.Update(ThietBi);
            await _unitOfWork.Complete();
        }
    }
}