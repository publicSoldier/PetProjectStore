using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PetProjectStore.Api.Dtos;
using PetProjectStore.Api.Exceptions;
using PetProjectStore.Api.Interfaces;
using PetProjectStore.DAL.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PetProjectStore.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task RegistrationAsync(RegistrationDto registrationDto)
        {
            var user = _mapper.Map<User>(registrationDto);

            var createResult = await _userManager.CreateAsync(user, registrationDto.Password);

            var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRole.User.ToString());

            if (!createResult.Succeeded)
                throw new RegistrationException(createResult.Errors);

            if (!addToRoleResult.Succeeded)
                throw new RegistrationException(addToRoleResult.Errors);           
        }

        public async Task<LogInResultDto> LogInAsync(LogInDto logInDto)
        {
            var user = await _userManager.FindByEmailAsync(logInDto.Email);

            if (user is null)
                throw new LogInException("Неверный логин или пароль");

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, logInDto.Password, false);

            if (!signInResult.Succeeded)
                throw new LogInException("Неверный логин или пароль");

            var role = await _userManager.GetRolesAsync(user);
            var userRole = (UserRole)Enum.Parse(typeof(UserRole), role.First());

            LogInResultDto logInResult = new LogInResultDto
            {
                Id = user.Id,
                Email = user.Email,
                UserRole = userRole
            };

            return logInResult;
        }
    }
}
