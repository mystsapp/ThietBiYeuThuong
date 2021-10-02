using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface ILoaiThietBiRepository : IRepository<LoaiThietBi>
    {
    }

    public class LoaiThietBiRepository : Repository<LoaiThietBi>, ILoaiThietBiRepository
    {
        public LoaiThietBiRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}