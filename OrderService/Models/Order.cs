namespace OrderService.Models
{
    public class Order
    {
    public int OrderID { get; set; }
    public int UserId { get; set; }
    public double Price { get; set; }
    public DateTime Date { get; set; }
    public ICollection<ProductOrder> ProductOrders { get; set; }
    }
}