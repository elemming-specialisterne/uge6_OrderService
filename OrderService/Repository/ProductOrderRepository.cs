using OrderService.Data;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Repository
{
    public class ProductOrderRepository(OrderContext context) : IProductOrderRepository
    {
        private readonly OrderContext _context = context;

        public bool CreateProductOrder(ProductOrder productOrder)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProductOrder(ProductOrder productOrder)
        {
            throw new NotImplementedException();
        }

        public ICollection<ProductOrder> GetProductFromProducts(int productID)
        {
            throw new NotImplementedException();
        }

        public ProductOrder GetProductOrder(int orderID, int ProductID)
        {
            throw new NotImplementedException();
        }

        public ICollection<ProductOrder> GetProductOrders()
        {
            throw new NotImplementedException();
        }

        public ICollection<ProductOrder> GetProductOrders(int orderID)
        {
            throw new NotImplementedException();
        }

        public bool ProductOrderExists(int orderID, int ProductID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProductOrder(ProductOrder productOrder)
        {
            throw new NotImplementedException();
        }
    }
}