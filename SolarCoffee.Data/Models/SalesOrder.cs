using SolarCoffee.Data.Enums;
using System;
using System.Collections.Generic;

namespace SolarCoffee.Data.Models
{
    public class SalesOrder
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public List<SalesOrderItem> SalesOrderItems { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Delivery Delivery { get; set; }
        public Payment Payment { get; set; }
        public Discount Discount { get; set; }
        public string AdditionalInfo { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}