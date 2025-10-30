using OrderService.Data;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Repository
{
    public class OrderItemRepository(OrderContext context) : IOrderItemRepository
    {
        private readonly OrderContext _context = context;

        public bool CreateProductOrder(OrderItem productOrder)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProductOrder(OrderItem productOrder)
        {
            throw new NotImplementedException();
        }

        public ICollection<OrderItem> GetProductFromProducts(int productID)
        {
            throw new NotImplementedException();
        }

        public OrderItem GetProductOrder(int orderID, int ProductID)
        {
            throw new NotImplementedException();
        }

        public ICollection<OrderItem> GetProductOrders()
        {
            throw new NotImplementedException();
        }

        public ICollection<OrderItem> GetProductOrders(int orderID)
        {
            throw new NotImplementedException();
        }

        public bool ProductOrderExists(int orderID, int ProductID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProductOrder(OrderItem productOrder)
        {
            throw new NotImplementedException();
        }
    }
}