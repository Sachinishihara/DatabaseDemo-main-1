using System;
using System.Collections.Generic;
using MyECommerce.Console.Entities;

namespace WebStore.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? OrderStatus { get; set; }

    public int ShippingAddressId { get; set; }

    public int BillingAddressId { get; set; }

    public int? CarrierId { get; set; }
    
    public virtual Carrier? Carrier { get; set; }

    public string? TrackingNumber { get; set; }

    public DateTime? ShippedDate { get; set; }
    
    public DateTime? DeliveredDate { get; set; }

    public int? DiscountCodeId { get; set; }

    public DiscountCode? DiscountCode { get; set; }

    public virtual Address BillingAddress { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Address ShippingAddress { get; set; } = null!;
}
