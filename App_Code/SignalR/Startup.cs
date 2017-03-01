using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(SignalRTeeMs.Startup))]

namespace SignalRTeeMs
{
    public class Startup
    {
        public Startup()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    } 
}