using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Models;

[Table("orders")]
[Index("Userid", Name = "idx_orders_userid")]
[Index("ExternalRef", Name = "ux_orders_external_ref", IsUnique = true)]
public partial class Order
{
    [Key]
    [Column("orderid")]
    public long Orderid { get; set; }

    [Column("userid")]
    public long Userid { get; set; }

    [Column("total")]
    [Precision(12, 2)]
    public decimal Total { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("external_ref")]
    public string? ExternalRef { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("Userid")]
    [InverseProperty("Orders")]
    public virtual User User { get; set; } = null!;
}
