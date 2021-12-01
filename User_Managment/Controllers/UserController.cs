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
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDb _applicationDb;

        public UserController(ApplicationDb applicationDb)
        {
            _applicationDb = applicationDb;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            var users =_applicationDb.users.Include(r=>r.roles).ToList();
            
            return Ok(users);
        }
        
        [HttpPost("CreateUser")]      
        public IActionResult CreateUser(UserCreateDTO userroles)
        {
            if (userroles == null)
            {
                return BadRequest("something error can't create user");
            }
            List<Role> roles = new List<Role>();
            for(int i =0; i < userroles.RolesId.Count; i++)
            {
                roles.Add(_applicationDb.roles.Find(userroles.RolesId[i]));
            }           
            User user = new User()
            {
                Email = userroles.Email,
                Name = userroles.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(userroles.Password),
                roles = roles
            };
            _applicationDb.users.Add(user);
            _applicationDb.SaveChanges();

            return Ok("user Created Successfully");
        }
        
        [HttpGet("Details")]
        public IActionResult Details(int id)
        {
           var user =  _applicationDb.users.Include(i=>i.roles).SingleOrDefault(s=>s.Id==id);
            if (user == null)
            {
                return NotFound("user not foun");
            }

            return Ok(user);
        }
       
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var user = _applicationDb.users.Find(id);
            if (user == null)
            {
                return NotFound("user not foun");
            }
            _applicationDb.Remove(user);
            _applicationDb.SaveChanges();
            return Ok($"{user.Name} Deleted Succefully");
        }

        [HttpPut("edite")]
        public IActionResult edit(editeForm model)
        {
            var user = _applicationDb.users.Find(model.Id);
        

            List<Role> roles1 = new List<Role>();
            List<Role> roles2 = new List<Role>();



            for (int i = 0; i < model.roles.Count; i++)
            {

                var r = _applicationDb.roles.Find(model.roles[i]);
                if (r == null)
                {
                    return NotFound();
                }
                roles1.Add(r);
            }
            /*
              foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }

             */

            foreach (var role in roles1)
            {
                if (user.roles.Any(r => r == role))
                    user.roles.Remove(role);

                if (!user.roles.Any(r => r == role) && roles1.Any(r => r == role))
                    user.roles.Add(role);

            }


            //for (int i = 0; i < roles1.Count; i++)
            //{
            //    if (user.roles.ToList().Contains(roles1[i])) 
            //    {
            //        roles2.Add(roles1[i]);

            //    }
            //    else user.roles.Add(roles1[i]);                
            //}


            user.Email = model.Email;
            user.Name = model.Name;
            user.Password = model.Password;

            _applicationDb.users.Update(user);
            _applicationDb.SaveChanges();

            return Ok($"{user.Name}'s info updated successfully ");

        }

        [HttpGet("GetUserRoles")]
        public IActionResult GetUserRoles(int id)
        {
            if (_applicationDb.users.Find(id) == null) { return NotFound(); }

            IEnumerable<Role> userRoles = from role in _applicationDb.roles
                                   where role.users.Any(c => c.Id == id)
                                   select role;

            if (userRoles == null) { NotFound("not"); }
            return Ok(userRoles.Select(s=>new {s.Id,s.Name}));
        }

        [HttpPost("assigneRolesToUser")]      
        public IActionResult assigneRolesToUser(int roleId,int userId)
        {
            var user = _applicationDb.users.Find(userId);
            var role = _applicationDb.roles.Find(roleId);
            List<Role> userRoles = user.roles.ToList();




            if (!user.roles.Contains(role))
            {
                user.roles.Add(role);
                _applicationDb.SaveChanges();
                return Ok(user.roles.ToList());
            }

            else return StatusCode(403);
        }

    }
}
