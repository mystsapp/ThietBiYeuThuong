using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Interface;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface ICTHoSoBNRepository : IRepository<CTHoSoBN>
    {
    }

    public class CTHoSoBNRepository : Repository<CTHoSoBN>, ICTHoSoBNRepository
    {
        public CTHoSoBNRepository(ThietBiYeuThuongDbContext context) : base(context)
        {
        }
    }
}