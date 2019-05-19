using System.ComponentModel.DataAnnotations;


namespace SCore.Models
{
    public class ProductOrder
    {
        public int Id { get; set; }

        [Display(Name = "Amount")]
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
