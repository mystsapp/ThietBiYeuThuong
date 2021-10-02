using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface IBenhNhanRepository : IRepository<BenhNhan>
    {
    }

    public class BenhNhanRepository : Repository<BenhNhan>, IBenhNhanRepository
    {
        public BenhNhanRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}