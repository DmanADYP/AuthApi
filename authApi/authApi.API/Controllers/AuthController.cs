using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using authApi.API.DTO;
using authApi.API.Interfaces;
using authApi.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace authApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;

        private readonly IConfiguration _config;

        public AuthController(IAuth auth, IConfiguration config)
        {
            this._auth = auth;
            this._config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(UserRegDTO userReg)
        {
            userReg.username = userReg.username.ToLower();
            if (await this._auth.UserExists(userReg.username))
            {
                return BadRequest("Username already exists");
            }

            var userToCreate = new User
            {
                UserName = userReg.username
            };

            var createdUser = await _auth.Register(userToCreate, userReg.password);

            // return CreatedAtRoute();
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userFromRepo = await _auth.Login(userForLoginDTO.username.ToLower(), userForLoginDTO.password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescription);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }

    }
}
