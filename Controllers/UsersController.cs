using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataEF;
using System.Security.Cryptography;
using System.Text;
using KobraSoftware.Filters;
using KobraSoftware.Security;

namespace KobraSoftware.Controllers
{
    [Authorize]
    [PermissionFilter]
    public class UsersController : Controller
    {
        private KobraEntities db = new KobraEntities();

        //
        // GET: /Users/

        public ActionResult Index()
        {
            var users = db.Users.Where(e => e.Deleted == false);
            return View(users.ToList());
        }

        //
        // GET: /Users/Create

        public ActionResult Create()
        {
            ViewBag.ProfileId = new SelectList(db.Profiles.Where(e => e.Deleted == false), "ProfileId", "Name");
            return View();
        }

        //
        // POST: /Users/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users users)
        {
            var emailexist = db.Users.Where(e => e.Deleted == false && e.Email == users.Email).ToList();
            if (emailexist.Count() > 0)
            {
                ModelState.AddModelError("Email", "E-mail já cadastrado.");
                return View();
            }

            if (ModelState.IsValid)
            {
                Cryptography cryptography = new Cryptography();
                var password = cryptography.SetMD5(users.Password);

                users.Password = password;
                users.CreatedDate = DateTime.Now;
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProfileId = new SelectList(db.Profiles.Where(e => e.Deleted == false), "ProfileId", "Name", users.ProfileId);
            return View(users);
        }

        //
        // GET: /Users/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProfileId = new SelectList(db.Profiles.Where(e => e.Deleted == false), "ProfileId", "Name", users.ProfileId);
            return View(users);
        }

        //
        // POST: /Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users users)
        {
           
            if (ModelState.IsValid)
            {
                var emailexist = db.Users.Where(e => e.Deleted == false && e.Email == users.Email && e.UserId != users.UserId).ToList();
                if (emailexist.Count() > 0)
                {
                        ModelState.AddModelError("Email", "E-mail já cadastrado.");
                        return View();
                }
                users.AlterDate = DateTime.Now;
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfileId = new SelectList(db.Profiles.Where(e => e.Deleted == false), "ProfileId", "Name", users.ProfileId);
            return View(users);
        }

        //
        // GET: /Users/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Users users = db.Users.Where(e => e.UserId == id).FirstOrDefault();
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        //
        // POST: /Users/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Where(e => e.UserId == id).FirstOrDefault();
            users.Deleted = true;
            db.Entry(users).State = EntityState.Modified;
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