using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ThietBiYeuThuong.Web.Services
{
    public interface ITinhTonService
    {
        TinhTon GetLast(string searchFromDate, string searchToDate);

        List<TinhTon> Find_Equal_By_Date(DateTime dateTime);

        Task<List<CTPhieuNX>> ListCTPhieuNX(string searchFromDate, string searchToDate);

        Task CreateAsync(TinhTon tinhTon);

        Task<string> CheckTonDauStatus(DateTime fromDate);

        Task<TinhTon> GetById(long id);

        Task UpdateAsync(TinhTon tinhTon1);
    }

    public class TinhTonService : ITinhTonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TinhTonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(TinhTon tinhTon)
        {
            _unitOfWork.tinhTonRepository.Create(tinhTon);
            await _unitOfWork.Complete();
        }

        public List<TinhTon> Find_Equal_By_Date(DateTime dateTime)
        {
            var tinhTons = _unitOfWork.tinhTonRepository.Find(x => x.NgayCT.Value.ToShortDateString() == dateTime.ToShortDateString()).ToList();
            if (tinhTons.Count == 0)
            {
                return tinhTons;
            }
            return tinhTons;
        }

        public TinhTon GetLast(string searchFromDate, string searchToDate)
        {
            List<TinhTon> list = new List<TinhTon>();
            // search date
            DateTime fromDate, toDate;
            if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
            {
                try
                {
                    fromDate = DateTime.Parse(searchFromDate); // NgayCT
                    toDate = DateTime.Parse(searchToDate); // NgayCT

                    list = _unitOfWork.tinhTonRepository.Find(x => x.NgayCT >= fromDate &&
                                       x.NgayCT < toDate.AddDays(1)).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                //if (!string.IsNullOrEmpty(searchFromDate)) // tungay
                //{
                //    try
                //    {
                //        fromDate = DateTime.Parse(searchFromDate);
                //        list = _unitOfWork.tonQuyRepository.Find(x => x.NgayCT >= fromDate).ToList();
                //        list = list.Where(x => x.MaCn == maCn).ToList();
                //    }
                //    catch (Exception)
                //    {
                //        return null;
                //    }

                //}
                if (!string.IsNullOrEmpty(searchToDate)) // denngay
                {
                    try
                    {
                        toDate = DateTime.Parse(searchToDate);
                        list = _unitOfWork.tinhTonRepository.Find(x => x.NgayCT < toDate.AddDays(1)).ToList();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            return list.OrderByDescending(x => x.NgayCT).FirstOrDefault();
        }

        public async Task<List<CTPhieuNX>> ListCTPhieuNX(string searchFromDate, string searchToDate)
        {
            var list = new List<CTPhieuNX>();

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

                    var cTPhieuNXes = await _unitOfWork.cTPhieuNXRepository
                                      .FindIncludeOneAsync(p => p.PhieuNX, x => x.PhieuNX.NgayLap >= fromDate &&
                                       x.PhieuNX.NgayLap < toDate.AddDays(1));
                    list = await cTPhieuNXes.ToListAsync();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchFromDate)) // NgayLap
                {
                    try
                    {
                        fromDate = DateTime.Parse(searchFromDate);
                        var cTPhieuNXes = await _unitOfWork.cTPhieuNXRepository
                                      .FindIncludeOneAsync(p => p.PhieuNX, x => x.PhieuNX.NgayLap >= fromDate);
                        list = await cTPhieuNXes.ToListAsync();
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
                        var cTPhieuNXes = await _unitOfWork.cTPhieuNXRepository
                                      .FindIncludeOneAsync(p => p.PhieuNX, x => x.PhieuNX.NgayLap < toDate.AddDays(1));
                        list = await cTPhieuNXes.ToListAsync();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            // search date

            list = list.OrderByDescending(x => x.PhieuNX.SoPhieu).ToList();

            return list;
        }

        public async Task<string> CheckTonDauStatus(DateTime fromDate)
        {
            //var date = DateTime.Now.AddDays(1);
            // ds tonquy truoc tuNgay
            List<TinhTon> tinhTons = new List<TinhTon>();
            try
            {
                tinhTons = _unitOfWork.tinhTonRepository.Find(x => x.NgayCT <= fromDate).ToList();

                if (tinhTons.Count == 0) return "";
            }
            catch (Exception ex)
            {
                return "";
            }
            // tonquy sau cung nhat
            TinhTon tinhTon = tinhTons.OrderByDescending(x => x.NgayCT).FirstOrDefault();

            // lay tat ca chi tiet truóc tuNgay(fromDate)
            var cTPhieuNXes = await _unitOfWork.cTPhieuNXRepository.FindIncludeOneAsync(x => x.PhieuNX, y => y.PhieuNX.NgayLap < fromDate.AddDays(1));
            string stringDate = "";

            // tonQuy.NgayCT (sau cung nhat) < nhung chi tiet < tuNggay (fromdate)
            for (DateTime i = tinhTon.NgayCT.Value.AddDays(1); i < fromDate; i = i.AddDays(1)) // chay tu ngay tonquy den fromday
            {
                var boolK = cTPhieuNXes.ToList().Exists(x => x.PhieuNX.NgayLap.Value.ToShortDateString() == i.ToShortDateString());
                if (boolK)
                {
                    stringDate += i.ToString("dd/MM/yyyy") + "-";
                }
            }

            return stringDate;
        }

        public async Task<TinhTon> GetById(long id)
        {
            return await _unitOfWork.tinhTonRepository.GetByLongIdAsync(id);
        }

        public async Task UpdateAsync(TinhTon tinhTon1)
        {
            _unitOfWork.tinhTonRepository.Update(tinhTon1);
            await _unitOfWork.Complete();
        }
    }
}