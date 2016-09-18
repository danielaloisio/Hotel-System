using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataEF;
using KobraSoftware.Filters;

namespace KobraSoftware.Controllers
{
    public class MenusController : Controller
    {
        private KobraEntities db = new KobraEntities();

        public PartialViewResult CreateMenu()
        {
            var user = UserRepository.GetUserLogged();
            ViewBag.userlogged = user.ProfileId;
            var listRet = db.Menus.Include(e => e.Menus2).Where(e => e.Deleted == false && e.MenuIdParent == null && e.Profiles.Where(ee => ee.ProfileId == user.ProfileId).Count() > 0 && e.Visible == true).ToList();
            return PartialView(listRet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}