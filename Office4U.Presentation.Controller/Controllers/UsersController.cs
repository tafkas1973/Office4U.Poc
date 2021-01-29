using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Office4U.Common;
using Office4U.Domain.Model.Entities.Users;
using Office4U.Presentation.Controller.Controllers.DTOs.AppUser;
using Office4U.Presentation.Controller.Extensions;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Office4U.Presentation.Controller.Controllers
{


    // TODO: REFACTOR toward DDD principles & remove EF reference!!!

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UsersController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsersAsync(
            [FromQuery] UserParams userParams)
        {
            var users = await _unitOfWork.UserRepository.GetUsersAsync(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<AppUserDto>>(users);

            // users is of type PagedList<User>
            // inherits List, so it's a List<Users> plus Pagination info
            Response.AddPaginationHeader(
                users.CurrentPage,
                users.PageSize,
                users.TotalCount,
                users.TotalPages
            );

            return Ok(usersToReturn);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<AppUserDto>> GetUser(string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            var userToReturn = _mapper.Map<AppUserDto>(user);

            return userToReturn;
        }
    }
}