using SCore.BLL.Models;
using System.Threading.Tasks;

namespace SCore.BLL.Interfaces
{
    public interface ICartService
    {
        Task AddToCart(Cart cart, int? id);
        Task RemoveFromCart(Cart cart, int? id);
    }
}
