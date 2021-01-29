using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Office4U.Domain.Model.Entities.Users;
using Office4U.Presentation.Controller.Services.Interfaces;
using Office4U.WriteApplication.Users.DTOs;
using System.Threading.Tasks;

namespace Office4U.Presentation.Controller.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserToReturnDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username already exists");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserToReturnDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToReturnDto>> Login(LoginDto loginDto)
        {
            var dbUser = await _userManager.Users
                .SingleOrDefaultAsync(u => u.UserName == loginDto.Username.ToLower());

            if (dbUser == null) return Unauthorized("Invalid username");

            var result = await _signInManager.CheckPasswordSignInAsync(
                user: dbUser,
                password: loginDto.Password,
                lockoutOnFailure: false
            );

            if (!result.Succeeded) return Unauthorized();

            return new UserToReturnDto
            {
                Username = dbUser.UserName,
                Token = await _tokenService.CreateToken(dbUser)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users
                .AnyAsync(u => u.UserName == username
                .ToLower());
        }
    }
}