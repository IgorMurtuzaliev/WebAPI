using Microsoft.AspNetCore.Identity;
using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCore.BLL.Services
{
    public class RoleService:IRoleService
    {
        private RoleManager<ApplicationRole> _roleManager;
        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Create(CreateRoleModel model)
        {
            IdentityResult result = await _roleManager.CreateAsync(new ApplicationRole
            {
                Name = model.Name,
            });
            return result;
        }

        public async Task<IdentityResult> Delete(string id)
        {
            ApplicationRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                return result;
            }
            return null;
        }

        public async Task<IdentityResult> Edit(EditRoleModel model)
        {
            ApplicationRole role = await _roleManager.FindByIdAsync(model.Id);
            role.Name = model.Name;
            IdentityResult result = await _roleManager.UpdateAsync(role);
            return result;

        }

        public async Task Edit(string id)
        {
            ApplicationRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                new EditRoleModel { Id = role.Id, Name = role.Name };
            }
        }
        public IEnumerable<ApplicationRole> GetAll()
        {
           var roles = _roleManager.Roles.ToList();
           return roles;
        }

    }

}
