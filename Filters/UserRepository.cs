using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataEF;

namespace KobraSoftware.Filters
{
    public static class UserRepository
    {
        public static bool UserAuthenticate(string Email, string Password)
        {
            KobraEntities db = new KobraEntities();

            var users = db.Users.Where(e => e.Email.Equals(Email) && e.Password.Equals(Password)).FirstOrDefault();
            if (users == null)
            {
                return false;
            }
            System.Web.Security.FormsAuthentication.SetAuthCookie(users.Email, false);
            return true;
        }

        public static Users GetUserLogged()
        {
            KobraEntities db = new KobraEntities();
            string login = HttpContext.Current.User.Identity.Name;
            if (login == "")
            {
                return null;
            }
            else
            {
                var user = db.Users.Where(e => e.Deleted == false && e.Email == login).FirstOrDefault();
                return user;
            }
        }

    }
}