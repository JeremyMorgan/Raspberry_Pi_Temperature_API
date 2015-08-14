using System.Net;
using System.Net.Http;
using System.Web.Http;
using TemperatureCheck.Models;
using TemperatureCheck.Repository;

namespace TemperatureCheck.Controllers
{
    public class StatusController : ApiController
    {
        readonly StatusDataMapper _ourMapper = new StatusDataMapper();

        // GET: api/Status
        public HttpResponseMessage Get()
        {
            // by default this will show the last temperature taken
            var ourResult = _ourMapper.StatusGet(1, "DESC");
            // send back a 200 with a package
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, ourResult);
            return response;
        }

        // GET: api/Status/5
        public HttpResponseMessage Get(int count, string sortorder)
        {
            // here we have some more options, you can grab a count and a sort
            var ourResult = _ourMapper.StatusGet(count, sortorder);
            // send back a 200 with a package
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, ourResult);
            return response;
        }

        // POST: api/Status
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Status ourStatus)
        {
            // here is where we insert it into the database
            StatusDataMapper ourMapper = new StatusDataMapper();
            ourMapper.StatusInsert(ourStatus.TempFahrenheit, ourStatus.TempCelcius, ourStatus.Humidity);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
