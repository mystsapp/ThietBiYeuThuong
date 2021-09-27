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
        }

        public IUserRepository userRepository { get; }

        public IRoleRepository roleRepository { get; }

        public ITinhTonRepository tinhTonRepository { get; }

        public IPhieuNXRepository phieuNXRepository { get; }

        public ICTPhieuNXRepository cTPhieuNXRepository { get; }

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