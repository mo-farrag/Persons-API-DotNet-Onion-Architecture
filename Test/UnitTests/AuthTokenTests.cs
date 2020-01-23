using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using API.Controllers;
using Business.Contacts;
using Business.Contacts.Repositories;
using Business.Services;
using Business.Services.Contracts;
using DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class AuthTokenTests
    {
        private static readonly JsonMediaTypeFormatter JsonFormatter = new JsonMediaTypeFormatter();
        Mock<IPersonsRepository> _personsRepositoryMock;
        Mock<IAuthTokenRepository> _authTokenRepositoryMock;
        Mock<IAuthTokenService> _authTokenServiceMock;
        AuthTokenController _authTokenAPI;

        public AuthTokenTests()
        {
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _authTokenRepositoryMock = new Mock<IAuthTokenRepository>();
        }

        [TestMethod]
        public async Task AuthToken_Generate_Success()
        {
            //arrange    
            _authTokenAPI = new AuthTokenController(new AuthTokenService(_authTokenRepositoryMock.Object, _personsRepositoryMock.Object))
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var authTokenRequest = new AuthTokenRequest()
            {
                Phone = "123456789123",
                Password = "asd123"
            };

            _personsRepositoryMock.Setup(x => x.IsPhoneExists(It.IsAny<string>())).Returns(true);
            _authTokenRepositoryMock.Setup(x => x.Add(It.IsAny<AuthToken>())).Returns(It.IsAny<int>());

            //act
            var response = await _authTokenAPI.GenerateAsync(authTokenRequest);

            //assert
            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task AuthToken_Generate_Failed_missing_phone_and_Password()
        {
            //arrange
            _authTokenAPI = new AuthTokenController(new AuthTokenService(_authTokenRepositoryMock.Object, _personsRepositoryMock.Object))
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var authTokenRequest = new AuthTokenRequest()
            {
                Phone = "",
                Password = ""
            };

            _personsRepositoryMock.Setup(x => x.IsPhoneExists(It.IsAny<string>())).Returns(true);
            _authTokenRepositoryMock.Setup(x => x.Add(It.IsAny<AuthToken>())).Returns(It.IsAny<int>());

            //act
            var response = await _authTokenAPI.GenerateAsync(authTokenRequest);
            var responseBody = await Task.FromResult(response.Content.ReadAsStringAsync()).Result;
            //assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsTrue(responseBody.Contains("{\\\"MemberNames\\\":[\\\"Password\\\"],\\\"ErrorMessage\\\":\\\"blank\\\"}"));
            Assert.IsTrue(responseBody.Contains("{\\\"MemberNames\\\":[\\\"Phone\\\"],\\\"ErrorMessage\\\":\\\"blank\\\"}"));
        }

        [TestMethod]
        public async Task AuthToken_Verify_Success()
        {
            //arrange
            _authTokenAPI = new AuthTokenController(new AuthTokenService(_authTokenRepositoryMock.Object, _personsRepositoryMock.Object))
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var authTokenRequest = new VerifyAuthTokenRequest()
            {
                Phone = "123456789123",
                Token = "GNSaYTxnLWWlyeyOL9RCUiYI4MRm++18mTkIwTMHIpwfZSygAAUovW/fkVV+saGe0+XfMKzEKjtpdluNSjUlZ6TUQllSvh32zNHZRCbyBpglvcJwJKJqjH+wV95lpvPpYXj+SotD8vflPzAVD7x0q3NxxK50TJhUOYQm6VblWR4="
            };

            var authTokenRow = new DTO.AuthToken()
            {
                Phone = "123456789123",
                Token = "GNSaYTxnLWWlyeyOL9RCUiYI4MRm++18mTkIwTMHIpwfZSygAAUovW/fkVV+saGe0+XfMKzEKjtpdluNSjUlZ6TUQllSvh32zNHZRCbyBpglvcJwJKJqjH+wV95lpvPpYXj+SotD8vflPzAVD7x0q3NxxK50TJhUOYQm6VblWR4=",
                ExpireAt = DateTime.UtcNow.AddMinutes(10)
            };

            _personsRepositoryMock.Setup(x => x.IsPhoneExists(It.IsAny<string>())).Returns(true);
            _authTokenRepositoryMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(authTokenRow);

            //act
            var response = await _authTokenAPI.VerifyAsync(authTokenRequest);
            var responseBody = await Task.FromResult(response.Content.ReadAsStringAsync()).Result;

            //assert
            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(responseBody, "\"valid\"");
        }

        [TestMethod]
        public async Task AuthToken_Verify_Failed_missing_phone_and_Token()
        {
            //arrange
            _authTokenAPI = new AuthTokenController(new AuthTokenService(_authTokenRepositoryMock.Object, _personsRepositoryMock.Object))
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var authTokenRequest = new VerifyAuthTokenRequest()
            {
                Phone = "",
                Token = ""
            };

            //act
            var response = await _authTokenAPI.VerifyAsync(authTokenRequest);
            var responseBody = await Task.FromResult(response.Content.ReadAsStringAsync()).Result;
            //assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsTrue(responseBody.Contains("{\\\"MemberNames\\\":[\\\"Phone\\\"],\\\"ErrorMessage\\\":\\\"blank\\\"}"));
            Assert.IsTrue(responseBody.Contains("{\\\"MemberNames\\\":[\\\"Token\\\"],\\\"ErrorMessage\\\":\\\"blank\\\"}"));
        }

        [TestMethod]
        public async Task AuthToken_Verify_Failed_Token_Expired()
        {
            //arrange
            _authTokenAPI = new AuthTokenController(new AuthTokenService(_authTokenRepositoryMock.Object, _personsRepositoryMock.Object))
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var authTokenRequest = new VerifyAuthTokenRequest()
            {
                Phone = "123456789123",
                Token = "GNSaYTxnLWWlyeyOL9RCUiYI4MRm++18mTkIwTMHIpwfZSygAAUovW/fkVV+saGe0+XfMKzEKjtpdluNSjUlZ6TUQllSvh32zNHZRCbyBpglvcJwJKJqjH+wV95lpvPpYXj+SotD8vflPzAVD7x0q3NxxK50TJhUOYQm6VblWR4="
            };

            var authTokenRow = new DTO.AuthToken()
            {
                Phone = "123456789123",
                Token = "GNSaYTxnLWWlyeyOL9RCUiYI4MRm++18mTkIwTMHIpwfZSygAAUovW/fkVV+saGe0+XfMKzEKjtpdluNSjUlZ6TUQllSvh32zNHZRCbyBpglvcJwJKJqjH+wV95lpvPpYXj+SotD8vflPzAVD7x0q3NxxK50TJhUOYQm6VblWR4=",
                ExpireAt = DateTime.UtcNow.AddHours(-10)
            };

            _personsRepositoryMock.Setup(x => x.IsPhoneExists(It.IsAny<string>())).Returns(true);
            _authTokenRepositoryMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(authTokenRow);

            //act
            var response = await _authTokenAPI.VerifyAsync(authTokenRequest);
            var responseBody = await Task.FromResult(response.Content.ReadAsStringAsync()).Result;
            //assert
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.AreEqual(responseBody, "\"expired\"");
        }

        [TestMethod]
        public async Task AuthToken_Verify_Failed_Not_Exists()
        {
            //arrange
            _authTokenAPI = new AuthTokenController(new AuthTokenService(_authTokenRepositoryMock.Object, _personsRepositoryMock.Object))
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var authTokenRequest = new VerifyAuthTokenRequest()
            {
                Phone = "123456789123",
                Token = "GNSaYTxnLWWlyeyOL9RCUiYI4MRm++18mTkIwTMHIpwfZSygAAUovW/fkVV+saGe0+XfMKzEKjtpdluNSjUlZ6TUQllSvh32zNHZRCbyBpglvcJwJKJqjH+wV95lpvPpYXj+SotD8vflPzAVD7x0q3NxxK50TJhUOYQm6VblWR4="
            };

            _personsRepositoryMock.Setup(x => x.IsPhoneExists(It.IsAny<string>())).Returns(true);
            _authTokenRepositoryMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns<AuthToken>(null);

            //act
            var response = await _authTokenAPI.VerifyAsync(authTokenRequest);
            var responseBody = await Task.FromResult(response.Content.ReadAsStringAsync()).Result;
            //assert
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.AreEqual(responseBody, "\"not exists\"");
        }
    }
}
