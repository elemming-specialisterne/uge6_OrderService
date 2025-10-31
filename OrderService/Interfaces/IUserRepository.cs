using OrderService.Models;

namespace OrderService.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
    }
}