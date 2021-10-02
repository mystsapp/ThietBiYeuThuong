using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface IThietBiRepository : IRepository<ThietBi>
    {
    }

    public class ThietBiRepository : Repository<ThietBi>, IThietBiRepository
    {
        public ThietBiRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}