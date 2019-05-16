using SCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace SCore.BLL.Models
{
    public class Cart
    {
        private List<ProductOrder> lineCollection = new List<ProductOrder>();
        public virtual void AddItem(Product product, int quantity)
        {
            ProductOrder line = lineCollection.Where(p => p.Product.ProductId == product.ProductId).FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new ProductOrder { Product = product, Amount = quantity });
            }
            else
            {
                line.Amount += quantity;
            }
        }
        public virtual void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId);
        }
        public decimal Totalvalue()
        {
            return lineCollection.Sum(e => e.Product.Price * e.Amount);
        }
        public virtual void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<ProductOrder> Lines
        {
            get { return lineCollection; }
        }
    }
}
