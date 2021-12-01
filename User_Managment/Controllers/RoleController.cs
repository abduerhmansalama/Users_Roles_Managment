using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Managment.DTO;
using User_Managment.Models;

namespace User_Managment.Controllers
{
    [ApiController]
    [Route("Role")]
    public class RoleController : ControllerBase
    {

        private readonly ApplicationDb _applicationDb;

        public RoleController(ApplicationDb applicationDb)
        {
            _applicationDb = applicationDb;
        }

        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            var roles = _applicationDb.roles.Include(r => r.users).ToList();
            
            return Ok(roles);
        }
        [HttpPost("AddRole")]
        public IActionResult AddRole(AddRoleDTO model)
        {
              var roles =  _applicationDb.roles.Select(s=>s.Name).ToList();


            if (!roles.Contains(model.Name)) {
                _applicationDb.roles.Add(new Role { Name=model.Name});
                _applicationDb.SaveChanges();
                return Ok("Role is added succsfully");
            }
            else
            {
            return StatusCode(403,"Role already exist");
            }
      
        }
        [HttpPut("EditeRole")]
        public IActionResult Edite(Role role)
        {
          var _role = _applicationDb.roles.Find(role.Id);
            if (_role == null) { return NotFound(); }
            _role.Name = role.Name;
            _applicationDb.Update(role);
            _applicationDb.SaveChanges();
            return Ok($"{role.Name} updated");
        }
        [HttpDelete("DeleteRole")]
        public IActionResult DeleteRole(int id) {           
            var role = _applicationDb.roles.Find(id);
            if(role == null)
            {
                return NotFound();
            }
            _applicationDb.roles.Remove(role);
            _applicationDb.SaveChanges();
            return Ok();
        }

        [HttpGet("GetRoleUsers")]
        public IActionResult GetRoleUsers(int id)
        {
            if (_applicationDb.roles.Find(id) == null) { return NotFound(); }

            IEnumerable<User> rolesUsers =_applicationDb.roles.Where(r=>r.Id==id).SelectMany(u=>u.users);
                
            return Ok(rolesUsers.Select(s=>new { s.Id , s.Name}));
        }
    }
}
