using OrderService.Models;

namespace OrderService.Interfaces
{
    public interface IOrderItemRepository
    {
        ICollection<OrderItem> GetProductOrders();
        ICollection<OrderItem> GetProductOrders(int orderID);
        ICollection<OrderItem> GetProductFromProducts(int productID);
        OrderItem GetProductOrder(int orderID, int ProductID);
        bool ProductOrderExists(int orderID, int ProductID);
        bool CreateProductOrder(OrderItem productOrder);
        bool UpdateProductOrder(OrderItem productOrder);
        bool DeleteProductOrder(OrderItem productOrder);
    }
}