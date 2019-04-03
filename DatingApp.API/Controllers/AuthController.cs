using System;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this._config = config;
            this._repo = repo;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userforRegistorDto)
        {
            userforRegistorDto.Username = userforRegistorDto.Username.ToLower();

            if (await _repo.UserExists(userforRegistorDto.Username))
                return BadRequest("user name already exists");

            var userToCreate = new User
            {
                Name = userforRegistorDto.Username

            };

            var createdUser = await _repo.Register(userToCreate, userforRegistorDto.Password);
            return StatusCode(201);
        }

         [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var UserFromRepo = await _repo.Login(userForLoginDto.Username, userForLoginDto.Password);
            if (UserFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new  Claim(ClaimTypes.NameIdentifier,UserFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,UserFromRepo.Name)
            };

             var key = new SymmetricSecurityKey(Encoding.UTF8
             .GetBytes(_config.GetSection("AppSettings:Token").Value));
             
             var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

             var tokenDescriptor = new SecurityTokenDescriptor
             {
                    Subject=  new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds

             };

             var tokenHandler = new JwtSecurityTokenHandler();  
             var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok( new {
              token = tokenHandler.WriteToken(token)
              });

       
        }

}
}