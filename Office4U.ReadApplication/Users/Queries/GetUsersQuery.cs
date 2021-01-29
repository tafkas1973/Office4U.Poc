using AutoMapper;
using Office4U.Common;
using Office4U.ReadApplication.Users.DTOs;
using Office4U.ReadApplication.Users.Interfaces;
using Office4U.ReadApplication.Users.Interfaces.IOC;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Users.Queries
{
    public class GetUsersQuery : IGetUsersQuery
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IMapper _mapper;

        public GetUsersQuery(
            IReadOnlyUserRepository userRepository,
            IMapper mapper)
        {
            _readOnlyUserRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<AppUserDto>> Execute(UserParams userParams)
        {
            var appUsers = await _readOnlyUserRepository.GetUsersAsync(userParams);

            // TODO: find AutoMapper solution iso projection
            //var usersDtos = _mapper.Map<IEnumerable<ArticleDto>>(articles);
            var appUsersDtos = appUsers.Select(a => new AppUserDto()
            {
                Id = a.Id,
                UserName = a.UserName,
                Age = a.GetAge(),
                Created = a.Created,
                LastActive = a.LastActive
            });


            var appUsersToReturn = new PagedList<AppUserDto>(appUsersDtos, appUsers.TotalCount, appUsers.CurrentPage, appUsers.PageSize);

            return appUsersToReturn;
        }
    }
}
