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
/// Summary description for GroupHub
/// </summary>
namespace SignalRTeeMs
{
    public class GroupHub : Hub
    {
        // First we'll set set the database entity context and formsauthentication ticket
        private TeeMsEntities ctx;
        private FormsAuthenticationTicket ticket;

        // Method to call when a client connects to the hub and forms a hubproxy
        public override async Task OnConnected()
        {
            try
            {
                ctx = new TeeMsEntities();

                int group_id = int.Parse(this.Context.QueryString["Group"]);

                var connectionlist = ctx.connection.ToList();
                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightgroup = ctx.group.Where(g => g.group_id == group_id).SingleOrDefault();

                foreach (var connection in connectionlist)
                {
                    if (connection.person_id == rightperson.person_id)
                    {
                        var rightconnection = ctx.connection.Where(con => con.connection_id == connection.connection_id).SingleOrDefault();

                        rightconnection.connected = true;

                        ctx.SaveChanges();
                    }
                }

                var groupconnection = rightperson.group_member.Where(gm => gm.group_id == group_id).SingleOrDefault();

                if (groupconnection != null)
                {
                    await JoinGroup(rightgroup.name + rightgroup.group_id.ToString());
                }

                if (rightgroup.name != String.Empty)
                {
                    SendGroupName(rightgroup);
                }

                if (rightgroup.group_picture_url != String.Empty)
                {
                    SendGroupImage(rightgroup);
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

                int group_id = int.Parse(this.Context.QueryString["Group"]);

                await Groups.Add(Context.ConnectionId, groupname);

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightgroup = ctx.group.Where(g => g.group_id == group_id).SingleOrDefault();

                rightconnection.group_id = rightgroup.group_id;
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SendGroupName(group rightgroup)
        {
            string groupname = rightgroup.name;
            int group_tag = (int)rightgroup.group_tag;
            string group_texttag = group_tag.ToString();

            Clients.Caller.insertGroupName(groupname, group_texttag);
        }

        public void SendGroupImage(group rightgroup)
        {
            string projectpic_url = rightgroup.group_picture_url;

            Clients.Caller.insertGroupImage(projectpic_url);
        }

        public void SaveGroupName(string groupname)
        {
            try
            {
                ctx = new TeeMsEntities();

                int group_id = int.Parse(this.Context.QueryString["Group"]);
                string newname = groupname;

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightgroup = ctx.group.Where(g => g.group_id == group_id).SingleOrDefault();

                rightgroup.name = newname;
                ctx.SaveChanges();

                // Call updateProjectTitle method on the client side
                // For some reason this call to the client side is made only if the variable passed to the client side is the same as the current project name in the database
                Clients.OthersInGroup(rightgroup.name + rightgroup.group_id.ToString()).updateGroupName(newname);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SaveGroupImage(string group_imageurl)
        {
            try
            {
                ctx = new TeeMsEntities();

                int group_id = int.Parse(this.Context.QueryString["Group"]);

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightgroup = ctx.group.Where(g => g.group_id == group_id).SingleOrDefault();

                rightgroup.group_picture_url = group_imageurl;
                ctx.SaveChanges();

                // Call updateProjectTitle method on the client side
                Clients.OthersInGroup(rightgroup.name + rightgroup.group_id.ToString()).updateGroupImage(group_imageurl);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    } 
}