using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for ProjectHub
/// </summary>
namespace SignalRTeeMs
{
    public class ProjectHub : Hub
    {
        // Reference for getting the context of another SignalR hub, in this case the TeeMsHub context
        // GlobalHost.ConnectionManager.GetHubContext<TeeMsHub>();

        // First we'll set set the database entity context and formsauthentication ticket
        private TeeMsEntities ctx;
        private FormsAuthenticationTicket ticket;

        // Method to call when a client connects to the hub and forms a hubproxy
        public override async Task OnConnected()
        {

        }

        // Method to call when a client reconnects to the hub
        public override async Task OnReconnected()
        {

        }

        // Method to call when a client disconnects from the hub either gracefully or not
        public override async Task OnDisconnected(bool stopCalled)
        {

        }
    }
}