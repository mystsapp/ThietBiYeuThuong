using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Repositories;
using ThietBiYeuThuong.Data.Utilities;

namespace ThietBiYeuThuong.Web.Services
{
    public interface ICTPhieuNXService
    {
        Task<IEnumerable<CTPhieuNX>> List_CTPhieuNX_By_PhieuNXId(string phieuNXId);

        Task Create(CTPhieuNX cTPhieuNX);

        Task UpdateAsync(CTPhieuNX cTPhieuNX);

        string GetSoPhieuCT(string param);

        Task<List<CTPhieuNX>> GetCTTrongNgay();

        Task<CTPhieuNX> GetById(string id);

        Task DeleteAsync(CTPhieuNX cTPhieuNX);

        Task<string> CheckTonDau(DateTime fromDate);

        CTPhieuNX GetByIdAsNoTracking(string id);
    }

    public class CTPhieuNXService : ICTPhieuNXService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CTPhieuNXService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(CTPhieuNX cTPhieuNX)
        {
            _unitOfWork.cTPhieuNXRepository.Create(cTPhieuNX);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(CTPhieuNX cTPhieuNX)
        {
            _unitOfWork.cTPhieuNXRepository.Delete(cTPhieuNX);
            await _unitOfWork.Complete();
        }

        public async Task<CTPhieuNX> GetById(string id)
        {
            return await _unitOfWork.cTPhieuNXRepository.GetByIdAsync(id);
        }

        public async Task<List<CTPhieuNX>> GetCTTrongNgay()
        {
            var cTPhieuNXes = _unitOfWork.cTPhieuNXRepository.GetAll();//.FindAsync(x => x.NgayNhap.Value.ToShortDateString() == DateTime.Now.ToShortDateString());
            if (cTPhieuNXes.Count() == 0) return null;
            else
            {
                return cTPhieuNXes.Where(x => x.NgayNhap.Value.ToShortDateString() == DateTime.Now.ToShortDateString()).ToList();
            }
        }

        public string GetSoPhieuCT(string param)
        {
            var currentYear = DateTime.Now.Year; // ngay hien tai
            var subfix = param + currentYear.ToString(); // QT2021? ?QC2021? ?NT2021? ?NC2021?
            var cTPhieuNXes = _unitOfWork.cTPhieuNXRepository
                                   .Find(x => x.SoPhieuCT.Trim()
                                   .Contains(subfix)).ToList();// chi lay nhung SoPhieu cung param: N, X + năm
            var cTPhieuNX = new CTPhieuNX();
            if (cTPhieuNXes.Count() > 0)
            {
                cTPhieuNX = cTPhieuNXes.OrderByDescending(x => x.SoPhieuCT).FirstOrDefault();
            }

            if (cTPhieuNX == null || string.IsNullOrEmpty(cTPhieuNX.SoPhieuCT))
            {
                return GetNextId.NextID("", "") + subfix; // 0001
            }
            else
            {
                var oldYear = cTPhieuNX.SoPhieuCT.Substring(6, 4);

                // cung nam
                if (oldYear == currentYear.ToString())
                {
                    var oldSoCT = cTPhieuNX.SoPhieuCT.Substring(0, 4);
                    return GetNextId.NextID(oldSoCT, "") + subfix;
                }
                else
                {
                    // sang nam khac' chay lai tu dau
                    return GetNextId.NextID("", "") + subfix; // 0001
                }
            }
        }

        public async Task<IEnumerable<CTPhieuNX>> List_CTPhieuNX_By_PhieuNXId(string phieuNXId)
        {
            return await _unitOfWork.cTPhieuNXRepository.FindIncludeOneAsync(x => x.PhieuNX, x => x.PhieuNXId == phieuNXId);
        }

        public async Task<string> CheckTonDau(DateTime fromDate)
        {
            //var date = DateTime.Now.AddDays(1);
            // ds tonquy truoc tuNgay
            List<TinhTon> tinhTons = new List<TinhTon>();
            try
            {
                // lay tat ca chi tiet truóc tuNgay(fromDate)
                var cTPhieuNXes = await _unitOfWork.cTPhieuNXRepository
                                                   .FindIncludeOneAsync(x => x.PhieuNX, y => y.NgayTao < fromDate.AddDays(1));
                string stringDate = "";

                tinhTons = _unitOfWork.tinhTonRepository.Find(x => x.NgayCT <= fromDate).ToList();

                if (tinhTons.Count == 0)
                {
                    var stringDates = cTPhieuNXes.Select(x => x.NgayTao.Value.ToShortDateString()).Distinct();
                    foreach (var item in stringDates)
                    {
                        stringDate += item + "-";
                    }
                }
                else
                {
                    // tonquy sau cung nhat
                    TinhTon tinhTon = tinhTons.OrderByDescending(x => x.NgayCT).FirstOrDefault();

                    // tonQuy.NgayCT (sau cung nhat) < nhung chi tiet < tuNggay (fromdate)
                    for (DateTime i = tinhTon.NgayCT.Value.AddDays(1); i < fromDate; i = i.AddDays(1)) // chay tu ngay tonquy den fromday
                    {
                        var boolK = cTPhieuNXes.ToList().Exists(x => x.NgayTao.Value.ToShortDateString() == i.ToShortDateString());
                        if (boolK)
                        {
                            stringDate += i.ToString("dd/MM/yyyy") + "-";
                        }
                    }
                }

                return stringDate;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public CTPhieuNX GetByIdAsNoTracking(string id)
        {
            return _unitOfWork.cTPhieuNXRepository.GetByIdAsNoTracking(x => x.SoPhieuCT == id);
        }

        public async Task UpdateAsync(CTPhieuNX cTPhieuNX)
        {
            _unitOfWork.cTPhieuNXRepository.Update(cTPhieuNX);
            await _unitOfWork.Complete();
        }
    }
}