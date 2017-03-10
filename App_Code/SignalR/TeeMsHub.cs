using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SignalRTeeMs
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
                    GetChatMembers(grouplist[0]);
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
            try
            {
                ctx = new TeeMsEntities();

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightgroup = rightperson.group_member.Where(gm => gm.group.name == groupname).SingleOrDefault();

                await Groups.Add(Context.ConnectionId, groupname + rightgroup.group_id.ToString());

                rightconnection.group_id = rightgroup.group_id;
                ctx.SaveChanges();

                Clients.Group(groupname + rightgroup.group_id.ToString()).broadcastMessage("ChatControl", rightperson.username + " Joined group: " + groupname);

                GetChatMembers(rightgroup.group.name);
                GetConversationMessages(rightgroup.group.name);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public async Task LeaveGroup()
        {
            try
            {
                ctx = new TeeMsEntities();

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightgroup = rightconnection.group;

                await Groups.Remove(Context.ConnectionId, rightgroup.name + rightgroup.group_id.ToString());

                rightconnection.group_id = null;
                ctx.SaveChanges();

                Clients.OthersInGroup(rightgroup.name + rightgroup.group_id.ToString()).broadcastMessage("ChatControl", rightperson.username + " Left group");
                ChatMemberLeft(rightgroup.name);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void GetConversationMessages(string groupname)
        {
            try
            {
                ctx = new TeeMsEntities();

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightgroup = rightperson.group_member.Where(gm => gm.group.name == groupname).SingleOrDefault();

                var ctxmessagelist = ctx.message.Where(msg => msg.group_id == rightgroup.group.group_id).ToList();

                List<string[]> stringarraylist = new List<string[]>();

                foreach (var message in ctxmessagelist)
                {
                    string[] messagearray = { message.person.username, message.creation_date.ToString(), message.message_content };
                    stringarraylist.Add(messagearray);
                }

                string messagejson = JsonConvert.SerializeObject(stringarraylist);

                // Call fillDiscussion method on the clients side
                Clients.Client(Context.ConnectionId).fillDiscussion(messagejson);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void GetChatMembers(string groupname)
        {
            try
            {
                ctx = new TeeMsEntities();

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightgroup = rightperson.group_member.Where(gm => gm.group.name == groupname).SingleOrDefault();

                List<string> memberlist = ctx.connection.Where(con => con.connected == true && con.group_id == rightgroup.group_id).Select(con => con.connection_username).ToList();

                string connectionjson = JsonConvert.SerializeObject(memberlist);

                // Call fillMemberList method on the clients side
                Clients.Group(groupname + rightgroup.group_id.ToString()).fillMemberList(connectionjson);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void ChatMemberLeft(string groupname)
        {
            try
            {
                ctx = new TeeMsEntities();

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightgroup = rightperson.group_member.Where(gm => gm.group.name == groupname).SingleOrDefault();

                List<string> memberlist = ctx.connection.Where(con => con.connected == true && con.group_id == rightgroup.group_id).Select(con => con.connection_username).ToList();

                string connectionjson = JsonConvert.SerializeObject(memberlist);

                // Call fillMemberList method on the clients side
                Clients.OthersInGroup(groupname + rightgroup.group_id.ToString()).fillMemberList(connectionjson);
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

        public void Send(string message)
        {
            try
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    ctx = new TeeMsEntities();

                    var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                    var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                    var rightgroup = rightconnection.group;

                    message newmessage = new message()
                    {
                        person_id = rightperson.person_id,
                        group_id = rightgroup.group_id,
                        message_content = message,
                        creation_date = DateTime.Now
                    };

                    ctx.message.Add(newmessage);
                    ctx.SaveChanges();

                    Regex regex = new Regex("#PRO(?<tag>[0-9]{1,3})");

                    Match tagmatch = regex.Match(message);

                    if (tagmatch.Success)
                    {
                        string stringtag = tagmatch.Groups["tag"].Value;
                        int projecttag = int.Parse(stringtag);
                        SaveAsComment(message, rightperson.person_id, projecttag);
                    }

                    // Call the broadcastMessage method to update clients.
                    Clients.Group(rightgroup.name + rightgroup.group_id.ToString()).broadcastMessage(rightperson.username, message);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        protected void SaveAsComment(string message, int personid, int projecttag)
        {
            try
            {
                ctx = new TeeMsEntities();

                var rightperson = ctx.person.Where(p => p.person_id == personid).SingleOrDefault();
                var rightproject = ctx.project.Where(pr => pr.project_tag == projecttag).SingleOrDefault();

                comment newcomment = new comment()
                {
                    comment_content = message,
                    person = rightperson,
                    project = rightproject,
                    creation_date = DateTime.Now
                };

                ctx.comment.Add(newcomment);
                ctx.SaveChanges();
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
                    var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                    var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                    var rightgroup = rightconnection.group;

                    // Call the broadcastMessage method to update clients.
                    Clients.Group(groupname + rightgroup.group_id).broadcastMessage(name, message);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    } 
}
