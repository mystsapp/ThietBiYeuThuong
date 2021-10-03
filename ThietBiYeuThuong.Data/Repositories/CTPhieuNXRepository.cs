using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface ICTPhieuNXRepository : IRepository<CTHoSoBN>
    {
    }

    public class CTPhieuNXRepository : Repository<CTHoSoBN>, ICTPhieuNXRepository
    {
        public CTPhieuNXRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}