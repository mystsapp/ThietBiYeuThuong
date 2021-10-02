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
    public interface IBenhNhanService
    {
        Task<List<BenhNhan>> GetAll();

        Task<BenhNhan> GetById(string id);

        Task<IPagedList<BenhNhan>> ListBenhNhan(string searchString, string searchFromDate, string searchToDate, int? page);

        Task CreateAsync(BenhNhan BenhNhan);

        Task UpdateAsync(BenhNhan BenhNhan);

        BenhNhan GetByIdAsNoTracking(string id);

        string GetMaBN(string param);
    }

    public class BenhNhanService : IBenhNhanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BenhNhanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(BenhNhan BenhNhan)
        {
            _unitOfWork.benhNhanRepository.Create(BenhNhan);
            await _unitOfWork.Complete();
        }

        public async Task<List<BenhNhan>> GetAll()
        {
            return await _unitOfWork.benhNhanRepository.GetAll().ToListAsync();
        }

        public async Task<BenhNhan> GetById(string id)
        {
            return await _unitOfWork.benhNhanRepository.GetByIdAsync(id);
        }

        public BenhNhan GetByIdAsNoTracking(string id)
        {
            return _unitOfWork.benhNhanRepository.GetByIdAsNoTracking(x => x.MaBN == id);
        }

        public string GetMaBN(string param)
        {
            var currentYear = DateTime.Now.Year; // ngay hien tai
            var subfix = param + currentYear.ToString(); // QT2021? ?QC2021? ?NT2021? ?NC2021?
            var benhNhans = _unitOfWork.benhNhanRepository
                                   .Find(x => x.MaBN.Trim()
                                   .Contains(subfix)).ToList();// chi lay nhung SoPhieu cung param: N, X + năm
            var benhNhan = new BenhNhan();
            if (benhNhans.Count() > 0)
            {
                benhNhan = benhNhans.OrderByDescending(x => x.MaBN).FirstOrDefault();
            }

            if (benhNhan == null || string.IsNullOrEmpty(benhNhan.MaBN))
            {
                return GetNextId.NextID_BenhNhan("", "") + subfix; // 000001BN2021
            }
            else
            {
                var oldYear = benhNhan.MaBN.Substring(8, 4);

                // cung nam
                if (oldYear == currentYear.ToString())
                {
                    var oldMaBN = benhNhan.MaBN.Substring(0, 6);
                    return GetNextId.NextID(oldMaBN, "") + subfix;
                }
                else
                {
                    // sang nam khac' chay lai tu dau
                    return GetNextId.NextID("", "") + subfix; // 000001BN2021
                }
            }
        }

        public async Task<IPagedList<BenhNhan>> ListBenhNhan(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = new List<BenhNhan>();

            // search for sgtcode in kvctptC
            if (!string.IsNullOrEmpty(searchString))
            {
                list = _unitOfWork.benhNhanRepository.Find(x => !string.IsNullOrEmpty(x.MaBN) && x.MaBN.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.HoTenTN) && x.HoTenTN.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.SDT_TN) && x.SDT_TN.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.GT_TN) && x.GT_TN.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.HoTenBN) && x.HoTenBN.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.NamSinh.ToString()) && x.NamSinh.ToString().ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.CMND_CCCD_BN.ToString()) && x.CMND_CCCD_BN.ToString().ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.DiaChi) && x.DiaChi.ToLower().Contains(searchString.ToLower()))
                                           ).ToList();
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

        public async Task UpdateAsync(BenhNhan BenhNhan)
        {
            _unitOfWork.benhNhanRepository.Update(BenhNhan);
            await _unitOfWork.Complete();
        }
    }
}