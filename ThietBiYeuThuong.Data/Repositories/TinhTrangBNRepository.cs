using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface ITinhTrangBNRepository : IRepository<TinhTrangBN>
    {
    }

    public class TinhTrangBNRepository : Repository<TinhTrangBN>, ITinhTrangBNRepository
    {
        public TinhTrangBNRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}