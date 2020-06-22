using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{

    [ServiceFilter(typeof(LogUserActivities))]
    [Authorize]
    [Route("api/users/{userUniqueId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{UniqueId}",Name="GetMessage")]
        public async Task<IActionResult> GetMessage(Guid userUniqueId, Guid uniqueId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( userUniqueId.ToString() !=claim)
            {
                return Unauthorized("Invalid user Access");
            } 

            var messageFromRepo = await _repo.GetMessage(uniqueId);
            if (messageFromRepo == null)
            {
                return NotFound();
            }
            return Ok(messageFromRepo);
        }


        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(Guid userUniqueId, 
        [FromQuery]MessageParams messageParams)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( userUniqueId.ToString() !=claim)
            {
                return Unauthorized("Invalid user Access");
            }
            messageParams.UserId = userUniqueId;
            var messagesFromRepo = await _repo.GetMessages(messageParams);
            var messages = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messagesFromRepo);
            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize,
                messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);
            
            return Ok(messages); 
        }

        [HttpGet("thread/{recipientUniqueId}")]
        public async Task<IActionResult> GetMessageThread(Guid userUniqueId, Guid recipientUniqueId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( userUniqueId.ToString() !=claim)
            {
                return Unauthorized("Invalid user Access");
            }
            var messagesFromDb = await _repo.GetMessageThread(userUniqueId, recipientUniqueId);
            var messages = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messagesFromDb);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> PostMessages(Guid userUniqueId, MessageForCreationDto message)
        {
            var senderId = await _repo.GetUser(userUniqueId);
            var claim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( userUniqueId.ToString() !=claim)
            {
                return Unauthorized("Invalid user Access");
            } 
           message.SenderUniqueId = userUniqueId;
            var sender = await _repo.GetUser(message.SenderUniqueId);
            var recipent = await _repo.GetUser(message.RecipientUniqueId);
           if(recipent == null)
           {
               return BadRequest("Could not find User");
           }
           
 
            message.SenderId = sender.Id;
            message.RecipientId = recipent.Id;

            var messagepost = _mapper.Map<Messages>(message);
            _repo.Add(messagepost);


            if ( await _repo.SaveAll())
            {
                var returnMessage = _mapper.Map<MessageToReturnDTO>(messagepost);
                return CreatedAtRoute("GetMessage", new{UniqueId = message.UniqueId},returnMessage);
            }
            return BadRequest("Unable to send message");
        }
    }
}