using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Office4U.Common;
using Office4U.Domain.Model.Users.Entities;
using Office4U.Presentation.Controller.Controllers;
using Office4U.Presentation.Controller.Extensions;
using Office4U.ReadApplication.Users.DTOs;
using Office4U.ReadApplication.Users.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.Presentation.Controller.Users
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IGetUsersQuery _listQuery;
        private readonly IGetUserQuery _singleQuery;

        public UsersController(
            IGetUsersQuery listQuery,
            IGetUserQuery singleQuery
            )
        {
            _listQuery = listQuery;
            _singleQuery = singleQuery;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsersAsync(
            [FromQuery] UserParams userParams)
        {
            var users = await _listQuery.Execute(userParams);

            // users is of type PagedList<User>
            // inherits List, so it's a List<Users> plus Pagination info
            Response.AddPaginationHeader(
                users.CurrentPage,
                users.PageSize,
                users.TotalCount,
                users.TotalPages
            );

            return Ok(users.ToList());
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<AppUserDto>> GetUser(string username)
        {
            var appUserDto = await _singleQuery.Execute(username);

            return Ok(appUserDto);
        }
    }
}