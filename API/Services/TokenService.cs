using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Helpers;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _environment;


    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _environment = Environment.GetEnvironmentVariable(Konstants.Env.Name);
    }


    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(Konstants.ClaimTypes.DeviceId.ToString(), user.DeviceId),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Konstants.TokenKey]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}