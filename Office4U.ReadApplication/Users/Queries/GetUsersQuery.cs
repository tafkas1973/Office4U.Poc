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

        public GetUsersQuery(IReadOnlyUserRepository userRepository)
        {
            _readOnlyUserRepository = userRepository;
        }

        public async Task<PagedList<AppUserDto>> Execute(UserParams userParams)
        {
            var appUsers = await _readOnlyUserRepository.GetUsersAsync(userParams);

            // TODO: inject correct project AutoMapper
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>()));
            var appUsersDtos = mapper.Map<IEnumerable<AppUserDto>>(appUsers);

            var appUsersToReturn = new PagedList<AppUserDto>(appUsersDtos, appUsers.TotalCount, appUsers.CurrentPage, appUsers.PageSize);

            return appUsersToReturn;
        }
    }
}
