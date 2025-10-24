namespace OrderService.Models
{
    public class ProductOrder
    {
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Amount { get; set; }
    public Order Order { get; set; }
    public Product Product { get; set; }
    }
}