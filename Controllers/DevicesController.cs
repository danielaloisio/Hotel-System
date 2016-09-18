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
    public class DevicesController : Controller
    {
        private KobraEntities db = new KobraEntities();

        //
        // GET: /Devices/

        public ActionResult Index()
        {
            var devices = db.Devices.Where(e => e.Deleted == false);

            return View(devices.ToList());
        }

        //
        // GET: /Devices/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Devices/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Devices devices)
        {
            if (ModelState.IsValid)
            {
                devices.Used = false;
                devices.CreatedDate = DateTime.Now;
                db.Devices.Add(devices);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(devices);
        }

        //
        // GET: /Devices/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Devices devices = db.Devices.Where(e => e.DeviceId == id).FirstOrDefault();
            if (devices == null)
            {
                return HttpNotFound();
            }
            return View(devices);
        }

        //
        // POST: /Devices/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Devices devices)
        {
            if (ModelState.IsValid)
            {
                devices.AlterDate = DateTime.Now;
                db.Entry(devices).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(devices);
        }

        //
        // GET: /Devices/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Devices devices = db.Devices.Where(e => e.DeviceId == id).FirstOrDefault();
            if (devices == null)
            {
                return HttpNotFound();
            }
            return View(devices);
        }

        //
        // POST: /Devices/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Devices devices = db.Devices.Where(e => e.DeviceId == id).FirstOrDefault();
            devices.Deleted = true;
            devices.DeletedDate = DateTime.Now;
            db.Entry(devices).State = EntityState.Modified;
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