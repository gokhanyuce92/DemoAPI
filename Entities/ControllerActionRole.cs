using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Demo.Entities
{
    public class ControllerActionRole
    {
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
    }
}