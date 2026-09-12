namespace JwtAuthenticationApi.Services
{
    using global::JwtAuthenticationApi.Model;
    using JwtAuthenticationApi.Model;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

        public class JwtService
        {
            private readonly IConfiguration _configuration;

            public JwtService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public string GenerateToken(User user)
            {
                var jwtSettings = _configuration.GetSection("Jwt");

                var key = jwtSettings["Key"]
                    ?? throw new InvalidOperationException("JWT Key is not configured.");

                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];

                var expirationMinutes =
                    Convert.ToDouble(jwtSettings["ExpirationMinutes"]);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

                new Claim(ClaimTypes.Name, user.Username),

                new Claim(ClaimTypes.Role, user.Role)
            };

                var securityKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                var credentials =
                    new SigningCredentials(
                        securityKey,
                        SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    
}
