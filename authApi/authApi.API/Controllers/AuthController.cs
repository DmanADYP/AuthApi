using System.Threading.Tasks;
using authApi.API.DTO;
using authApi.API.Interfaces;
using authApi.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace authApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;

        public AuthController(IAuth auth)
        {
            this._auth = auth;
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
    }
}
