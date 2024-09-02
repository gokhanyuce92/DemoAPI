using Demo.Entities;
using Demo.Repositories.Abstract;

namespace Demo.Repositories.Concrete
{
    public class CalisanRepository : GenericRepository<Calisan>, ICalisanRepository
    {
        public CalisanRepository(MyDbContext context) : base(context)
        {
        }
    }
}