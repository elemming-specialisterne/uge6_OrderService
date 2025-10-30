using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Models;

[PrimaryKey("Orderid", "Productid")]
[Table("order_items")]
[Index("Productid", Name = "idx_items_productid")]
public partial class OrderItem
{
    [Key]
    [Column("orderid")]
    public long Orderid { get; set; }

    [Key]
    [Column("productid")]
    public long Productid { get; set; }

    [Column("qty")]
    public int Qty { get; set; }

    [Column("unit_price")]
    [Precision(10, 2)]
    public decimal UnitPrice { get; set; }

    [Column("line_total")]
    [Precision(12, 2)]
    public decimal? LineTotal { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("Orderid")]
    [InverseProperty("OrderItems")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("Productid")]
    [InverseProperty("OrderItems")]
    public virtual Product Product { get; set; } = null!;
}
