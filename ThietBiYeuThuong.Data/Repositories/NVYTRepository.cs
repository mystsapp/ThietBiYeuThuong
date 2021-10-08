using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface INVYTRepository : IRepository<NhanVienYTe>
    {
    }

    public class NVYTRepository : Repository<NhanVienYTe>, INVYTRepository
    {
        public NVYTRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}