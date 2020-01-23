using Business.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Controllers
{
    [Route("api/token")]
    public class AuthTokenController : ApiController
    {
        IAuthTokenService _authTokenService;
        public AuthTokenController(IAuthTokenService authTokenService)
        {
            _authTokenService = authTokenService;
        }

        /// <summary>
        /// generate auth token
        /// </summary>
        /// <param name="request">
        ///  phone , password
        ///  </param>
        /// <returns> return generated token</returns>
        [HttpPost]
        [Route("api/token/generate")]
        public async Task<HttpResponseMessage> GenerateAsync(DTO.AuthTokenRequest request1)
        {
            try
            {
                var result = await Task.FromResult(_authTokenService.GenerateAuthToken(request1));
                if (!result.Errors.HasErrors)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, result.Token);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result.Errors.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// verify token
        /// </summary>
        /// <param name="request">
        /// phone , token
        /// </param>
        /// <returns> return status </returns>
        [HttpPost]
        [Route("api/token/verify")]
        public async Task<HttpResponseMessage> VerifyAsync(DTO.VerifyAuthTokenRequest request)
        {
            try
            {
                var result = await Task.FromResult(_authTokenService.VerifyAuthToken(request));
                if (!result.Errors.HasErrors)
                {
                    if (result.Result == DTO.AuthTokenResult.Expired)
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "expired");
                    else if (result.Result == DTO.AuthTokenResult.NotFound)
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "not exists");
                    else
                        return Request.CreateResponse(HttpStatusCode.Created, "valid");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result.Errors.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
