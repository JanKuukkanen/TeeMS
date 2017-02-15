using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace SignalRChat
{
    public class TeeMsHub : Hub
    {
        // First we'll set set the database entity context and formsauthentication ticket
        private TeeMsEntities ctx;
        private FormsAuthenticationTicket ticket;

        public void Hello()
        {
            Clients.All.hello();
        }

        // Method to call when a client connects to the hub and forms a hubproxy
        public override async Task OnConnected()
        {
            try 
	        {
                ctx = new TeeMsEntities();
                HttpContextBase httpctx = Context.Request.GetHttpContext();
                HttpCookie authcookie = httpctx.Request.Cookies[FormsAuthentication.FormsCookieName];
                ticket = FormsAuthentication.Decrypt(authcookie.Value);

                var connectionlist = ctx.connection.ToList();
                var rightperson = ctx.person.Where(p => p.username == ticket.Name).SingleOrDefault();

                if (connectionlist.Count != 0)
                {
                    bool makenewconnection = true;

                    foreach (var connection in connectionlist)
                    {
                        if (connection.person_id == rightperson.person_id)
                        {
                            var rightconnection = ctx.connection.Where(con => con.connection_id == connection.connection_id).SingleOrDefault();

                            rightconnection.connected = true;

                            ctx.SaveChanges();

                            makenewconnection = false;
                        }
                    }

                    if (makenewconnection == true)
                    {
                        connection newconnection = new connection()
                        {
                            connected = true,
                            connection_username = rightperson.username,
                            person_id = rightperson.person_id
                        };

                        ctx.connection.Add(newconnection);
                        ctx.SaveChanges();
                    }
                }
                else
                {
                    connection newconnection = new connection()
                    {
                        connected = true,
                        connection_username = rightperson.username,
                        person_id = rightperson.person_id
                    };

                    ctx.connection.Add(newconnection);
                    ctx.SaveChanges();
                }

                GetChatMembers();

                await base.OnConnected();
	        }
	        catch (Exception ex)
	        {

                throw ex;
	        }
        }

        // Method to call when a client reconnects to the hub
        public override async Task OnReconnected()
        {
            try
            {
                ctx = new TeeMsEntities();
                HttpContextBase httpctx = Context.Request.GetHttpContext();
                HttpCookie authcookie = httpctx.Request.Cookies[FormsAuthentication.FormsCookieName];
                ticket = FormsAuthentication.Decrypt(authcookie.Value);

                var connectionlist = ctx.connection.ToList();
                var rightperson = ctx.person.Where(p => p.username == ticket.Name).SingleOrDefault();

                foreach (var connection in connectionlist)
                {
                    if (connection.person_id == rightperson.person_id)
                    {
                        var rightconnection = ctx.connection.Where(con => con.connection_id == connection.connection_id).SingleOrDefault();

                        rightconnection.connected = true;

                        ctx.SaveChanges();
                    }
                }

                await base.OnReconnected();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // Method to call when a client disconnects from the hub either gracefully or not
        public override async Task OnDisconnected(bool stopCalled)
        {
            try
            {
                ctx = new TeeMsEntities();
                // We'll use Context.Request.GetHttpContext because during a disconnect HttpContext.Current can be null
                HttpContextBase httpctx = Context.Request.GetHttpContext();
                HttpCookie authcookie = httpctx.Request.Cookies[FormsAuthentication.FormsCookieName];
                ticket = FormsAuthentication.Decrypt(authcookie.Value);

                var connectionlist = ctx.connection.ToList();
                var rightperson = ctx.person.Where(p => p.username == ticket.Name).SingleOrDefault(); 

                // We'll set the connection property in the database to false and enter the time of disconnect
                foreach (var connection in connectionlist)
                {
                    if (connection.person_id == rightperson.person_id)
                    {
                        var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();

                        rightconnection.connected = false;
                        rightconnection.connection_time = DateTime.Now;

                        ctx.SaveChanges();
                    }
                }

                GetChatMembers();

                await base.OnDisconnected(stopCalled);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void GetChatMembers()
        {
            try
            {
                List<string> memberlist = ctx.connection.Where(con => con.connected == true).Select(con => con.connection_username).ToList();

                string connectionjson = JsonConvert.SerializeObject(memberlist);

                // Call fillMemberList method to get the memberlist
                Clients.All.fillMemberList(connectionjson);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void Send(string name, string message)
        {
            try
            {
                HttpContextBase httpctx = Context.Request.GetHttpContext();
                HttpCookie authcookie = httpctx.Request.Cookies[FormsAuthentication.FormsCookieName];
                ticket = FormsAuthentication.Decrypt(authcookie.Value);

                // Call the broadcastMessage method to update clients.
                Clients.All.broadcastMessage(ticket.Name, message);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    } 
}
