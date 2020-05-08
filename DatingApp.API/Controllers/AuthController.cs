using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register (UserForRegisterDto userForRegisterDto)
        {

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if(await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists.");

                var userToCreate = new User
                {
                    Username= userForRegisterDto.Username
                };

                var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);
                
                return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login (UserForLoginDto userForLoginDto)
        {
            
             // Checking to make sure we have a user and their username & password that matches what stored in database
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if(userFromRepo == null)
                return Unauthorized();

            // Starting building up token: 2 claims: userID & userName
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            // To make sure the tokens are valid when it comes back, the server needs to sign this toke, and this part does:
            // Creating security key 
                var key = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(_config.GetSection("AppSettings:Token").Value));

            // Then using this key as part of signing credentials and enxrypting this key with hashing algorithm
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
            // Creating toke: token descpr => passing claims, expiry date (24h), signning cred
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

            // Allows to create the token
                var tokenHandler= new JwtSecurityTokenHandler();

            // Creating the token based on tokenDescriptor being passed here and we store this in a token variable
                var token =tokenHandler.CreateToken(tokenDescriptor);

            // Using token var to write the token into the response that we send back to the client
                return Ok(new 
                {
                    token = tokenHandler.WriteToken(token)
                });
        }
    }
}