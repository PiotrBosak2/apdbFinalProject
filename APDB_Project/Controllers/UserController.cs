using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using APDB_Project.Dtos;
using APDB_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        [HttpPost("register")]
        public IActionResult RegisterUser(UserRegistrationDto registrationDto)
        {
            try
            {
                var token = _service.RegisterUser(registrationDto);
                return Ok(new 
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken = Guid.NewGuid()
                });
            }
            catch (Exception)
            {
                return BadRequest("provided data was incorrect");
            }
            
            
        }

        [HttpGet]
        public IActionResult RefreshToken()
        {
            return null;//need to implement it
        }


        [HttpPost("login")]
        public IActionResult LogIn(UserLoginDto dto)
        {
            try
            {
                var token = _service.LoginUser(dto);
                return Ok(new 
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken = Guid.NewGuid()
                });
            }
            catch (InvalidCredentialException)
            {
                return BadRequest("you passed incorrect credentials");
            }
            
        }
            

        [HttpGet("method")]
        [Authorize(Roles = "client2")]
        public IActionResult someMethod()
        {
            return Ok("very good");
        }


      
        
    }
}