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
    public class PersonsController : ApiController
    {
        IPersonsService _personsService;
        public PersonsController(IPersonsService personsService)
        {
            _personsService = personsService;
        }
      
        // POST api/items
        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(DTO.PersonRequest person)
        {
            try
            {
                var result = await Task.FromResult(_personsService.Add(person));
                if (!result.Errors.HasErrors)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, result.Result);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result.Errors.ErrorMessage);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

            }

        }
    }
}
