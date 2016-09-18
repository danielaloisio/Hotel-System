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
    public class RequestsController : Controller
    {
        private KobraEntities db = new KobraEntities();

        //
        // GET: /Requests/

        public ActionResult Index(int id)
        {
            var requests = db.Requests.Include(r => r.Clients).Include(r => r.Services).Where(e => e.Deleted == false && e.ClientId == id && e.CheckOut == false).ToList();
            ViewBag.ClientId = id;

            return View(requests);
        }

        //
        // GET: /Requests/Create

        public ActionResult Create(int id)
        {
            ViewBag.ServiceId = new SelectList(db.Services.Where(e => e.Deleted == false), "ServiceId", "Name");
            ViewBag.ClientId = id;
            return View();
        }

        //
        // POST: /Requests/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Requests requests)
        {
            if (ModelState.IsValid)
            {
                
                requests.Active = true;
                requests.Realized = false;
                requests.CheckOut = false;
                requests.CreatedDate = DateTime.Now;
                requests.ClientId = requests.ClientId;
                db.Requests.Add(requests);
                db.SaveChanges();

                var clients = db.Clients.Where(e => e.Deleted == false && e.ClientId == requests.ClientId).FirstOrDefault();
                clients.DateService = DateTime.Now;
                clients.RoomService = true;
                db.Entry(clients).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { id = requests.ClientId });
            }

            ViewBag.ClientId = requests.ClientId;
            ViewBag.ServiceId = new SelectList(db.Services.Where(e => e.Deleted == false), "ServiceId", "Name", requests.ServiceId);
            return View(requests);
        }

        //
        // GET: /Requests/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Requests requests = db.Requests.Where(e => e.Deleted == false && e.RequestId == id && e.CheckOut == false).FirstOrDefault();
            if (requests == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceId = new SelectList(db.Services.Where(e => e.Deleted == false), "ServiceId", "Name", requests.ServiceId);
            return View(requests);
        }

        //
        // POST: /Requests/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Requests requests)
        {
            if (ModelState.IsValid)
            {
                requests.AlterDate = DateTime.Now;
                db.Entry(requests).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = requests.ClientId });
            }
            ViewBag.ServiceId = new SelectList(db.Services.Where(e => e.Deleted == false), "ServiceId", "Name", requests.ServiceId);
            return View(requests);
        }

        //
        // GET: /Requests/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Requests requests = db.Requests.Where(e => e.Deleted == false && e.RequestId == id && e.CheckOut == false).FirstOrDefault();
            if (requests == null)
            {
                return HttpNotFound();
            }
            return View(requests);
        }

        //
        // POST: /Requests/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Requests requests = db.Requests.Where(e => e.Deleted == false && e.RequestId == id && e.CheckOut == false).FirstOrDefault();
            requests.DeletedDate = DateTime.Now;
            requests.Deleted = true;
            db.Entry(requests).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SolicitionConclued(int id, int clientid)
        {
            var requests = db.Requests.Where(e => e.Deleted == false && e.RequestId == id && e.CheckOut == false).FirstOrDefault();
            requests.Realized = true;
            db.Entry(requests).State = EntityState.Modified;
            db.SaveChanges();

            var lsrequest = db.Requests.Where(e => e.Deleted == false && e.ClientId == clientid && e.CheckOut == false).ToList();
            var requetscount = lsrequest.Count(e => e.Realized == false);
            if (requetscount == 0)
            {
                var clients = db.Clients.Where(e => e.Deleted == false && e.ClientId == clientid && e.Active == true).FirstOrDefault();
                clients.RoomService = false;
                db.Entry(clients).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { erro = false, id = id });
            }
            return Json(new { erro = true });
        }

        public ActionResult TotalSolicitation(int id)
        {
            var requests = db.Requests.Where(e => e.Deleted == false && e.ClientId == id && e.CheckOut == false && e.Realized == true).ToList();

            var totalsolicitation = requests.Sum(e => decimal.Parse(e.Services.Price));
            ViewBag.totalsolicitation = totalsolicitation;

            var clients = db.Clients.Where(e => e.Deleted == false && e.ClientId == id && e.Active == true).FirstOrDefault();
            if (clients != null)
            {
                var clientname = clients.Name;
                ViewBag.clientname = clientname;
                ViewBag.priceroom = decimal.Parse(clients.Rooms.PriceRoom);
                ViewBag.qtddays = clients.QtdDays;
            }
            else
            {
                ViewBag.clientname = "";
            }

            ViewBag.clientid = id;

            return View(requests);
        }

        public ActionResult CheckOut(int clientid)
        {
            var requests = db.Requests.Where(e => e.Deleted == false && e.ClientId == clientid && e.CheckOut == false).ToList();

            foreach (var item in requests)
            {
                item.CheckOut = true;
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();

            var clients = db.Clients.Where(e => e.Deleted == false && e.ClientId == clientid && e.Active == true).FirstOrDefault();
            if (clients != null)
            {
                clients.Active = false;
                clients.Rooms.Used = false;
                //clients.RoomId = 0;
                
                db.Entry(clients).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { erro = false, id = clientid });
            }
            return Json(new { erro = true });

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}