using SolarCoffee.Data.Enums;
using System;

namespace SolarCoffee.Data.Models
{
    public class InventoryAccounting
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public DateTime CreatedOn { get; set; }
        public ProductTransactionType TransactionType { get; set; }
    }
}