using System;
using System.Collections.Generic;

namespace WebStore.Entities
{
    public partial class Store
    {
        public Store()
        {
            Stocks = new HashSet<Stock>();
            staff = new HashSet<staff>();
        }

        public int StoreId { get; set; }
        public string StoreName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<staff> staff { get; set; }
    }
}
