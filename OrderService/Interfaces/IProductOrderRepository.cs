using OrderService.Models;

namespace OrderService.Interfaces
{
    public interface IProductOrderRepository
    {
        ICollection<ProductOrder> GetProductOrders();
        ICollection<ProductOrder> GetProductOrders(int orderID);
        ICollection<ProductOrder> GetProductFromProducts(int productID);
        ProductOrder GetProductOrder(int orderID, int ProductID);
        bool ProductOrderExists(int orderID, int ProductID);
        bool CreateProductOrder(ProductOrder productOrder);
        bool UpdateProductOrder(ProductOrder productOrder);
        bool DeleteProductOrder(ProductOrder productOrder);
    }
}