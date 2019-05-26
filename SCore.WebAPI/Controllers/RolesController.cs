using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RolesController(IRoleService _roleService)
        {
            roleService = _roleService;
        }

        [HttpGet]
       // [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<ApplicationRole>> GetRoles()
        {
            return Ok(roleService.GetAll());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApplicationRole>> GetRole(string id)
        {
            var applicationRole =  await roleService.GetRole(id);

            if (applicationRole == null)
            {
                return NotFound("Role's not found");
            }
            return applicationRole;
        }

        [HttpPut("{id}")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, RoleViewModelEdit model)
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
                if (!RoleExists(id))
                {
                    return NotFound("Role's not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
       // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApplicationRole>> Create(RoleViewModel model)
        {
            var role = new CreateRoleModel { Name = model.Name };
            await roleService.Create(role);
            await roleService.Save();

            return CreatedAtAction("GetApplicationRole", new { id = model.Id }, role);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationRole>> Delete(string id)
        {
            var applicationRole = await roleService.GetRole(id);
            if (applicationRole == null)
            {
                return NotFound("Role's not found");
            }

            await roleService.Delete(id);
            await roleService.Save();

            return applicationRole;
        }

        private bool RoleExists(string id)
        {
            return roleService.RoleExists(id);
        }
    }
}
