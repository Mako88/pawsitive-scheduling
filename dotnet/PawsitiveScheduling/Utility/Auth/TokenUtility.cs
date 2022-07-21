using Be.Vlaanderen.Basisregisters.Generators.Guid;
using Microsoft.IdentityModel.Tokens;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility.Auth.DTO;
using PawsitiveScheduling.Utility.DI;
using PawsitiveScheduling.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility.Auth
{
    /// <summary>
    /// Utility for managing JWT tokens
    /// </summary>
    [Component]
    public class TokenUtility : ITokenUtility
    {
        private readonly IDatabaseUtility dbUtililty;
        private readonly JwtSecurityTokenHandler tokenHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenUtility(IDatabaseUtility dbUtililty)
        {
            this.dbUtililty = dbUtililty;
            tokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Create JWT tokens for the given user
        /// </summary>
        public async Task<TokenResponse> CreateTokens(User user, string ipAddress)
        {
            var key = Encoding.UTF8.GetBytes(EnvironmentVariables.JwtKey);

            var signingCreds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Name, user.Email),
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = signingCreds,
                Audience = EnvironmentVariables.JwtAudience,
                Issuer = EnvironmentVariables.JwtIssuer,
                IssuedAt = DateTime.UtcNow,
            });

            var refreshToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.Id),
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = signingCreds,
                Audience = EnvironmentVariables.JwtAudience,
                Issuer = EnvironmentVariables.JwtIssuer,
                IssuedAt = DateTime.UtcNow,
            });

            var refreshTokenString = tokenHandler.WriteToken(refreshToken);

            var ipHash = Deterministic.Create(Constants.IpAddressDeterministicNamespace, ipAddress);

            user.RefreshTokens[ipHash.ToString()] = refreshTokenString;

            await dbUtililty.UpdateEntity(user).ConfigureAwait(false);

            return new TokenResponse
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshTokenString,
            };
        }

        /// <summary>
        /// Get a user ID from a ClaimsPrincipal
        /// </summary>
        public string GetUserId(ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;

            if (!userId.HasValue())
            {
                throw new AuthenticationException("Sid claim missing from token");
            }

            return userId;
        }
    }
}
