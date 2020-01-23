using Business.Contacts;
using Business.Contacts.Repositories;
using Business.Helpers;
using Business.Services.Contracts;
using DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AuthTokenService : IAuthTokenService
    {
        IAuthTokenRepository _authTokenRepository;
        IPersonsRepository _personsRepository;

        public AuthTokenService(IAuthTokenRepository authTokenRepository, IPersonsRepository personsRepository)
        {
            this._authTokenRepository = authTokenRepository;
            this._personsRepository = personsRepository;
        }

        /// <summary>
        /// Generate auth token for the user 
        /// use Password to encrypt the new generated token
        /// </summary>
        /// <param name="authTokenRequest">phone , password</param>
        /// <returns> return AuthTokenResponse(validation errors + token string)</returns>
        public AuthTokenResponse GenerateAuthToken(AuthTokenRequest authTokenRequest)
        {
            var result = new AuthTokenResponse();

            var error = ValidateRequest(authTokenRequest);
            if (error.HasErrors)
            {
                result.Errors = error;
                return result;
            }

            var createdAt = DateTime.UtcNow;

            var authTokenRow = new DTO.AuthToken();
            authTokenRow.Password = authTokenRequest.Password;
            authTokenRow.Phone = authTokenRequest.Phone;
            authTokenRow.ExpireAt = createdAt.AddHours(1);
            authTokenRow.Token = GenerateToken(authTokenRequest.Password, createdAt);

            _authTokenRepository.Add(authTokenRow);

            result.Token = authTokenRow.Token;
            return result;
        }

        /// <summary>
        /// verify that phone and token are exists in databse 
        /// + verify token status(valid, expired or not found)
        /// </summary>
        /// <param name="request"></param>
        /// <returns> return VerifyAuthTokenResponse(validation errors + status as string)</returns>
        public VerifyAuthTokenResponse VerifyAuthToken(VerifyAuthTokenRequest request)
        {
            var result = new VerifyAuthTokenResponse();

            var error = ValidateRequest(request);
            if (error.HasErrors)
            {
                result.Errors = error;
                return result;
            }

            var authTokenRow = _authTokenRepository.Get(request.Phone, request.Token);
            if (authTokenRow == null)// not found..
                result.Result = AuthTokenResult.NotFound;
            else
            {
                if (authTokenRow.ExpireAt < DateTime.UtcNow)// expired tooken
                    result.Result = AuthTokenResult.Expired;
                else
                    // handle valid token
                    result.Result = AuthTokenResult.Valid;
            }
            return result;
        }

        Error ValidateRequest(object request)
        {
            Error error = new Error() { HasErrors = false };

            var context = new ValidationContext(request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(request, context, validationResults, true);

            if (request is AuthTokenRequest)
                CheckPhoneIfExists((request as AuthTokenRequest).Phone, ref validationResults);
            else if (request is VerifyAuthTokenRequest)
                CheckPhoneIfExists((request as VerifyAuthTokenRequest).Phone, ref validationResults);


            if (validationResults.Count > 0)
            {
                error.HasErrors = true;
                error.ErrorMessage = JsonConvert.SerializeObject(validationResults);
            }

            return error;
        }

        void CheckPhoneIfExists(string phone, ref List<ValidationResult> validationResult)
        {
            if (!_personsRepository.IsPhoneExists(phone))
                validationResult.Add(new ValidationResult("not exists", new string[] { "Phone" }));
        }

        string GenerateToken(string password, DateTime createdAt)
        {
            byte[] time = BitConverter.GetBytes(createdAt.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());

            string encryptedToken = StringCipher.Encrypt(token, password);

            return encryptedToken;
        }
    }
}
