using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.DAL.EF;
using SCore.Models.Entities;
using SCore.WEB.ViewModels;

namespace SCore.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RolesController(IRoleService _roleService)
        {
            roleService = _roleService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ApplicationRole>> GetRoles()
        {
            return Ok(roleService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationRole>> GetRole(string id)
        {
            var applicationRole =  await roleService.GetRole(id);

            if (applicationRole == null)
            {
                return NotFound();
            }
            return applicationRole;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditRole(string id, RoleViewModelEdit model)
        {
            var role = new EditRoleModel { Name = model.Name, Id = model.Id };
            if (id != role.Id)
            {
                return BadRequest();
            }

            await roleService.Edit(role);
            try
            {
                await roleService.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationRoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationRole>> CreateRole(RoleViewModel model)
        {
            var role = new CreateRoleModel { Name = model.Name };
            await roleService.Create(role);
            await roleService.Save();

            return CreatedAtAction("GetApplicationRole", new { id = model.Id }, role);
        }

        // DELETE: api/ApplicationRoles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationRole>> DeleteRole(string id)
        {
            var applicationRole = await roleService.GetRole(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            await roleService.Delete(id);
            await roleService.Save();

            return applicationRole;
        }

        private bool ApplicationRoleExists(string id)
        {
            return roleService.RoleExists(id);
        }
    }
}
