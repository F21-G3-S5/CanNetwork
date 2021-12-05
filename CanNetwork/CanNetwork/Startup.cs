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
            if (db.Admins.Where(a=>a.Name == "Abdelrhman").Count() < 1)
            {
                db.Admins.Add(new Models.Admin { Id = 1, Name = "Abdelrhman", Email = "abdelrhman44@gmail.com", Password = "01128479486" });
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