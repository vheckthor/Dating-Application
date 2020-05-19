using System.Linq;
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
using AutoMapper;
using System.Collections.Generic;
using DatingApp.API.Helpers;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly ITokenProvider _provider;
        private IMapper _mapper;

        public AuthController(IAuthRepository repo, ITokenProvider provider, IMapper mapper)
        {
            _provider = provider;
          
            _repo = repo;
            _mapper = mapper;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO user)
        {
            try
            {
                var val = user.DateOfBirth.GetAge();

                if( val < 18){
                    return BadRequest("Cannot register user under 18 years.");
                }
                
                if (await _repo.UserExists(user.username.ToLower()))
                {
                    return BadRequest("Username already Exists");
                }

                var ouruser = _mapper.Map<User>(user);

                ouruser.UserUniqueIdentity = Guid.NewGuid();
                ouruser.PhoneNumber.ToList()[0].UserIdentifier = ouruser.UserUniqueIdentity;
                var result = await _repo.Register(ouruser, user.password);
                if (result == null)
                {
                    return BadRequest("An error occured saving changes");
                }
                var resultReturn = _mapper.Map<UserForDetailDTO>(result);
                return CreatedAtRoute("GetUser", new {Controller="Users", UserUniqueIdentity= result.UserUniqueIdentity}, resultReturn);
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

                var mapped = _mapper.Map<UserForListDTO>(logged);
                return Ok(new
                {
                    token = tokenHandler.WriteToken(token), mapped.PhotoUrl
                });
            }
            catch (Exception e)
            {
                return StatusCode(500,e);
            }

            
        }


    }
}