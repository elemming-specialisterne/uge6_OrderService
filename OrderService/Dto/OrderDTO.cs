namespace OrderService.Dto
{
    public class OrderDto
    {
    public int OrderID { get; set; }
    public int UserId { get; set; }
    public double Price { get; set; }
    public DateTime Date { get; set; }

    public ICollection<OrderItemDto> Products { get; set; }
    }
}