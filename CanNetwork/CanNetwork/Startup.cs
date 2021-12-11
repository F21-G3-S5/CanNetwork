using CanNetwork.Context;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Linq;

[assembly: OwinStartup(typeof(CanNetwork.Startup))]

namespace CanNetwork
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            MyDbContext db = new MyDbContext();
            if (db.Admins.Where(a=>a.Name == "Vamsi").Count() < 1)
            {
                db.Admins.Add(new Models.Admin { Id = 8, Name = "Vamsi", Email = "vamsi@gmail.com", Password = "0123456789" });
                db.SaveChanges();
            }
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            });
        }
    }
}