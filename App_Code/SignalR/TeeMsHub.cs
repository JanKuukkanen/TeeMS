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

                var connectionlist = ctx.connection.ToList();
                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();

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

                List<string> grouplist = GetMemberGroups(rightperson);

                if (grouplist != null)
                {
                    await JoinGroup(grouplist[0]); 
                }

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

                var connectionlist = ctx.connection.ToList();
                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();

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

                var connectionlist = ctx.connection.ToList();
                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault(); 

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

                await base.OnDisconnected(stopCalled);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task JoinGroup(string groupname)
        {
            await Groups.Add(Context.ConnectionId, groupname);
            Clients.Group(groupname).broadcastMessage("ChatControl", Context.ConnectionId + " added to group: " + groupname);
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }

        public void GetChatMembers()
        {
            try
            {
                List<string> memberlist = ctx.connection.Where(con => con.connected == true).Select(con => con.connection_username).ToList();

                string connectionjson = JsonConvert.SerializeObject(memberlist);

                // Call fillMemberList method on the clients side
                Clients.All.fillMemberList(connectionjson);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public List<string> GetMemberGroups(person rightperson)
        {
            try
            {
                var groupmemberlist = rightperson.group_member.ToList();

                List<group> grouplist = new List<group>();

                foreach (var groupmember in groupmemberlist)
                {
                    grouplist.Add(groupmember.group);
                }

                List<string> groupnamelist = new List<string>();

                foreach (var group in grouplist)
                {
                    groupnamelist.Add(group.name);
                }

                string groupjson = JsonConvert.SerializeObject(groupnamelist);

                // Call fillGroupList method on the clients side
                Clients.Client(Context.ConnectionId).fillGroupList(groupjson);

                return groupnamelist;
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
                if (Context.User.Identity.IsAuthenticated)
                {
                    string username = Context.User.Identity.Name;

                    // Call the broadcastMessage method to update clients.
                    Clients.All.broadcastMessage(username, message);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void Send(string name, string message, string groupname)
        {
            try
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    string username = Context.User.Identity.Name;

                    // Call the broadcastMessage method to update clients.
                    Clients.Group(groupname).broadcastMessage(name, message);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    } 
}
