//
// AuthService.cs
//
// Author:
//       Michael,Vito
//
// Copyright (c) 2019 WTM
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.ConfigOptions;
using WalkingTec.Mvvm.Core.Models;

namespace WalkingTec.Mvvm.Mvc.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ILogger _logger;
        private readonly JwtOptions _jwtOptions;
        private readonly Configs _configs;
        private readonly IDataContext _dc;
        public IDataContext DC => _dc;

        private const Token _emptyToken = null;

        public AuthService(
            ILogger<AuthService> logger
        )
        {
            _configs = GlobalServices.GetRequiredService<Configs>();
            _jwtOptions = _configs.JwtOptions;
            _logger = logger;
            _dc = CreateDC();
        }

        public async Task<Token> IssueToken(LoginUserInfo loginUserInfo)
        {
            if (loginUserInfo == null)
                throw new ArgumentNullException(nameof(loginUserInfo));

            var signinCredentials = new SigningCredentials(_jwtOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: new List<Claim>()
                {
                    new Claim("id",loginUserInfo.Id.ToString()),
                    new Claim("itcode",loginUserInfo.ITCode),
                    new Claim("name",loginUserInfo.Name)
                },
                expires: DateTime.Now.AddSeconds(_jwtOptions.Expires),
                signingCredentials: signinCredentials
            );

            if (Guid.Empty == loginUserInfo.Id)
            {
                return await Task.FromResult(_emptyToken);
            }
            else
            {
                var refreshToken = new PersistedGrant()
                {
                    UserId = loginUserInfo.Id,
                    Type = "refresh_token",
                    CreationTime = DateTime.Now,
                    RefreshToken = Guid.NewGuid().ToString().Replace("-", string.Empty),
                    Expiration = DateTime.Now.AddSeconds(_jwtOptions.RefreshTokenExpires)
                };
                _dc.AddEntity(refreshToken);
                await _dc.SaveChangesAsync();

                return await Task.FromResult(new Token()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(tokeOptions),
                    ExpiresIn = _jwtOptions.Expires,
                    RefreshToken = refreshToken.RefreshToken,
                    TokenType = "Bearer",
                });
            }
        }

        private IDataContext CreateDC()
        {
            string cs = "default";
            var globalIngo = GlobalServices.GetRequiredService<GlobalData>();
            return (IDataContext)globalIngo.DataContextCI?.Invoke(new object[] { _configs.ConnectionStrings?.Where(x => x.Key.ToLower() == cs).Select(x => x.Value).FirstOrDefault(), _configs.DbType });
        }
    }
}
