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
    public interface IPhieuXuatService
    {
        Task<List<PhieuXuat>> GetAll();

        Task<PhieuXuat> GetById(string id);

        Task<IPagedList<PhieuXuat>> ListPhieuXuat(string searchString, string searchFromDate, string searchToDate, int? page);

        Task CreateAsync(PhieuXuat PhieuXuat);

        Task UpdateAsync(PhieuXuat PhieuXuat);

        PhieuXuat GetByIdAsNoTracking(string id);
    }

    public class PhieuXuatService : IPhieuXuatService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhieuXuatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(PhieuXuat PhieuXuat)
        {
            _unitOfWork.phieuXuatRepository.Create(PhieuXuat);
            await _unitOfWork.Complete();
        }

        public async Task<List<PhieuXuat>> GetAll()
        {
            return await _unitOfWork.phieuXuatRepository.GetAll().ToListAsync();
        }

        public async Task<PhieuXuat> GetById(string id)
        {
            return await _unitOfWork.phieuXuatRepository.GetByIdAsync(id);
        }

        public PhieuXuat GetByIdAsNoTracking(string id)
        {
            return _unitOfWork.phieuXuatRepository.GetByIdAsNoTracking(x => x.SoPhieu == id);
        }

        public async Task<IPagedList<PhieuXuat>> ListPhieuXuat(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = new List<PhieuXuat>();

            // search for sgtcode in kvctptC
            if (!string.IsNullOrEmpty(searchString))
            {
                var list1 = await _unitOfWork.phieuXuatRepository.FindIncludeOneAsync(bn => bn.BenhNhan, x => !string.IsNullOrEmpty(x.SoPhieu) && x.SoPhieu.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.BenhNhan.HoTenBN) && x.BenhNhan.HoTenBN.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.BenhNhan.HoTenTN) && x.BenhNhan.HoTenTN.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.NguoiXuat) && x.NguoiXuat.ToLower().Contains(searchString.ToLower())));
                list = list1.ToList();
            }
            else
            {
                list = await GetAll();

                if (list == null)
                {
                    return null;
                }
            }

            list = list.OrderByDescending(x => x.NgayXuat).ToList();
            var count = list.Count();

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

                    list = list.Where(x => x.NgayXuat >= fromDate &&
                                       x.NgayXuat < toDate.AddDays(1)).ToList();
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
                        list = list.Where(x => x.NgayXuat >= fromDate).ToList();
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
                        list = list.Where(x => x.NgayXuat < toDate.AddDays(1)).ToList();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            // search date

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

        public async Task UpdateAsync(PhieuXuat PhieuXuat)
        {
            _unitOfWork.phieuXuatRepository.Update(PhieuXuat);
            await _unitOfWork.Complete();
        }
    }
}