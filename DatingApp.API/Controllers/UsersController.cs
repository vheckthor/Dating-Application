using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivities))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams param)
        {
            var loggedIn = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(loggedIn);
            param.UserId = loggedIn;
            
            if(string.IsNullOrEmpty(param.Gender)){
               param.Gender = userFromRepo.Gender == "male"? "female": "male"; 
            }


            var users = await _repo.GetUsers(param);
            var usersReturner = _mapper.Map<IEnumerable<UserForListDTO>>(users);

            Response.AddPagination(users.CurrentPage, 
            users.PageSize, users.TotalCount, users.TotalPages);
            
            return Ok(usersReturner);
        }

        [HttpGet("{UserUniqueIdentity}", Name="GetUser")]
        public async Task<IActionResult> GetUser(Guid  UserUniqueIdentity)
        {
            var user = await _repo.GetUser(UserUniqueIdentity);
            var userReturn = _mapper.Map<UserForDetailDTO>(user);
            return Ok(userReturn);
        }

        [HttpPut("{UserUniqueIdentity}")]
        public async Task<IActionResult> UpdateUser(Guid UserUniqueIdentity, UserForUpdateDTO updateUser)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( UserUniqueIdentity.ToString() !=claim)
            {
                return Unauthorized("Invalid user Access");
            }

            var userFromRepo = await _repo.GetUser(UserUniqueIdentity);
            _mapper.Map(updateUser,userFromRepo);
            if(await _repo.SaveAll())
                return NoContent();
            return StatusCode(403);
        }

        [HttpPost("{UserUniqueIdentity}/like/{recipientUniqueId}")]
        public async Task<IActionResult>LikeUser(Guid UserUniqueIdentity, Guid recipientUniqueId)
        {
             var claim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( UserUniqueIdentity.ToString() !=claim)
            {
                return Unauthorized("Invalid user Access");
            }
            var userLike = await _repo.GetUser(UserUniqueIdentity);

            var recipientLike = await _repo.GetUser(recipientUniqueId);
            if (recipientLike == null)
            {
                return NotFound("User not Found");
            }
            var like = await _repo.GetLike(userLike.Id,recipientLike.Id);

            if (like != null)
            {
                return BadRequest("You already liked this user");
            }

            like = new Like
            {
                LikerId = userLike.Id,
                LikerUniqueId = userLike.UserUniqueIdentity,
                LikeeId = recipientLike.Id,
                LikeeUniqueId = recipientUniqueId

            };

            _repo.Add<Like>(like);
            if( await _repo.SaveAll())
            {
                return Ok();
            }
            return BadRequest("Failed to like user");
        }

         [HttpPost("{UserUniqueIdentity}/unlike/{recipientUniqueId}")]
        public async Task<IActionResult>UnLikeUser(Guid UserUniqueIdentity, Guid recipientUniqueId)
        {
             var claim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( UserUniqueIdentity.ToString() !=claim)
            {
                return Unauthorized("Invalid user Access");
            }
            var userLike = await _repo.GetUser(UserUniqueIdentity);

            var recipientLike = await _repo.GetUser(recipientUniqueId);
            if (recipientLike == null)
            {
                return NotFound("User not Found");
            }
            var like = await _repo.GetLike(userLike.Id,recipientLike.Id);

            if (like == null)
            {
                return BadRequest("You have not liked this user");
            }

            _repo.Delete<Like>(like);

            if( await _repo.SaveAll())
            {
                return Ok();
            }
            return BadRequest("Failed to like user");
        }
    
    }
}