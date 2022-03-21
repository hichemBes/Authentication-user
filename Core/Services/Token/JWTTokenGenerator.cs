using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Services.Token
{
	public class JWTTokenGenerator : IJWTTokenGenerator
	{
		private readonly IConfiguration _config;

		public JWTTokenGenerator(IConfiguration config)
		{
			_config = config;

		}
		public string GenerateToken(IdentityUser user, IList<string> roles, IList<Claim> claims)
		{

            claims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.GivenName, user.UserName));
            claims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, user.Email));
	
			//var claims = new List<Claim>
			// {
			//     new Claim(JwtRegisteredClaimNames.GivenName , user.UserName),
			//     new Claim(JwtRegisteredClaimNames.Email , user.Email),
			// };

			foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = creds,
                Issuer = _config["Token:Issuer"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    
		//public string GenerateToken(IdentityUser user)
		//{
		// var claims = new List<Claim>
		// {
		// 	new Claim(JwtRegisteredClaimNames.GivenName , user.UserName),
		// 	new Claim(JwtRegisteredClaimNames.Email , user.Email),
		// };

		//claims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.GivenName, user.UserName));
		//claims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, user.Email));

		//foreach (var role in roles)
		//{
		//	claims.Add(new Claim(ClaimTypes.Role, role));
		//}

		//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));

		//var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

		//var tokenDescriptor = new SecurityTokenDescriptor
		//{
		//	Subject = new ClaimsIdentity(claims),
		//	Expires = DateTime.Now.AddDays(7),
		//	SigningCredentials = creds,
		//	Issuer = _config["Token:Issuer"],
		//};

		//	var tokenHandler = new JwtSecurityTokenHandler();

		//	var token = tokenHandler.CreateToken(tokenDescriptor);

		//	return tokenHandler.WriteToken(token);
		//}


	}
}