using Microsoft.AspNetCore.Identity;
using SCore.BLL.Models;
using System.Threading.Tasks;

namespace SCore.BLL.Interfaces
{
    public interface IAccountService
    {
       Task<IdentityResult> Create(RegisterModel registerViewModel, string url);
       Task<SignInResult> LogIn(LoginModel loginViewModel);
       Task Logout();
       Task<IdentityResult> ConfirmEmail(string userId, string code);
    }
}
