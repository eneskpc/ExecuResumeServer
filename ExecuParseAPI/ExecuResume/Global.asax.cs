using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ExecuResume
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Timer timer = new Timer(1000 * 3600 * 3);
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(@"\TemporaryFiles"));

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
