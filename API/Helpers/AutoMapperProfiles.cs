using API.DTOs;
using API.Extensions;
using AutoMapper;
using Tinder_lvl10.Entities;

namespace API.Helpers
{
    public class AutoMapperProfiles:Profile
    {

        public AutoMapperProfiles() {

            CreateMap<AppUser, MemberDTO>().ForMember(dest => dest.PhotoURL, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url)).ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo,PhotoDTO>();

            CreateMap<MemberUpdateDto, AppUser>();

            CreateMap<RegisterDTO, AppUser>();

        }


    }
}
