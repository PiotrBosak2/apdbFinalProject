using System;
using APDB_Project.Dtos;
using APDB_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace APDB_Project.Controllers
{
    [ApiController]
    [Route("api/users/")]
    public class UserController : ControllerBase


    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        // GET
        
        public IActionResult Index()
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult RegisterUser(UserRegistrationDto registrationDto)
        {
            try
            {
                _service.RegisterUser(registrationDto);
                return Ok(); // should return token    
            }
            catch (Exception)
            {
                return BadRequest();
            }

            
        }
    }
}