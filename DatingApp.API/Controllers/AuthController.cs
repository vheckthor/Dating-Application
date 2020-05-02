using System;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using DatingApp.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using DatingApp.API.JwtToken;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly ITokenProvider _provider;
        public AuthController(IAuthRepository repo, ITokenProvider provider)
        {
            _provider = provider;
          
            _repo = repo;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO user)
        {
            try
            {
                if (await _repo.UserExists(user.username.ToLower()))
                {
                    return BadRequest("Username already Exists");
                }
                var ouruser = new User
                {
                    Username = user.username
                };
                var result = await _repo.Register(ouruser, user.password);
                if (result == null)
                {
                    return BadRequest("An error occured saving changes");
                }
                return StatusCode(201);
            }
            catch(Exception e)
            {
                return StatusCode(500,e);
            };

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto user)
        {
            
            try
            {
                var logged = await _repo.Login(user.username, user.password);
                if (logged == null)
                {
                    return Unauthorized("You do not have access to login");
                }
                JwtSecurityTokenHandler tokenHandler;
                SecurityToken token;
                
                _provider.Tokenizer(logged, out tokenHandler, out token);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token)
                });
            }
            catch (Exception e)
            {
                return StatusCode(500,e);
            }

            
        }


    }
}