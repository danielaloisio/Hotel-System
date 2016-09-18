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
    public class ServicesController : Controller
    {
        private KobraEntities db = new KobraEntities();

        //
        // GET: /Services/

        public ActionResult Index()
        {
            var services = db.Services.Where(e => e.Deleted == false);
            return View(services.ToList());
        }

        //
        // GET: /Services/Details/5

        public ActionResult Details(int id = 0)
        {
            Services services = db.Services.Where(e => e.Deleted == false && e.ServiceId == id).FirstOrDefault();
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        //
        // GET: /Services/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Services/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Services services)
        {
            if (ModelState.IsValid)
            {
                var guid = Guid.NewGuid();
                var fileName = string.Format("{0}.{1}", guid.ToString(), Request.Files[0].FileName.Split('.').Last());

                var servicesposition = db.Services.Where(e => e.Deleted == false).ToList();
                var position = servicesposition.Count(e => e.Position > 0);

                services.Position = position + 1;
                services.Image = fileName;
                services.CreatedDate = DateTime.Now;
                db.Services.Add(services);
                db.SaveChanges();

                string ImgLogo = "";
                if (Request.Files.Count > 0)
                {
                    ImgLogo = fileName;

                    if (!string.IsNullOrEmpty(ImgLogo))
                    {
                        string caminho = System.Configuration.ConfigurationManager.AppSettings["UploadSave"].ToString();
                        System.IO.Directory.CreateDirectory(string.Format("{0}\\{1}\\{2}", caminho, "Services", services.ServiceId));
                        Request.Files[0].SaveAs(string.Format("{0}\\{1}\\{2}\\{3}", caminho, "Services", services.ServiceId, ImgLogo));
                    }
                }
                return RedirectToAction("Index");
            }

            return View(services);
        }

        //
        // GET: /Services/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Services services = db.Services.Where(e => e.Deleted == false && e.ServiceId == id).FirstOrDefault();
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        //
        // POST: /Services/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Services services)
        {
            if (ModelState.IsValid)
            {
                string ImgLogo = services.Image;
                string img = Request.Files[0].FileName;
                if (Request.Files.Count > 0)
                {
                    var guid = Guid.NewGuid();
                    var fileName = string.Format("{0}.{1}", guid.ToString(), Request.Files[0].FileName.Split('.').Last());
                    ImgLogo = fileName;

                    if (!string.IsNullOrEmpty(ImgLogo))
                    {
                        string caminho = System.Configuration.ConfigurationManager.AppSettings["UploadSave"].ToString();
                        System.IO.Directory.CreateDirectory(string.Format("{0}\\{1}\\{2}", caminho, "Services", services.ServiceId));
                        Request.Files[0].SaveAs(string.Format("{0}\\{1}\\{2}\\{3}", caminho, "Services", services.ServiceId, ImgLogo));
                    }
                }
                if (img != "")
                {
                    services.Image = ImgLogo;
                }

                services.AlterDate = DateTime.Now;
                db.Entry(services).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(services);
        }

        //
        // GET: /Services/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Services services = db.Services.Find(id);
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        //
        // POST: /Services/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Services services = db.Services.Where(e => e.Deleted == false  && e.ServiceId == id).FirstOrDefault();
            services.Deleted = true;
            services.DeletedDate = DateTime.Now;
            db.Entry(services).State = EntityState.Modified;
            db.SaveChanges();

            //coloca todos na posição
            var servicesposition = db.Services.Where(e => e.Deleted == false).ToList();
            int count = 0;
            foreach (var item in servicesposition)
            {
                count++;

               item.Position = count;
               db.Entry(item).State = EntityState.Modified;
            }
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