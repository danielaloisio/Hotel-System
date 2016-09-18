using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KobraSoftware.Controllers.Api
{
    public class ServicesController : ApiController
    {
        public HttpResponseMessage Get()
        {
            try
            {
                using (var db = new DataEF.KobraEntities())
                {
                    var services = db.Services.Where(e => e.Deleted == false && e.Active == true).ToList();

                    var ret = services.Select(e => new
                        {
                            serviceid = e.ServiceId,
                            name = e.Name,
                            description = e.Description,
                            price = e.Price,
                            image = string.Format("{0}/{1}/{2}/{3}", System.Configuration.ConfigurationManager.AppSettings["RenderImg"], "Services", e.ServiceId, e.Image)
                        }
                        ).ToList();

                    return this.Request.CreateResponse(
                            HttpStatusCode.OK,
                            new { service = ret }
                    );
                }
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(
                        HttpStatusCode.NotFound,
                        new { error = true, msg = ex.InnerException }
                );
            }
        }
    }
}

