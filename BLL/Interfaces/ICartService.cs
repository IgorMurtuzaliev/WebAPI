using SCore.BLL.Models;

namespace SCore.BLL.Interfaces
{
    public interface ICartService
    {
        void AddToCart(Cart cart, int? id);
        void RemoveFromCart(Cart cart, int? id);
    }
}
