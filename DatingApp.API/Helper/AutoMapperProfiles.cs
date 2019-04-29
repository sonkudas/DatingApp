using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helper
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
           //  CreateMap<User,UserForLsitDto>();
           // CreateMap<User,UserForDetailDto>();

             CreateMap<User,UserForDetailDto>()
            .ForMember(dest => dest.PhotoUrl,opt => {
               opt.MapFrom(src =>src.Photos.FirstOrDefault(p=>p.IsMain).Url);
            })
            .ForMember(dest => dest.Age , opt =>{
                opt.ResolveUsing(d=> d.DateOfBirth.CalculateAge());
            }  );
            CreateMap<User,UserForLsitDto>()
              .ForMember(dest => dest.PhotoUrl,opt => {
                opt.MapFrom(src =>src.Photos.FirstOrDefault(p=>p.IsMain).Url);
            })
             .ForMember(dest => dest.Age , opt =>{
                opt.ResolveUsing(d=> d.DateOfBirth.CalculateAge());
            }  );
            CreateMap<Photo,PhotosForDetailedDto>();
            CreateMap<UserForUpdateDto,User>();
        }
        
    }
}