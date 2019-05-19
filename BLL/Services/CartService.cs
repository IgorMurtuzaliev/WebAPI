using Microsoft.AspNetCore.Mvc;
using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.DAL.Interfaces;
using SCore.Models;
using System.Threading.Tasks;

namespace SCore.BLL.Services
{
    public class CartService : ICartService
    {
        IUnitOfWork db { get; set; }
        public CartService(IUnitOfWork _db)
        {
            db = _db;
        }
        public async Task AddToCart(Cart cart, int? id)
        {
            Product product = await db.Products.Get(id);
            if (id == null)
            {
                new BadRequestResult();
            }

            if (product != null)
            {
              cart.AddItem(product, 1);
            }
        }
       public async Task RemoveFromCart(Cart cart, int? id)
        {
            Product product = await db.Products.Get(id);
            if (id == null)
            {
                new BadRequestResult();
            }

            if (product != null)
            {
                cart.RemoveLine(product);
            }
        }

    }
}
