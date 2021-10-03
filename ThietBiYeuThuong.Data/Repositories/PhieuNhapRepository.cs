using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface IPhieuNhapRepository : IRepository<PhieuNhap>
    {
    }

    public class PhieuNhapRepository : Repository<PhieuNhap>, IPhieuNhapRepository
    {
        public PhieuNhapRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}