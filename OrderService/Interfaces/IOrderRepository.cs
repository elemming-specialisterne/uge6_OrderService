using OrderService.Models;
namespace OrderService.Interfaces
{
    public interface IOrderRepository
    {
        ICollection<Order> GetOrders();
        ICollection<Order> GetOrders(int userId);
        ICollection<Order> GetOrdersBetween(DateTime startDate, DateTime endDate);
        Order GetOrder(int orderID);
        bool OrderExists(int orderID);
        bool CreateOrder(Order order);
        bool UpdateOrder(Order order);
        bool DeleteOrder(Order order);


    }
}