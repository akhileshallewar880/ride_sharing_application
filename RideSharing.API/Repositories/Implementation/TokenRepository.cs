using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RideSharing.API.Repositories.Interface;

namespace RideSharing.API.Repositories.Implementation;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration configuration;
    public TokenRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public string CreateJwtToken(IdentityUser user, List<string> roles)
    {
        // Implementation for creating JWT token
        // Create Claims from user and roles
        var claimms = new List<Claim>();
            claimms.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                claimms.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                
                issuer: configuration["JwtSettings:validIssuer"],
                audience: configuration["JwtSettings:validAudience"],
                claims: claimms,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );
            // Generate Token
            return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
