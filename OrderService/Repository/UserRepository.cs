using OrderService.Data;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Repository
{
    public class UserRepository(OrderContext context) : IUserRepository
    {
        private readonly OrderContext _context = context;

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
        public ICollection<User> GetUsers()
        {
            return [.. _context.Users];
        }
    }
}