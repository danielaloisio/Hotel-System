using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataEF;

namespace KobraSoftware.Controllers.Api
{
    public class SolicitationsController : ApiController
    {
        public HttpResponseMessage Get(string ime, int id)
        {
            try
            {
                using (var db = new DataEF.KobraEntities())
                {
                    bool result = false;

                    var clients = db.Clients.Where(p => p.Rooms.Devices.IME == ime && p.Deleted == false).FirstOrDefault();

                    var services = db.Services.Where(e => e.Deleted == false && e.Position == id).FirstOrDefault();

                    if (clients != null && services != null)
                    {
                        //salvar na tabela requests
                        Requests requests = new Requests();
                        requests.Active = true;
                        requests.Realized = false;
                        requests.CheckOut = false;
                        requests.CreatedDate = DateTime.Now;
                        requests.ClientId = clients.ClientId;
                        requests.ServiceId = services.ServiceId;
                        db.Requests.Add(requests);
                        db.SaveChanges();
                        //

                        clients.DateService = DateTime.Now;
                        clients.RoomService = true;
                        db.Entry(clients).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        result = true;
                    }

                    var ret =  new
                    {
                        error = result
                    };

                    return this.Request.CreateResponse(
                            HttpStatusCode.OK,
                            new { Solicitations = ret }
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

