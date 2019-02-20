using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Project.Domain;
using Project.Entity.Context;
using Project.Domain.Service;
using Microsoft.Extensions.Logging;
using Serilog;
using Project.Entity;

namespace Project.Api.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;
        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        //get all
       // [Authorize]
        [HttpGet]
        public IEnumerable<RoleViewModel> GetAll()
        {
            var test = _roleService.DoNothing();
            var items = _roleService.GetAll();
            return items;
        }

        [HttpGet("User/{id}")]
        public IEnumerable<RoleViewModel> GetUserRoles(int id)
        {
            var test = _roleService.DoNothing();
            var items = _roleService.GetUserRoles(id);
            return items;
        }
        //get one
        //[Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _roleService.GetOne(id);
            if (item == null)
            {
                Log.Error("GetById({ ID}) NOT FOUND", id);
                return NotFound();
            }

            return Ok(item);
        }

        //add
        //[Authorize(Roles = "Administrator")]
        [HttpPut]
        public IActionResult Create([FromBody] RoleViewModel role)
        {
            if (role == null)
                return BadRequest();

            //  var id = _roleService.Add(role);
            //  return Created($"api/Role/{id}", id);  //HTTP201 Resource created
            return null;
        }

        //update
        //[Authorize(Roles = "Administrator")]
        [HttpPost("{id}")]
        public IActionResult Update(int id, [FromBody] RoleViewModel role)
        {
            if (role == null || role.Id != id)
                return BadRequest();

          //  if (_roleService.Update(role))
          //      return Accepted(role);
          //  else
                return StatusCode(304);     //Not Modified
        }

        //delete -- it will check ClaimTypes.Role
        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
          //  if (_roleService.Remove(id))
          //      return NoContent();          //204
         //   else
                return NotFound();           //404
        }

    }
}


