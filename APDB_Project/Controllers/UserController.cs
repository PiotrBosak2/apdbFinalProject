using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using APDB_Project.Dtos;
using APDB_Project.Exceptions;
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
            catch (LoginAlreadyTakenException)
            {
                return BadRequest("provided login is already used");
            }
            catch (InvalidRegistrationDataException)
            {
                return BadRequest("provided data was incorrect");
            }
            
            
        }

        [HttpGet("refresh")]
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
            catch (InvalidLoginException)
            {
                return BadRequest("you passed incorrect login");
            }
            catch (InvalidPasswordException)
            {
                return BadRequest("you passed incorrect password");
            }
            
        }
            


        [HttpGet("list")]
        [Authorize(Roles = "client")]
        public IActionResult ListCampaigns()
        {
            var campaigns = _service.ListCampaigns();
            return Ok(campaigns);
        }

        [HttpPost]
        [Authorize(Roles = "client")]
        public IActionResult CreateCampaign(CampaignCreationDto dto)
        {
            try
            {
                var campaign =  _service.CreateCampaign(dto);
                return Created("",campaign);//maybe put something here
            }
            
            catch (NoSuchBuildingException)
            {
                return BadRequest("building with given id doesn't exist");
            }
            catch (BuildingsOnDifferentStreetsException)
            {
                return BadRequest("provided buildings are on different streets");
            }
        }


      
        
    }
}