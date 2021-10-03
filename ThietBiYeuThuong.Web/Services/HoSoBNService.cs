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
    public interface IHoSoBNService
    {
        Task<List<HoSoBN>> GetAll();

        Task<HoSoBN> GetById(string id);

        Task<IPagedList<HoSoBNDto>> ListHoSoBN(string searchString, string searchFromDate, string searchToDate, int? page);

        string GetSoPhieu(string param);

        Task CreateAsync(HoSoBN phieuNX);

        Task UpdateAsync(HoSoBN phieuNX);

        HoSoBN GetByIdAsNoTracking(string id);
    }

    public class HoSoBNService : IHoSoBNService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoSoBNService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(HoSoBN phieuNX)
        {
            _unitOfWork.hoSoBNRepository.Create(phieuNX);
            await _unitOfWork.Complete();
        }

        public async Task<List<HoSoBN>> GetAll()
        {
            return await _unitOfWork.hoSoBNRepository.GetAll().ToListAsync();
        }

        public async Task<HoSoBN> GetById(string id)
        {
            return await _unitOfWork.hoSoBNRepository.GetByIdAsync(id);
        }

        public HoSoBN GetByIdAsNoTracking(string id)
        {
            return _unitOfWork.hoSoBNRepository.GetByIdAsNoTracking(x => x.SoPhieu == id);
        }

        public string GetSoPhieu(string param)
        {
            var currentYear = DateTime.Now.Year; // ngay hien tai
            var subfix = param + currentYear.ToString(); // QT2021? ?QC2021? ?NT2021? ?NC2021?
            var phieuNXes = _unitOfWork.hoSoBNRepository
                                   .Find(x => x.SoPhieu.Trim()
                                   .Contains(subfix)).ToList();// chi lay nhung SoPhieu cung param: N, X + năm
            var phieuNX = new HoSoBN();
            if (phieuNXes.Count() > 0)
            {
                phieuNX = phieuNXes.OrderByDescending(x => x.SoPhieu).FirstOrDefault();
            }

            if (phieuNX == null || string.IsNullOrEmpty(phieuNX.SoPhieu))
            {
                return GetNextId.NextID("", "") + subfix; // 0001
            }
            else
            {
                var oldYear = phieuNX.SoPhieu.Substring(6, 4);

                // cung nam
                if (oldYear == currentYear.ToString())
                {
                    var oldSoCT = phieuNX.SoPhieu.Substring(0, 4);
                    return GetNextId.NextID(oldSoCT, "") + subfix;
                }
                else
                {
                    // sang nam khac' chay lai tu dau
                    return GetNextId.NextID("", "") + subfix; // 0001
                }
            }
        }

        public async Task<IPagedList<HoSoBNDto>> ListHoSoBN(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = new List<HoSoBNDto>();
            var hoSoBNs = new List<HoSoBN>();

            // search for sgtcode in kvctptC
            if (!string.IsNullOrEmpty(searchString))
            {
                var hoSoBNs1 = await _unitOfWork.hoSoBNRepository.FindIncludeOneAsync(bn => bn.BenhNhan, x => x.SoPhieu.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.BenhNhan.HoTenBN) && x.BenhNhan.HoTenBN.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.HoTenNVYTe) && x.HoTenNVYTe.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.SDT_NVYT) && x.SDT_NVYT.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.DonVi) && x.DonVi.ToLower().Contains(searchString.ToLower())));
                hoSoBNs = hoSoBNs1.ToList();
            }
            else
            {
                hoSoBNs = await GetAll();

                if (hoSoBNs == null)
                {
                    return null;
                }
            }

            foreach (var item in hoSoBNs)
            {
                var hoSoBNDto = new HoSoBNDto();

                hoSoBNDto.SoPhieu = item.SoPhieu;
                hoSoBNDto.DonVi = item.DonVi;
                hoSoBNDto.HoTenNVYTe = item.HoTenNVYTe;
                hoSoBNDto.NgayLap = item.NgayLap;
                hoSoBNDto.NgaySua = item.NgaySua;
                hoSoBNDto.NguoiSua = item.NguoiSua;
                hoSoBNDto.NVTruc = item.NVTruc;
                hoSoBNDto.SDT_NVYT = item.SDT_NVYT;
                hoSoBNDto.STT = item.STT;
                hoSoBNDto.TenBN = item.BenhNhan.HoTenBN;

                list.Add(hoSoBNDto);
            }

            list = list.OrderByDescending(x => x.NgayLap).ToList();
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

                    list = list.Where(x => x.NgayLap >= fromDate &&
                                       x.NgayLap < toDate.AddDays(1)).ToList();
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
                        list = list.Where(x => x.NgayLap >= fromDate).ToList();
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
                        list = list.Where(x => x.NgayLap < toDate.AddDays(1)).ToList();
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

        public async Task UpdateAsync(HoSoBN phieuNX)
        {
            _unitOfWork.hoSoBNRepository.Update(phieuNX);
            await _unitOfWork.Complete();
        }
    }
}