using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataEF;
using KobraSoftware.Security;

namespace KobraSoftware.Controllers
{
    [Authorize]
    [PermissionFilter]
    public class RoomsController : Controller
    {
        private KobraEntities db = new KobraEntities();

        //
        // GET: /Rooms/
        public ActionResult Index()
        {
            var rooms = db.Rooms.Include(r => r.Clients).Where(e => e.Deleted == false);
            return View(rooms.ToList());
        }

        //
        // GET: /Rooms/Details/5
        public ActionResult Details(int id = 0)
        {
            Rooms rooms = db.Rooms.Where(e => e.RoomId == id).FirstOrDefault();
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        //
        // GET: /Rooms/Create
        public ActionResult Create()
        {
            ViewBag.DeviceId = new SelectList(db.Devices.Where(e => e.Deleted == false && e.Used == false), "DeviceId", "Device");
            return View();
        }

        //
        // POST: /Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rooms rooms)
        {
            if (ModelState.IsValid)
            {
                var coderoom = String.Concat(rooms.Number, rooms.Block, rooms.Floor);
                rooms.Used = false;
                rooms.CodeRoom = coderoom;
                rooms.CreatedDate = DateTime.Now;
                db.Rooms.Add(rooms);
                db.SaveChanges();

                var devices = db.Devices.Where(e => e.Deleted == false && e.DeviceId == rooms.DeviceId).FirstOrDefault();
                devices.Used = true;
                db.Entry(devices).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.DeviceId = new SelectList(db.Devices.Where(e => e.Deleted == false && e.Used == false), "DeviceId", "Device", rooms.DeviceId);
            return View(rooms);
        }

        //
        // GET: /Rooms/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Rooms rooms = db.Rooms.Where(e => e.RoomId == id).FirstOrDefault();
            if (rooms == null)
            {
                return HttpNotFound();
            }
            var deviceidold = rooms.DeviceId;
            ViewBag.deviceold = deviceidold;
            ViewBag.DeviceId = new SelectList(db.Devices.Where(e => e.Deleted == false && ((e.DeviceId != deviceidold && e.Used == false) || (e.DeviceId == deviceidold))), "DeviceId", "Device", rooms.DeviceId);
            return View(rooms);
        }

        //
        // POST: /Rooms/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rooms rooms, int id)
        {
            if (ModelState.IsValid)
            {
                var coderoom = String.Concat(rooms.Number, rooms.Block, rooms.Floor);
                rooms.CodeRoom = coderoom;
                rooms.AlterDate = DateTime.Now;
                db.Entry(rooms).State = EntityState.Modified;
                db.SaveChanges();

                if (rooms.DeviceId != id)
                {
                    var devicesold = db.Devices.Where(e => e.Deleted == false && e.DeviceId == id).FirstOrDefault();
                    devicesold.Used = false;
                    db.Entry(devicesold).State = EntityState.Modified;
                    db.SaveChanges();
                }

                var devices = db.Devices.Where(e => e.Deleted == false && e.DeviceId == rooms.DeviceId).FirstOrDefault();
                devices.Used = true;
                db.Entry(devices).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.DeviceId = new SelectList(db.Devices.Where(e => e.Deleted == false && e.Used == false), "DeviceId", "Device", rooms.DeviceId);
            return View(rooms);
        }

        //
        // GET: /Rooms/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Rooms rooms = db.Rooms.Where(e => e.RoomId == id).FirstOrDefault();
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        //
        // POST: /Rooms/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rooms rooms = db.Rooms.Where(e => e.RoomId == id).FirstOrDefault();
            rooms.Devices.Used = false;
            rooms.Deleted = true;
            rooms.DeletedDate = DateTime.Now;
            db.Entry(rooms).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}