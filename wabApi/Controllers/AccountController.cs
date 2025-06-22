using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using wabApi.DTOs.AccountDtos;
using wabApi.Models;
namespace wabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        ItiDbContext context;
        public AccountController(ItiDbContext context)
        {
            this.context = context;
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid data");

            string hashedPassword=BCrypt.Net.BCrypt.HashPassword(dto.Password);
            User u=new User
            {
                Name=dto.Name,
                Email=dto.Email,
                Password=hashedPassword,
                Age=dto.Age,
            };
            context.Users.Add(u);
            context.SaveChanges();
            return Ok("successfully register");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return Unauthorized("invalid data");
            User user=context.Users.FirstOrDefault(u=>u.Email==dto.Email);
            if (user == null)
                return Unauthorized("invalid userName Or password");
            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!isValid)
                return Unauthorized("invalid userName Or password");

            #region claims
            var UserData = new List<Claim>();
            UserData.Add(new Claim("userName", user.Email));
            UserData.Add(new Claim("name", user.Name));
            UserData.Add(new Claim("age", user.Age.ToString()));
            #endregion

            #region Credentials
            string key = "this_is_a_very_long_secret_key_456789"; 
            var scrtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCred = new SigningCredentials(scrtKey, SecurityAlgorithms.HmacSha256); 
            #endregion

            JwtSecurityToken tokenObject = new JwtSecurityToken(
                claims:UserData,
                expires:DateTime.Now.AddDays(2),
                signingCredentials:signingCred
                );
            //convert token from object to string
            var token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return Created("login successfully",token);
        }
    }
}
