using AutoMapper;
using Office4U.Domain.Model.Entities.Articles;
using Office4U.Domain.Model.Entities.Users;
using Office4U.ReadApplication.Articles.DTOs;
using Office4U.ReadApplication.Users.DTOs;
using System.Linq;

namespace Office4U.ReadApplication.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, AppUserDto>();

            CreateMap<Article, ArticleDto>()
                .ForMember(
                    dest => dest.PhotoUrl,
                    options => options.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url));

            CreateMap<ArticlePhoto, ArticlePhotoDto>();
        }
    }
}