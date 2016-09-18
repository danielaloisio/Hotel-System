using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataEF;
using System.Web.Security;
using KobraSoftware.Security;
using KobraSoftware.Filters;

namespace KobraSoftware.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private KobraEntities db = new KobraEntities();

        //
        // GET: /Clients/

        public ActionResult Index()
        {
            var userlogged = UserRepository.GetUserLogged();
            ViewBag.UserLoggedProfile = userlogged.ProfileId;

            var clients = db.Clients.Include(c => c.Rooms).Include(c => c.Genders).Where(e => e.Deleted == false);
            return View(clients.ToList());
        }


        //
        // GET: /Clients/Create

        public ActionResult Create()
        {
            ViewBag.RoomId = new SelectList(db.Rooms.Where(e => e.Deleted == false && e.Used == false), "RoomId", "CodeRoom");
            ViewBag.GenderId = new SelectList(db.Genders.Where(e => e.Deleted == false), "GenderId", "Gender");
            return View();
        }

        //
        // POST: /Clients/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Clients clients)
        {
            if (ModelState.IsValid)
            {
                clients.CreatedDate = DateTime.Now;
                db.Clients.Add(clients);
                db.SaveChanges();

                var rooms = db.Rooms.Where(e => e.Deleted == false && e.RoomId == clients.RoomId).FirstOrDefault();
                rooms.Used = true;
                db.Entry(rooms).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.RoomId = new SelectList(db.Rooms.Where(e => e.Deleted == false && e.Used == false), "RoomId", "CodeRoom", clients.RoomId);
            ViewBag.GenderId = new SelectList(db.Genders.Where(e => e.Deleted == false), "GenderId", "Gender", clients.GenderId);
            return View(clients);
        }

        //
        // GET: /Clients/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Clients clients = db.Clients.Where(e => e.ClientId == id).FirstOrDefault();
            if (clients == null)
            {
                return HttpNotFound();
            }
            var roomidold = clients.RoomId;
            ViewBag.roomidold = roomidold;
            ViewBag.RoomId = new SelectList(db.Rooms.Where(e => e.Deleted == false && ((e.RoomId != roomidold && e.Used == false) || (e.RoomId == roomidold))), "RoomId", "CodeRoom", clients.RoomId);
            ViewBag.GenderId = new SelectList(db.Genders.Where(e => e.Deleted == false), "GenderId", "Gender", clients.GenderId);
            return View(clients);
        }

        //
        // POST: /Clients/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Clients clients, int id)
        {
            if (ModelState.IsValid)
            {
                clients.AlterDate = DateTime.Now;
                db.Entry(clients).State = EntityState.Modified;
                db.SaveChanges();

                if (clients.RoomId != id)
                {
                    var roomsold = db.Rooms.Where(e => e.Deleted == false && e.RoomId == id).FirstOrDefault();
                    roomsold.Used = false;
                    db.Entry(roomsold).State = EntityState.Modified;
                    db.SaveChanges();
                }

                var rooms = db.Rooms.Where(e => e.Deleted == false && e.RoomId == clients.RoomId).FirstOrDefault();
                rooms.Used = true;
                db.Entry(rooms).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.RoomId = new SelectList(db.Rooms.Where(e => e.Deleted == false && e.Used == false), "RoomId", "CodeRoom", clients.RoomId);
            ViewBag.GenderId = new SelectList(db.Genders.Where(e => e.Deleted == false), "GenderId", "Gender", clients.GenderId);
            return View(clients);
        }

        //
        // GET: /Clients/Delete/5
        [PermissionFilter]
        public ActionResult Delete(int id = 0)
        {
            Clients clients = db.Clients.Where(e => e.ClientId == id).FirstOrDefault();
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

        //
        // POST: /Clients/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clients clients = db.Clients.Where(e => e.ClientId == id).FirstOrDefault();
            clients.Deleted = true;
            clients.Rooms.Used = false;
            clients.DeletedDate = DateTime.Now;
            db.Entry(clients).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //get price room
        public ActionResult GetPriceRoom(int roomid)
        {
            var rooms = db.Rooms.Where(e => e.Deleted == false && e.RoomId == roomid).FirstOrDefault();
            string price = "";

            if (rooms != null)
            {
                price = rooms.PriceRoom;
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { erro = false, price = price });
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