using AutoMapper;
using Office4U.Domain.Model.Entities.Articles;
using Office4U.ReadApplication.Article.DTOs;
using System.Linq;

namespace Office4U.ReadApplication.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //CreateMap<AppUser, AppUserDto>();

            CreateMap<Domain.Model.Entities.Articles.Article, ArticleDto>()
                .ForMember(
                    dest => dest.PhotoUrl,
                    options => options.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url));

            CreateMap<ArticlePhoto, ArticlePhotoDto>();
        }
    }
}