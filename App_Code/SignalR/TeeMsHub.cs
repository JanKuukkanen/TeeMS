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
        private TeeMsEntities ctx;
        private FormsAuthenticationTicket ticket;

        public void Hello()
        {
            Clients.All.hello();
        }

        public override Task OnConnected()
        {
            try 
	        {
                ctx = new TeeMsEntities();
                HttpCookie authcookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
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

                return base.OnConnected();
	        }
	        catch (Exception ex)
	        {

                throw ex;
	        }
        }

        public override Task OnReconnected()
        {
            try
            {
                ctx = new TeeMsEntities();
                HttpCookie authcookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
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

                return base.OnReconnected();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {
                HttpCookie authcookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                ticket = FormsAuthentication.Decrypt(authcookie.Value);

                var connectionlist = ctx.connection.ToList();
                var rightperson = ctx.person.Where(p => p.username == ticket.Name).SingleOrDefault();

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

                return base.OnDisconnected(stopCalled);
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
                HttpCookie authcookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                ticket = FormsAuthentication.Decrypt(authcookie.Value);

                // Call the broadcastMessage method to update clients.
                Clients.All.broadcastMessage(ticket.Name, message);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
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
