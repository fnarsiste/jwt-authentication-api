using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JWTAuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);
            user.Username = request.Username;
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            bool validUser = user.Username != request.Username;
            if (user.Username != request.Username)
            {
                return BadRequest("Given user not found.");
            }

            if(!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Bad user credentials.");
            }

            string token = CreateUserToken(user);
            return Ok(token);
        }

        private string CreateUserToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Country, "BJ"),
                new Claim(ClaimTypes.Email, "given@email.com"),
                new Claim(ClaimTypes.MobilePhone, "+229-123"),
            };
            string passPhrase = _configuration.GetSection("AppSettings:TokenSecret").Value;
            var key = new SymmetricSecurityKey(GetBytes(passPhrase));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(GetBytes(password));
                return computedHash.SequenceEqual(hash);
            }
        }

        private static byte[] GetBytes(string input)
        {
            return System.Text.Encoding.UTF8.GetBytes(input);
        }
    }
}
