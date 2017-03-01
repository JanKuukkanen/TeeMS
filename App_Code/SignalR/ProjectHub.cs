using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.SignalR;

/// <summary>
/// Summary description for ProjectHub
/// </summary>
namespace SignalRTeeMs
{
    public class ProjectHub : Hub
    {
        public ProjectHub()
        {
            GlobalHost.ConnectionManager.GetHubContext<TeeMsHub>();
            //
            // TODO: Add constructor logic here
            //
        }
    }
}