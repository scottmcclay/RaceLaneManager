using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rlm.Web.WebApi
{
    public class AuthController : ApiController
    {
        public HttpResponseMessage PostAuthenticate([FromBody] dynamic json)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            if (json != null)
            {
                string name = json.name;
                string password = json.password;

                if ((name == "admin") && (password == "pv675"))
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
            }

            return response;
        }
    }
}
