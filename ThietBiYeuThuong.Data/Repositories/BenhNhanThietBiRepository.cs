using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface IBenhNhanThietBiRepository : IRepository<BenhNhanThietBi>
    {
    }

    public class BenhNhanThietBiRepository : Repository<BenhNhanThietBi>, IBenhNhanThietBiRepository
    {
        public BenhNhanThietBiRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}