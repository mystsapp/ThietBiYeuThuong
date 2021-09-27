using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface ITinhTonRepository : IRepository<TinhTon>
    {
    }

    public class TinhTonRepository : Repository<TinhTon>, ITinhTonRepository
    {
        public TinhTonRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}