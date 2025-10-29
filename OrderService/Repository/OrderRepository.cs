using OrderService.Data;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Repository
{
    public class OrderRepository(OrderContext context) : IOrderRepository
    {
        private readonly OrderContext _context = context;

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool CreateOrder(Order order)
        {
            _context.Add(order);
            return Save();
        }

        public bool DeleteOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public ICollection<Order> GetCurrentOrders()
        {
            throw new NotImplementedException();
        }

        public Order GetOrder(int orderID)
        {
            throw new NotImplementedException();
        }

        public ICollection<Order> GetOrders()
        {
            throw new NotImplementedException();
        }

        public ICollection<Order> GetOrders(int userId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Order> GetOrdersBetween(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public bool OrderExists(int orderID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}