using Microsoft.EntityFrameworkCore;
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

        public Order GetOrder(int orderID)
        {
            return _context.Orders.Where(o => o.OrderID == orderID).Include(o => o.ProductOrders).FirstOrDefault()!;
        }

        public ICollection<Order> GetOrders()
        {
            return [.. _context.Orders.Include(o => o.ProductOrders)];
        }

        public ICollection<Order> GetOrders(int userId)
        {
            return [.. _context.Orders.Where(o => o.UserId == userId).Include(o => o.ProductOrders)];
        }

        public ICollection<Order> GetOrdersBetween(DateTime startDate, DateTime endDate)
        {
            return [.. _context.Orders.Where(o => startDate <= o.Date && o.Date <= endDate).Include(o => o.ProductOrders)];
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