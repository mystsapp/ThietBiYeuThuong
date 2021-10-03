using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface IPhieuNXRepository : IRepository<HoSoBN>
    {
    }

    public class PhieuNXRepository : Repository<HoSoBN>, IPhieuNXRepository
    {
        public PhieuNXRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}