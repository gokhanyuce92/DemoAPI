using Demo.Entities;
using Demo.Repositories.Abstract;

namespace Demo.Repositories.Concrete 
{
    public class ControllerActionRoleRepository : GenericRepository<ControllerActionRole>, IControllerActionRoleRepository
    {
        public ControllerActionRoleRepository(MyDbContext context) : base(context)
        {
        }
    }
}