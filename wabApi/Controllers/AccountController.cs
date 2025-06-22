using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using wabApi.DTOs.AccountDtos;
using wabApi.Migrations;
using wabApi.Models;
using wabApi.UnitOfWorks;
namespace wabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        UnitOfWork unitOfWork;
        public AccountController(UnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
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
           unitOfWork.AccountRpository.addUser(u);
            return Ok("successfully register");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return Unauthorized("invalid data");
            User user=unitOfWork.AccountRpository.getUserByEmail(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return Unauthorized("invalid userName Or password");

            #region claims
            List<Claim> UserData = new List<Claim>
            {
                new Claim("userName", user.Email),
                new Claim("name", user.Name),
                new Claim("age", user.Age.ToString())
            };
            #endregion

            #region Credentials
            string key = "this_is_a_very_long_secret_key_456789"; 
            var scrtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCred = new SigningCredentials(scrtKey, SecurityAlgorithms.HmacSha256); 
            #endregion

            JwtSecurityToken tokenObject = new JwtSecurityToken(
                claims:UserData,
                expires:DateTime.Now.AddMinutes(1),
                signingCredentials:signingCred
                );
            //convert token from object to string
            var AccesstokenString = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            var refreshToken=Guid.NewGuid().ToString();
            user.RefreshToken= refreshToken;
            user.RefreshTokenExpiry= DateTime.Now.AddDays(15);
            unitOfWork.AccountRpository.save();

            return Created("login successfully", new
            {
                token=AccesstokenString,
                refreshToken=refreshToken
            });
        }

        [HttpPost("refreshToken")]
        public IActionResult RefreshToken(RefreshTokenDto dto)
        {
            var user = unitOfWork.AccountRpository.getUserByRefreshToken(dto.RefreshToken);
          
            if (user == null)
                return Unauthorized("Invalid or expired refresh token");
            #region claims
            List<Claim> UserData = new List<Claim>
            {
                new Claim("userName", user.Email),
                new Claim("name", user.Name),
                new Claim("age", user.Age.ToString())
            };
            #endregion

            #region Credentials
            string key = "this_is_a_very_long_secret_key_456789";
            var scrtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCred = new SigningCredentials(scrtKey, SecurityAlgorithms.HmacSha256);
            #endregion

            JwtSecurityToken tokenObject = new JwtSecurityToken(
                claims: UserData,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: signingCred
                );
            //convert token from object to string
            var AccesstokenString = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            user.RefreshToken = Guid.NewGuid().ToString();
            unitOfWork.AccountRpository.save();

            return Ok( new
            {
                token = AccesstokenString,
                refreshToken = user.RefreshToken
            });
        }
    }
}
