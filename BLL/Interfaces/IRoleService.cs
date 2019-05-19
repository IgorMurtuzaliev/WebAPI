using Microsoft.AspNetCore.Identity;
using SCore.BLL.Models;
using SCore.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCore.BLL.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> Create(CreateRoleModel model);
        IEnumerable<ApplicationRole> GetAll();
        Task<IdentityResult> Edit(EditRoleModel model);
        Task Edit(string id);
        Task<IdentityResult> Delete(string id);
        Task<ApplicationRole> GetRole(string id);
    }
}
