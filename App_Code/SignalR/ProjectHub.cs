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

        // Method to call when a client connects to the hub and forms a hubproxy
        public override async Task OnConnected()
        {
            try
            {
                ctx = new TeeMsEntities();

                int project_id = int.Parse(this.Context.QueryString["Project"]);

                var connectionlist = ctx.connection.ToList();
                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();

                foreach (var connection in connectionlist)
                {
                    if (connection.person_id == rightperson.person_id)
                    {
                        var rightconnection = ctx.connection.Where(con => con.connection_id == connection.connection_id).SingleOrDefault();

                        rightconnection.connected = true;

                        ctx.SaveChanges();
                    }
                }

                var projectconnection = rightperson.project_person.Where(prope => prope.project_id == rightproject.project_id).SingleOrDefault();

                if (projectconnection != null)
                {
                    await JoinGroup(rightproject.name + rightproject.project_id.ToString());
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

        public async Task JoinGroup(string projectname)
        {
            try
            {
                ctx = new TeeMsEntities();

                int project_id = int.Parse(this.Context.QueryString["Project"]);

                await Groups.Add(Context.ConnectionId, projectname);

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();

                rightconnection.project_id = rightproject.project_id;
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SaveComment(string commentcon)
        {
            try
            {
                ctx = new TeeMsEntities();

                if (commentcon != String.Empty)
                {
                    int project_id = int.Parse(this.Context.QueryString["Project"]);
                    string username = Context.User.Identity.Name;

                    var rightperson = ctx.person.Where(p => p.username == username).SingleOrDefault();

                    comment newcomment = new comment
                    {
                        comment_content = commentcon,
                        creation_date = DateTime.Now,
                        person_id = rightperson.person_id,
                        project_id = project_id
                    };

                    ctx.comment.Add(newcomment);
                    ctx.SaveChanges();

                    BroadcastUpdateComments();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void BroadcastUpdateComments()
        {
            try
            {
                ctx = new TeeMsEntities();

                int project_id = int.Parse(this.Context.QueryString["Project"]);

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();

                // Call updateComments method on the clients side
                Clients.Group(rightproject.name + rightproject.project_id.ToString()).updateComments();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void BroadcastUpdateProjectDescription()
        {
            try
            {
                ctx = new TeeMsEntities();

                int project_id = int.Parse(this.Context.QueryString["Project"]);

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();

                // Call fillMemberList method on the clients side
                Clients.OthersInGroup(rightproject.name + rightproject.project_id.ToString()).updateProjectDescription();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void UpdateClientsComments(int project_id, person rightperson)
        {
            ctx = new TeeMsEntities();

            var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
            var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();

            // Call updateComments method on the clients side
            Clients.Group(rightproject.name + rightproject.project_id.ToString()).updateComments();
        }
    }
}