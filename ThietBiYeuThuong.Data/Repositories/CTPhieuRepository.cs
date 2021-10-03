using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface ICTPhieuRepository : IRepository<CTPhieu>
    {
    }

    public class CTPhieuRepository : Repository<CTPhieu>, ICTPhieuRepository
    {
        public CTPhieuRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}