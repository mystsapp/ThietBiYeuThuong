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
    public interface IPhieuNhapService
    {
        Task<List<PhieuNhap>> GetAll();

        Task<PhieuNhap> GetById(string id);

        Task<IPagedList<PhieuNhapDto>> ListPhieuNhap(string searchString, string searchFromDate, string searchToDate, int? page);

        Task CreateAsync(PhieuNhap phieuNhap);

        Task UpdateAsync(PhieuNhap phieuNhap);

        PhieuNhap GetByIdAsNoTracking(string id);

        string GetSoPhieu(string param);
    }

    public class PhieuNhapService : IPhieuNhapService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhieuNhapService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(PhieuNhap PhieuNhap)
        {
            _unitOfWork.phieuNhapRepository.Create(PhieuNhap);
            await _unitOfWork.Complete();
        }

        public async Task<List<PhieuNhap>> GetAll()
        {
            return await _unitOfWork.phieuNhapRepository.GetAll().ToListAsync();
        }

        public async Task<PhieuNhap> GetById(string id)
        {
            return await _unitOfWork.phieuNhapRepository.GetByIdAsync(id);
        }

        public PhieuNhap GetByIdAsNoTracking(string id)
        {
            return _unitOfWork.phieuNhapRepository.GetByIdAsNoTracking(x => x.SoPhieu == id);
        }

        public string GetSoPhieu(string param)
        {
            var currentYear = DateTime.Now.Year; // ngay hien tai
            var subfix = param + currentYear.ToString(); // QT2021? ?QC2021? ?NT2021? ?NC2021?
            var phieuNhaps = _unitOfWork.phieuNhapRepository
                                   .Find(x => x.SoPhieu.Trim()
                                   .Contains(subfix)).ToList();// chi lay nhung SoPhieu cung param: N, X + năm
            var phieuNhap = new PhieuNhap();
            if (phieuNhaps.Count() > 0)
            {
                phieuNhap = phieuNhaps.OrderByDescending(x => x.SoPhieu).FirstOrDefault();
            }

            if (phieuNhap == null || string.IsNullOrEmpty(phieuNhap.SoPhieu))
            {
                return GetNextId.NextID_Phieu("", "") + subfix; // 000001PN2021
            }
            else
            {
                var oldYear = phieuNhap.SoPhieu.Substring(8, 4);

                // cung nam
                if (oldYear == currentYear.ToString())
                {
                    var oldSoCT = phieuNhap.SoPhieu.Substring(0, 6);
                    return GetNextId.NextID_Phieu(oldSoCT, "") + subfix;
                }
                else
                {
                    // sang nam khac' chay lai tu dau
                    return GetNextId.NextID_Phieu("", "") + subfix; // 000001PN2021
                }
            }
        }

        public async Task<IPagedList<PhieuNhapDto>> ListPhieuNhap(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            var phieuNhaps = new List<PhieuNhap>();
            List<PhieuNhapDto> list = new List<PhieuNhapDto>();

            // search for sgtcode in kvctptC
            if (!string.IsNullOrEmpty(searchString))
            {
                phieuNhaps = _unitOfWork.phieuNhapRepository.Find(x => !string.IsNullOrEmpty(x.SoPhieu) && x.SoPhieu.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.DonVi) && x.DonVi.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.NguoiNhap) && x.NguoiNhap.ToLower().Contains(searchString.ToLower()))).ToList();
            }
            else
            {
                phieuNhaps = await GetAll();
            }

            // search date
            DateTime fromDate, toDate;
            if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
            {
                try
                {
                    fromDate = DateTime.Parse(searchFromDate); // NgayCT
                    toDate = DateTime.Parse(searchToDate); // NgayCT

                    if (fromDate > toDate)
                    {
                        return null; //
                    }

                    phieuNhaps = phieuNhaps.Where(x => x.NgayNhap >= fromDate &&
                                       x.NgayNhap < toDate.AddDays(1)).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchFromDate)) // NgayCT
                {
                    try
                    {
                        fromDate = DateTime.Parse(searchFromDate);
                        phieuNhaps = phieuNhaps.Where(x => x.NgayNhap >= fromDate).ToList();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                if (!string.IsNullOrEmpty(searchToDate)) // NgayCT
                {
                    try
                    {
                        toDate = DateTime.Parse(searchToDate);
                        phieuNhaps = phieuNhaps.Where(x => x.NgayNhap < toDate.AddDays(1)).ToList();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            // search date

            foreach (var item in phieuNhaps)
            {
                var trangThai = await _unitOfWork.trangThaiRepository.GetByIdAsync(item.TrangThaiId);
                var phieuNhapDto = new PhieuNhapDto()
                {
                    DonVi = item.DonVi,
                    LogFile = item.LogFile,
                    NgayNhap = item.NgayNhap,
                    NgaySua = item.NgaySua,
                    NguoiNhap = item.NguoiNhap,
                    NguoiSua = item.NguoiSua,
                    SoPhieu = item.SoPhieu,
                    TrangThai = trangThai.Name
                };
                list.Add(phieuNhapDto);
            }

            list = list.OrderByDescending(x => x.NgayNhap).ToList();
            var count = list.Count();

            //// List<string> listRoleChiNhanh --> chi lay nhung tour thuộc phanKhuCN cua minh
            //if (listRoleChiNhanh.Count > 0)
            //{
            //    list = list.Where(item1 => listRoleChiNhanh.Any(item2 => item1.MaCNTao == item2)).ToList();
            //}
            //// List<string> listRoleChiNhanh --> chi lay nhung tour thuộc phanKhuCN cua minh

            // page the list
            const int pageSize = 10;
            decimal aa = (decimal)list.Count() / (decimal)pageSize;
            var bb = Math.Ceiling(aa);
            if (page > bb)
            {
                page--;
            }
            page = (page == 0) ? 1 : page;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);
            //if (page > listPaged.PageCount)
            //    page--;
            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        public async Task UpdateAsync(PhieuNhap PhieuNhap)
        {
            _unitOfWork.phieuNhapRepository.Update(PhieuNhap);
            await _unitOfWork.Complete();
        }
    }
}