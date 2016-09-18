using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataEF;
using KobraSoftware.Filters;

namespace KobraSoftware.Security
{
    public class PermissionFilter : AuthorizeAttribute
    {
       
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

           var user =  UserRepository.GetUserLogged();
           if (user.ProfileId != 1)
           {
               filterContext.HttpContext.Response.Redirect("/Home/Index");
           }    
            //if (filterContext.Result is HttpUnauthorizedResult)
            //{
            //    filterContext.HttpContext.Response.Redirect("/Home/Index");
            //}
        }
    }
}