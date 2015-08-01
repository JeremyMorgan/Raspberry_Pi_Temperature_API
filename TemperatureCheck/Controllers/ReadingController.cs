using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TemperatureCheck.Models;
using TemperatureCheck.Repository;

namespace TemperatureCheck.Controllers
{
    public class ReadingController : ApiController
    {
        readonly ReadingDataMapper _ourMapper = new ReadingDataMapper();

        // GET: api/Reading
        public HttpResponseMessage Get()
        {
            // by default this will show the last temperature taken
            var ourResult = _ourMapper.ReadingGet(1, "DESC");
            // send back a 200 with a package
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, ourResult);
            return response;
        }

        // GET: api/Reading/5
        public HttpResponseMessage Get(int count, string sortorder)
        {
            // here we have some more options, you can grab a count and a sort
            var ourResult = _ourMapper.ReadingGet(count, sortorder);
            // send back a 200 with a package
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, ourResult);
            return response;
        
        }

        // POST: api/Reading
        [HttpPost]
        public HttpResponseMessage Post([FromBody]TempReading ourReading)
        {
            // here is where we insert it into the database
            ReadingDataMapper ourMapper = new ReadingDataMapper();
            ourMapper.ReadingInsert(ourReading.Temp);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
       
    }
}
