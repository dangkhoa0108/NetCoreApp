using CoreApp.Infrastructure.Interfaces;

namespace CoreApp.Data.EF
{
    public class EFUnitOfWork:IUnitOfWork
    {
        private readonly AppDbContext _context;

        public EFUnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
