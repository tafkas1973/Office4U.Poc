using AutoMapper;
using Office4U.ReadApplication.Users.DTOs;
using Office4U.ReadApplication.Users.Interfaces;
using Office4U.ReadApplication.Users.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Users.Queries
{
    public class GetUserQuery : IGetUserQuery
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IMapper _mapper;

        public GetUserQuery(
            IReadOnlyUserRepository userRepository,
            IMapper mapper)
        {
            _readOnlyUserRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<AppUserDto> Execute(int id)
        {
            var appUser = await _readOnlyUserRepository.GetUserByIdAsync(id);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

            return appUserDto;
        }

        public async Task<AppUserDto> Execute(string name)
        {
            var appUser = await _readOnlyUserRepository.GetUserByNameAsync(name);

            var appUserDto = _mapper.Map<AppUserDto>(appUser);

            //var appUserDto = new AppUserDto()
            //{
            //    Id = appUser.Id,
            //    Age = appUser.GetAge(),
            //    UserName = appUser.UserName,
            //    Created = appUser.Created,
            //    LastActive = appUser.LastActive
            //};

            return appUserDto;
        }
    }
}
