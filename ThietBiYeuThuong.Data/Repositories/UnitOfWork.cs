using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;

namespace ThietBiYeuThuong.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository userRepository { get; }
        IRoleRepository roleRepository { get; }
        ITinhTonRepository tinhTonRepository { get; }
        IPhieuNXRepository phieuNXRepository { get; }
        ICTPhieuNXRepository cTPhieuNXRepository { get; }
        ILoaiThietBiRepository loaiThietBiRepository { get; }
        IBenhNhanRepository benhNhanRepository { get; }
        IBenhNhanThietBiRepository benhNhanThietBiRepository { get; }
        IThietBiRepository thietBiRepository { get; }
        ITinhTrangBNRepository tinhTrangBNRepository { get; }
        ITrangThaiRepository trangThaiRepository { get; }

        Task<int> Complete();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ThietBiYeuThuongDbContext _context;

        public UnitOfWork(ThietBiYeuThuongDbContext context)
        {
            _context = context;

            userRepository = new UserRepository(_context);
            roleRepository = new RoleRepository(_context);
            tinhTonRepository = new TinhTonRepository(_context);
            phieuNXRepository = new PhieuNXRepository(_context);
            cTPhieuNXRepository = new CTPhieuNXRepository(_context);
            loaiThietBiRepository = new LoaiThietBiRepository(_context);
            benhNhanRepository = new BenhNhanRepository(_context);
            benhNhanThietBiRepository = new BenhNhanThietBiRepository(_context);
            thietBiRepository = new ThietBiRepository(_context);
            tinhTrangBNRepository = new TinhTrangBNRepository(_context);
            trangThaiRepository = new TrangThaiRepository(_context);
        }

        public IUserRepository userRepository { get; }

        public IRoleRepository roleRepository { get; }

        public ITinhTonRepository tinhTonRepository { get; }

        public IPhieuNXRepository phieuNXRepository { get; }

        public ICTPhieuNXRepository cTPhieuNXRepository { get; }

        public ILoaiThietBiRepository loaiThietBiRepository { get; }

        public IBenhNhanRepository benhNhanRepository { get; }

        public IBenhNhanThietBiRepository benhNhanThietBiRepository { get; }
        public IThietBiRepository thietBiRepository { get; }

        public ITinhTrangBNRepository tinhTrangBNRepository { get; }

        public ITrangThaiRepository trangThaiRepository { get; }

        public async Task<int> Complete()
        {
            await _context.SaveChangesAsync();

            return 1;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.Collect();
        }
    }
}