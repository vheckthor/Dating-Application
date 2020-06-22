using System.Xml;
using System.Drawing;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController:ControllerBase
    {
        private readonly IDatingRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfigurations;
        private Cloudinary cloudinary;
        private ImageUploadResult uploadResult;

        public PhotosController(IDatingRepository repository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfigurations)
        {
            _cloudinaryConfigurations = cloudinaryConfigurations;
            _mapper = mapper;
            _repository = repository;

            Account account = new Account(
            _cloudinaryConfigurations.Value.CloudName,
            _cloudinaryConfigurations.Value.ApiKey,
            _cloudinaryConfigurations.Value.ApiSecret);

            cloudinary = new Cloudinary(account);

        }

        [HttpGet("{unique}",Name="GetPhoto")]
        public async Task<IActionResult> GetPhoto(Guid unique)
        {
            var photoFromRepository =await _repository.GetPhoto(unique);
            var photoSaving = _mapper.Map<PhotoReturnDTO>(photoFromRepository);
             
            return Ok(photoSaving);  

        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(Guid userId, [FromForm]PhotoForCreationDTO photocreated)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( userId.ToString() !=claim)
            {
                return Unauthorized("Access Denied");
            }

            var userFromRepo = await _repository.GetUser(userId);
            var file = photocreated.File;
            var uploadImage = new ImageUploadResult(); 
            if(file.Length>0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadValues = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name,stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                        
                    };
                    
                    uploadResult = cloudinary.Upload(uploadValues);

                }
            } 

            photocreated.Url = uploadResult.Uri.ToString();
            photocreated.PublicId = uploadResult.PublicId;
            var photoSave = _mapper.Map<Photo>(photocreated);
            photoSave.PhotoUniqueIdentifier = Guid.NewGuid();
            photoSave.UserUniqueID = userId;

            if(!userFromRepo.Photos.Any(x => x.IsMain))
            {
                photoSave.IsMain=true;
            }
            userFromRepo.Photos.Add(photoSave);



            if(await _repository.SaveAll())
            {           
                var photoToReturn = _mapper.Map<PhotoReturnDTO>(photoSave);
                return CreatedAtRoute("GetPhoto", new{unique = photoSave.PhotoUniqueIdentifier},photoToReturn);
            }
            return BadRequest("Could not add the photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto (Guid userId, Guid id)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( userId.ToString() !=claim)
            {
                return Unauthorized("Access Denied");
            }
            var user = await _repository.GetUser(userId);
            if(!user.Photos.Any(x=>x.PhotoUniqueIdentifier==id)){
                return Unauthorized(); 
            }

            
            var photoFromRepository = await _repository.GetPhoto(id);
            if(photoFromRepository.IsMain)
            {
                return BadRequest("Photo is already set as main");
            }

            var currentMainPhoto = await _repository.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;
            photoFromRepository.IsMain=true;

            if(await _repository.SaveAll())
            {
                return NoContent();
            }
            return BadRequest("Some bad happened, could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(Guid userId, Guid id)
        {
             var claim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( userId.ToString() !=claim)
            {
                return Unauthorized("Access Denied");
            }
            var user = await _repository.GetUser(userId);
            if(!user.Photos.Any(x=>x.PhotoUniqueIdentifier==id)){
                return Unauthorized(); 
            }
            var photoFromRepository = await _repository.GetPhoto(id);
            if(photoFromRepository.IsMain)
            {
                return BadRequest("Cannot delete main photo");
            }
            if(photoFromRepository.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepository.PublicId);
                var result = await cloudinary.DestroyAsync(deleteParams);
                if(result.Result == "ok")
                {
                    _repository.Delete(photoFromRepository);
                }     
            }
            if(photoFromRepository.PublicId == null)
            {
                _repository.Delete(photoFromRepository);
            }

            if( await _repository.SaveAll())
            {
                return Ok();
            }
            return BadRequest("Unable to delete photo");
        }
    }

}