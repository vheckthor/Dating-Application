using System.Runtime.InteropServices;
using System.Linq;
using AutoMapper;
using DatingApp.API.Models;
using DatingApp.API.Models.DTOs;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles:Profile
    {
     public AutoMapperProfiles()
    {
        CreateMap<User,UserForListDTO>()
            .ForMember(dest =>dest.PhotoUrl,
            options=>options.MapFrom(
                src=>src.Photos.FirstOrDefault(
                    x=>x.IsMain).Url))
            .ForMember(dest =>dest.Age, 
            opt=>opt.MapFrom(
                src=>src.DateofBirth.GetAge()));


        CreateMap<User,UserForDetailDTO>()
            .ForMember(dest =>dest.PhotoUrl,
                options=>options.MapFrom(
                src=>src.Photos.FirstOrDefault(
                    x=>x.IsMain).Url))
            .ForMember(dest =>dest.Age, 
                opt=>opt.MapFrom(
                    src=>src.DateofBirth.GetAge()))
            .ForMember(dest => dest.PhoneNumber,
            opt => opt.MapFrom(
                src => src.PhoneNumber.FirstOrDefault(b => b.UserIdentifier == src.UserUniqueIdentity).nationalNumber
            ));

        CreateMap<Photo, PhotoForDetailsDTO>()
            .ForMember(dest => dest.PhotoUniqueIdentifier, 
            opt => opt.MapFrom(src => src.PhotoUniqueIdentifier));
        CreateMap<UserForUpdateDTO, User>();
        CreateMap<Photo, PhotoReturnDTO>()
        .ForMember(dest => dest.UniquePhotoIdentifier,
         option=> option.MapFrom(src=>src.PhotoUniqueIdentifier));

        CreateMap<PhotoForCreationDTO, Photo>();

        CreateMap<UserForRegisterDTO, User>();
        CreateMap<PhoneNumberDetailsDTO,PhoneNumberDetails>();
        CreateMap<MessageForCreationDto, Messages>().ReverseMap();
        CreateMap<Messages, MessageToReturnDTO>()
        .ForMember(m => m.SenderPhotoUrl,opt => opt
            .MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
        .ForMember(m => m.RecipientPhotoUrl,opt => opt
            .MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
     }   
    }
}