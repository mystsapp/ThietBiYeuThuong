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
    public interface ILoaiThietBiService
    {
        Task<List<LoaiThietBi>> GetAll();

        Task<LoaiThietBi> GetById(int id);

        Task<IPagedList<LoaiThietBi>> ListLoaiThietBi(string searchString, string searchFromDate, string searchToDate, int? page);

        Task CreateAsync(LoaiThietBi LoaiThietBi);

        Task UpdateAsync(LoaiThietBi LoaiThietBi);

        LoaiThietBi GetByIdAsNoTracking(int id);
    }

    public class LoaiThietBiService : ILoaiThietBiService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoaiThietBiService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(LoaiThietBi LoaiThietBi)
        {
            _unitOfWork.loaiThietBiRepository.Create(LoaiThietBi);
            await _unitOfWork.Complete();
        }

        public async Task<List<LoaiThietBi>> GetAll()
        {
            return await _unitOfWork.loaiThietBiRepository.GetAll().ToListAsync();
        }

        public async Task<LoaiThietBi> GetById(int id)
        {
            return await _unitOfWork.loaiThietBiRepository.GetByIdAsync(id);
        }

        public LoaiThietBi GetByIdAsNoTracking(int id)
        {
            return _unitOfWork.loaiThietBiRepository.GetByIdAsNoTracking(x => x.Id == id);
        }

        public async Task<IPagedList<LoaiThietBi>> ListLoaiThietBi(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = new List<LoaiThietBi>();

            // search for sgtcode in kvctptC
            if (!string.IsNullOrEmpty(searchString))
            {
                list = _unitOfWork.loaiThietBiRepository.Find(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.Descripttion) && x.Descripttion.ToLower().Contains(searchString.ToLower()))).ToList();
            }
            else
            {
                list = await GetAll();

                if (list == null)
                {
                    return null;
                }
            }

            list = list.OrderByDescending(x => x.NgayTao).ToList();
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

                    list = list.Where(x => x.NgayTao >= fromDate &&
                                       x.NgayTao < toDate.AddDays(1)).ToList();
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
                        list = list.Where(x => x.NgayTao >= fromDate).ToList();
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
                        list = list.Where(x => x.NgayTao < toDate.AddDays(1)).ToList();
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

        public async Task UpdateAsync(LoaiThietBi LoaiThietBi)
        {
            _unitOfWork.loaiThietBiRepository.Update(LoaiThietBi);
            await _unitOfWork.Complete();
        }
    }
}