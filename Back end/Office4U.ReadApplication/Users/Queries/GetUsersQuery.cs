using AutoMapper;
using Office4U.Common;
using Office4U.ReadApplication.Helpers;
using Office4U.ReadApplication.Users.DTOs;
using Office4U.ReadApplication.Users.Interfaces;
using Office4U.ReadApplication.Users.Interfaces.IOC;
using System.Collections.Generic;
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

            var appUsersDtos = _mapper.Map<IEnumerable<AppUserDto>>(appUsers);

            var appUsersToReturn = new PagedList<AppUserDto>(appUsersDtos, appUsers.TotalCount, appUsers.CurrentPage, appUsers.PageSize);

            return appUsersToReturn;
        }
    }
}
