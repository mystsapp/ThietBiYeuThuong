using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Dtos;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Repositories;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Services
{
    public interface IPhieuNXService
    {
        Task<List<PhieuNX>> GetAll();

        Task<PhieuNX> GetById(string id);

        Task<IPagedList<PhieuNXDto>> ListPhieuNX(string searchString, string searchFromDate, string searchToDate, int? page);
    }

    public class PhieuNXService : IPhieuNXService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhieuNXService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PhieuNX>> GetAll()
        {
            return await _unitOfWork.phieuNXRepository.GetAll().ToListAsync();
        }

        public async Task<PhieuNX> GetById(string id)
        {
            return await _unitOfWork.phieuNXRepository.GetByIdAsync(id);
        }

        public async Task<IPagedList<PhieuNXDto>> ListPhieuNX(string searchString, string searchFromDate, string searchToDate, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = new List<PhieuNXDto>();
            var phieuNXs = new List<PhieuNX>();

            // search for sgtcode in kvctptC
            if (!string.IsNullOrEmpty(searchString))
            {
                phieuNXs = _unitOfWork.phieuNXRepository.Find(x => x.SoPhieu.ToLower().Contains(searchString.Trim().ToLower()) ||
                                           (!string.IsNullOrEmpty(x.HoTenTN) && x.HoTenTN.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.SDT_TN) && x.SDT_TN.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.HoTenBN) && x.HoTenBN.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.CMND_CCCD_BN.ToString()) && x.CMND_CCCD_BN.ToString().Contains(searchString)) ||
                                           (!string.IsNullOrEmpty(x.NamSinh.ToString()) && x.NamSinh.ToString().Contains(searchString)) ||
                                           (!string.IsNullOrEmpty(x.DiaChi) && x.DiaChi.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.HoTenNVYTe) && x.HoTenNVYTe.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.SDT_NVYT) && x.SDT_NVYT.ToLower().Contains(searchString.ToLower())) ||
                                           (!string.IsNullOrEmpty(x.DonVi) && x.DonVi.ToLower().Contains(searchString.ToLower()))).ToList();
            }
            else
            {
                phieuNXs = await GetAll();

                if (phieuNXs == null)
                {
                    return null;
                }
            }

            foreach (var item in phieuNXs)
            {
                var phieuNXDto = new PhieuNXDto();

                phieuNXDto.SoPhieu = item.SoPhieu;
                phieuNXDto.BenhNenBN = item.BenhNenBN;
                phieuNXDto.ChiSoSPO2 = item.ChiSoSPO2;
                phieuNXDto.CMND_CCCD_BN = item.CMND_CCCD_BN;
                phieuNXDto.DiaChi = item.DiaChi;
                phieuNXDto.DonVi = item.DonVi;
                phieuNXDto.GT_TN = item.GT_TN;
                phieuNXDto.HoTenBN = item.HoTenBN;
                phieuNXDto.HoTenNVYTe = item.HoTenNVYTe;
                phieuNXDto.HoTenTN = item.HoTenTN;
                phieuNXDto.KetLuan = item.KetLuan;
                phieuNXDto.LapPhieu = item.LapPhieu;
                phieuNXDto.LoaiPhieu = item.LoaiPhieu;
                phieuNXDto.NamSinh = item.NamSinh;

                phieuNXDto.NgayLap = item.NgayLap;
                phieuNXDto.NVTruc = item.NVTruc;
                phieuNXDto.SDT_NVYT = item.SDT_NVYT;
                phieuNXDto.SDT_TN = item.SDT_TN;
                phieuNXDto.STT = item.STT;
                phieuNXDto.TinhTrangBN = item.TinhTrangBN;
                phieuNXDto.TinhTrangBNSauO2 = item.TinhTrangBNSauO2;

                list.Add(phieuNXDto);
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
    }
}