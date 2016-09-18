using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataEF;

namespace KobraSoftware.Controllers
{
    [Authorize]
    public class MonitoringController : Controller
    {
        private KobraEntities db = new KobraEntities();

        //
        // GET: /Monitoring/

        public ActionResult Index()
        {
            var clients = db.Clients.Include(c => c.Genders).Include(c => c.Rooms).Where(e=> e.Deleted == false && e.Active == true).OrderBy(e => e.DateService);

            return View(clients.ToList());
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}