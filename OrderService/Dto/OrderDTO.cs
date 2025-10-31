namespace OrderService.Dto
{
    public class OrderDto
    {
    public int Orderid { get; set; }
    public int Userid { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<OrderItemDto> OrderItems { get; set; }
    }
}