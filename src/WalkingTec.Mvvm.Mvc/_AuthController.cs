//
// AuthController.cs
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
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc.Auth;

namespace WalkingTec.Mvvm.Mvc
{
    [ActionDescription("刷新token")]
    [ApiController]
    [Route("api/_[controller]")]
    [WTMAuthorize]
    [AllRights]
    public class AuthController : BaseApiController
    {
        [HttpPost("RefreshToken")]
        [ProducesResponseType(typeof(Token), StatusCodes.Status200OK)]
        public async Task<Token> RefreshToken(string refreshToken)
        {
            var _tokenRefreshService = HttpContext.RequestServices.GetRequiredService<ITokenRefreshService>();
            return await _tokenRefreshService.RefreshTokenAsync(refreshToken);
        }

    }
}
