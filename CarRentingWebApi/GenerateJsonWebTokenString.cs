using BusinessObjects.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI;

public static class GenerateJsonWebTokenString
{
    public static string GenerateJsonWebToken(this Account user, string secretKey, DateTime now)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        // Create new token
        var token = new JwtSecurityToken(
            claims: claims, // user identity claims
            expires: now.AddHours(1), // expire time
            signingCredentials: credentials);

        // Write token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
