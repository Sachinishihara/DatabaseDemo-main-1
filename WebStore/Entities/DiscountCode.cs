//DiscountCode.cs
using System;
using System.Collections.Generic;
using WebStore.Entities;

namespace MyECommerce.Console.Entities
{
    public class DiscountCode
    {
        public int DiscountCodeId { get; set; }

        // e.g. "SUMMER2025"
        public string Code { get; set; } = null!;

        // e.g. "10% off entire order" or "$5 discount for new customers"
        public string? Description { get; set; }

        // Instead of string, use our enum
        public DiscountType DiscountType { get; set; }

        // e.g. 10 => 10% if "Percentage", or $10 if "Flat"
        public decimal DiscountValue { get; set; }

        // Code invalid after this date
        public DateTime? ExpirationDate { get; set; }

        // If single-use, set to 1, else null or larger number
        public int? MaxUsage { get; set; }

        // Times code used so far
        public int TimesUsed { get; set; }

        // Navigation property to Orders using this code
        public ICollection<Order>? Orders { get; set; } 
    }
}
