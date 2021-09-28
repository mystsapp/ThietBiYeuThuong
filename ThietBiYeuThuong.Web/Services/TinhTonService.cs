using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ThietBiYeuThuong.Web.Services
{
    public interface ITinhTonService
    {
        TinhTon GetLast();
    }

    public class TinhTonService : ITinhTonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TinhTonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TinhTon GetLast()
        {
            return _unitOfWork.tinhTonRepository.GetAll().OrderByDescending(x => x.NgayTao).FirstOrDefault();
        }
    }
}