using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCore.Models
{   [Serializable]
    public class Order
    {
        public Order()
        {
            ProductOrders = new List<ProductOrder>();
        }
        public int OrderId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of order")]
        public DateTime TimeOfOrder { get; set; } = DateTime.Now;

        public string UserId { get; set; }

        [Display(Name = "Total")]
        public int Sum { get; set; }
         
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
