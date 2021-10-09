﻿using System;
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
    public interface INVYTService
    {
        Task<List<NhanVienYTe>> GetAll();

        Task<NhanVienYTe> GetById(string id);

        Task<IPagedList<NhanVienYTe>> ListNhanVienYTe(string searchString, string searchFromDate, string searchToDate, int? page);

        Task CreateAsync(NhanVienYTe nhanVienYTe);

        Task UpdateAsync(NhanVienYTe nhanVienYTe);

        NhanVienYTe GetByIdAsNoTracking(string id);

        string GetMaNVYT(string param);

        IEnumerable<NhanVienYTe> SearchNVYTs_Code(string code);

        NhanVienYTe GetNVYTByCode(string code);
    }

    public class NVYTService : INVYTService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NVYTService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(NhanVienYTe NhanVienYTe)
        {
            _unitOfWork.nVYTRepository.Create(NhanVienYTe);
            await _unitOfWork.Complete();
        }

        public async Task<List<NhanVienYTe>> GetAll()
        {
            return await _unitOfWork.nVYTRepository.GetAll().ToListAsync();
        }

        public NhanVienYTe GetNVYTByCode(string code)
        {
            code ??= "";
            return _unitOfWork.nVYTRepository.GetById(code);
        }

        public IEnumerable<NhanVienYTe> SearchNVYTs_Code(string code)
        {
            code ??= "";
            var NhanVienYTes = _unitOfWork.nVYTRepository.Find(x => x.MaNVYT.Trim().ToLower().Contains(code.Trim().ToLower()) ||
                                                                     (!string.IsNullOrEmpty(x.HoTenNVYTe) && x.HoTenNVYTe.Trim().ToLower().Contains(code.Trim().ToLower())) ||
                                                                     (!string.IsNullOrEmpty(x.SDT_NVYT) && x.SDT_NVYT.Trim().ToLower().Contains(code.Trim().ToLower())) ||
                                                                     (!string.IsNullOrEmpty(x.DonVi.ToString()) && x.DonVi.ToString().Trim().ToLower().Contains(code.Trim().ToLower()))).ToList();
            return NhanVienYTes;
        }

        public async Task<NhanVienYTe> GetById(string id)
        {
            return await _unitOfWork.nVYTRepository.GetByIdAsync(id);
        }

        public NhanVienYTe GetByIdAsNoTracking(string id)
        {
            return _unitOfWork.nVYTRepository.GetByIdAsNoTracking(x => x.MaNVYT == id);
        }

        public string GetMaNVYT(string param)
        {
            var currentYear = DateTime.Now.Year; // ngay hien tai
            var subfix = param + currentYear.ToString(); // QT2021? ?QC2021? ?NT2021? ?NC2021?
            var NhanVienYTes = _unitOfWork.nVYTRepository
                                   .Find(x => x.MaNVYT.Trim()
                                   .Contains(subfix)).ToList();// chi lay nhung SoPhieu cung param: N, X + năm
            var NhanVienYTe = new NhanVienYTe();
            if (NhanVienYTes.Count() > 0)
            {
                NhanVienYTe = NhanVienYTes.OrderByDescending(x => x.MaNVYT).FirstOrDefault();
            }

            if (NhanVienYTe == null || string.IsNullOrEmpty(NhanVienYTe.MaNVYT))
            {
                return GetNextId.NextID_BenhNhan("", "") + subfix; // 000001NV2021
            }
            else
            {
                var oldYear = NhanVienYTe.MaNVYT.Substring(8, 4);

                // cung nam
                if (oldYear == currentYear.ToString())
                {
                    var oldMaBN = NhanVienYTe.MaNVYT.Substring(0, 6);
                    return GetNextId.NextID(oldMaBN, "") + subfix;
                }
                else
                {
                    // sang nam khac' chay lai tu dau
                    return GetNextId.NextID("", "") + subfix; // 000001NV2021
                }
            }
        }

        public async Task<IPagedList<NhanVienYTe>> ListNhanVienYTe(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = new List<NhanVienYTe>();

            // search for sgtcode in kvctptC
            if (!string.IsNullOrEmpty(searchString))
            {
                list = _unitOfWork.nVYTRepository.Find(x => !string.IsNullOrEmpty(x.MaNVYT) && x.MaNVYT.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.HoTenNVYTe) && x.HoTenNVYTe.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.SDT_NVYT) && x.SDT_NVYT.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.DonVi) && x.DonVi.ToLower().Contains(searchString.ToLower()))).ToList();
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

        public async Task UpdateAsync(NhanVienYTe NhanVienYTe)
        {
            _unitOfWork.nVYTRepository.Update(NhanVienYTe);
            await _unitOfWork.Complete();
        }
    }
}