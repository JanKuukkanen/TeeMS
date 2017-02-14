using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SignalRChat
{
    public class TeeMsHub : Hub
    {
        private TeeMsEntities ctx;
        private FormsAuthenticationTicket ticket;

        public void Hello()
        {
            Clients.All.hello();
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public void Send(string name, string message)
        {
            HttpCookie authcookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);

            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(ticket.Name, message);
        }

        public string Authenticate()
        {
            try
            {
                HttpCookie authcookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                ticket = FormsAuthentication.Decrypt(authcookie.Value);

                var rightperson = ctx.person.Where(p => p.username == ticket.Name).SingleOrDefault();

                return rightperson.username;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    } 
}
