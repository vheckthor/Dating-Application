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
                    src=>src.DateofBirth.GetAge()))    ;

        CreateMap<Photo, PhotoForDetailsDTO>();
     }   
    }
}