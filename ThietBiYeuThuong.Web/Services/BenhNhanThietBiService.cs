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
    public interface IBenhNhanThietBiService
    {
        Task<List<BenhNhanThietBi>> GetAll();

        Task<BenhNhanThietBi> GetById(string maBN, string maTB);

        //Task<IPagedList<BenhNhanThietBi>> ListBenhNhanThietBi(string searchString, string searchFromDate, string searchToDate, int? page);

        Task CreateAsync(BenhNhanThietBi benhNhanThietBi);

        Task UpdateAsync(BenhNhanThietBi benhNhanThietBi);

        BenhNhanThietBi GetByIdAsNoTracking(string maBN, string maTB);
    }

    public class BenhNhanThietBiService : IBenhNhanThietBiService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BenhNhanThietBiService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(BenhNhanThietBi BenhNhanThietBi)
        {
            _unitOfWork.benhNhanThietBiRepository.Create(BenhNhanThietBi);
            await _unitOfWork.Complete();
        }

        public async Task<List<BenhNhanThietBi>> GetAll()
        {
            return await _unitOfWork.benhNhanThietBiRepository.GetAll().ToListAsync();
        }

        public async Task<BenhNhanThietBi> GetById(string maBN, string maTB)
        {
            var benhNhanThietBis = await _unitOfWork.benhNhanThietBiRepository.FindAsync(x => x.BenhNhanId == maBN && x.ThietBiId == maTB);
            return benhNhanThietBis.FirstOrDefault();
        }

        public BenhNhanThietBi GetByIdAsNoTracking(string maBN, string maTB)
        {
            return _unitOfWork.benhNhanThietBiRepository.GetByIdAsNoTracking(x => x.BenhNhanId == maBN && x.ThietBiId == maTB);
        }

        //public async Task<IPagedList<BenhNhanThietBi>> ListBenhNhanThietBi(string searchString, string searchFromDate, string searchToDate, int? page)
        //{
        //    // return a 404 if user browses to before the first page
        //    if (page.HasValue && page < 1)
        //        return null;

        //    // retrieve list from database/whereverand

        //    var list = new List<BenhNhanThietBi>();

        //    // search for sgtcode in kvctptC
        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        list = _unitOfWork.benhNhanThietBiRepository.Find(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(searchString.Trim().ToLower()) ||
        //                                   (!string.IsNullOrEmpty(x.Descripttion) && x.Descripttion.ToLower().Contains(searchString.ToLower()))).ToList();
        //    }
        //    else
        //    {
        //        list = await GetAll();

        //        if (list == null)
        //        {
        //            return null;
        //        }
        //    }

        //    list = list.OrderByDescending(x => x.NgayTao).ToList();
        //    var count = list.Count();

        //    // search date
        //    DateTime fromDate, toDate;
        //    if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
        //    {
        //        try
        //        {
        //            fromDate = DateTime.Parse(searchFromDate); // NgayCT
        //            toDate = DateTime.Parse(searchToDate); // NgayCT

        //            if (fromDate > toDate)
        //            {
        //                return null; //
        //            }

        //            list = list.Where(x => x.NgayTao >= fromDate &&
        //                               x.NgayTao < toDate.AddDays(1)).ToList();
        //        }
        //        catch (Exception)
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(searchFromDate)) // NgayCT
        //        {
        //            try
        //            {
        //                fromDate = DateTime.Parse(searchFromDate);
        //                list = list.Where(x => x.NgayTao >= fromDate).ToList();
        //            }
        //            catch (Exception)
        //            {
        //                return null;
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(searchToDate)) // NgayCT
        //        {
        //            try
        //            {
        //                toDate = DateTime.Parse(searchToDate);
        //                list = list.Where(x => x.NgayTao < toDate.AddDays(1)).ToList();
        //            }
        //            catch (Exception)
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    // search date

        //    //// List<string> listRoleChiNhanh --> chi lay nhung tour thuộc phanKhuCN cua minh
        //    //if (listRoleChiNhanh.Count > 0)
        //    //{
        //    //    list = list.Where(item1 => listRoleChiNhanh.Any(item2 => item1.MaCNTao == item2)).ToList();
        //    //}
        //    //// List<string> listRoleChiNhanh --> chi lay nhung tour thuộc phanKhuCN cua minh

        //    // page the list
        //    const int pageSize = 10;
        //    decimal aa = (decimal)list.Count() / (decimal)pageSize;
        //    var bb = Math.Ceiling(aa);
        //    if (page > bb)
        //    {
        //        page--;
        //    }
        //    page = (page == 0) ? 1 : page;
        //    var listPaged = list.ToPagedList(page ?? 1, pageSize);
        //    //if (page > listPaged.PageCount)
        //    //    page--;
        //    // return a 404 if user browses to pages beyond last page. special case first page if no items exist
        //    if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
        //        return null;

        //    return listPaged;
        //}

        public async Task UpdateAsync(BenhNhanThietBi BenhNhanThietBi)
        {
            _unitOfWork.benhNhanThietBiRepository.Update(BenhNhanThietBi);
            await _unitOfWork.Complete();
        }
    }
}