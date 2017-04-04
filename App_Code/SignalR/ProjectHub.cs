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
                string assignment_textid = String.Empty;

                if (this.Context.QueryString["Assignment"] != null)
                {
                    assignment_textid = this.Context.QueryString["Assignment"];
                }

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

                if (rightproject.name != String.Empty)
                {
                    SendProjectName(rightproject);
                }

                if (rightproject.picture_url != String.Empty)
                {
                    SendProjectImage(rightproject);
                }

                if (assignment_textid != String.Empty)
                {
                    int assignment_id = int.Parse(assignment_textid);

                    var rightassignment = ctx.assignment.Where(amt => amt.amt_id == assignment_id).SingleOrDefault();

                    SendAssignmentDescription(rightproject, rightassignment);
                }
                else
                {
                    SendProjectDescription(rightproject);
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

        public void SendProjectName(project rightproject)
        {
            string projectname = rightproject.name;
            int project_tag = (int) rightproject.project_tag;
            string project_texttag = project_tag.ToString();
            string archive = "false";

            if (rightproject.finished == true)
            {
                archive = "true";
            }

            Clients.Caller.insertProjectName(projectname, project_texttag, archive);
        }

        public void SendProjectImage(project rightproject)
        {
            string projectpic_url = rightproject.picture_url;
            string archive = "false";

            if (rightproject.finished == true)
            {
                archive = "true";
            }

            Clients.Caller.insertProjectImage(projectpic_url, archive);
        }

        public void SendProjectDescription(project rightproject)
        {
            string projectdesc = rightproject.description;
            string archive = "false";

            if (rightproject.finished == true)
            {
                archive = "true";
            }

            Clients.Caller.insertProjectDescription(projectdesc, archive);
        }

        public void SendAssignmentDescription(project rightproject, assignment rightassignment)
        {
            string assignmentdesc = rightassignment.description;
            string archive = "false";

            if (rightproject.finished == true)
            {
                archive = "true";
            }

            Clients.Caller.insertAssignmentDescription(assignmentdesc, archive);
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

        public void SaveAssignmentComment(string commentcon)
        {
            try
            {
                ctx = new TeeMsEntities();

                if (commentcon != String.Empty)
                {
                    int project_id = int.Parse(this.Context.QueryString["Project"]);
                    int assignment_id = int.Parse(this.Context.QueryString["Assignment"]);
                    string username = Context.User.Identity.Name;

                    var rightperson = ctx.person.Where(p => p.username == username).SingleOrDefault();

                    comment newcomment = new comment
                    {
                        comment_content = commentcon,
                        creation_date = DateTime.Now,
                        person_id = rightperson.person_id,
                        project_id = project_id,
                        amt_id = assignment_id
                    };

                    ctx.comment.Add(newcomment);
                    ctx.SaveChanges();

                    BroadcastUpdateAssignmentComments();
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

        public void BroadcastUpdateAssignmentComments()
        {
            try
            {
                ctx = new TeeMsEntities();

                int project_id = int.Parse(this.Context.QueryString["Project"]);

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();

                // Call updateAssignmentComments method on the clients side
                Clients.Group(rightproject.name + rightproject.project_id.ToString()).updateAssignmentComments();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SaveProjectName(string projectname)
        {
            try
            {
                ctx = new TeeMsEntities();

                int project_id = int.Parse(this.Context.QueryString["Project"]);
                string newname = projectname;

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();

                rightproject.name = newname;
                ctx.SaveChanges();

                // Call updateProjectTitle method on the client side
                // For some reason this call to the client side is made only if the variable passed to the client side is the same as the current project name in the database
                Clients.OthersInGroup(rightproject.name + rightproject.project_id.ToString()).updateProjectName(newname);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SaveProjectImage(string project_imageurl)
        {
            try
            {
                ctx = new TeeMsEntities();

                int project_id = int.Parse(this.Context.QueryString["Project"]);

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();

                rightproject.picture_url = project_imageurl;
                ctx.SaveChanges();

                // Call updateProjectTitle method on the client side
                Clients.OthersInGroup(rightproject.name + rightproject.project_id.ToString()).updateProjectImage(project_imageurl);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SaveProjectDescription(string projectdescription)
        {
            try
            {
                ctx = new TeeMsEntities();

                int project_id = int.Parse(this.Context.QueryString["Project"]);

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();

                rightproject.description = projectdescription;
                ctx.SaveChanges();

                // Call updateProjectDescription method on the clients side
                Clients.OthersInGroup(rightproject.name + rightproject.project_id.ToString()).updateProjectDescription(projectdescription);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SaveAssignmentDescription(string assignmentdescription)
        {
            try
            {
                ctx = new TeeMsEntities();

                int project_id = int.Parse(this.Context.QueryString["Project"]);

                var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
                var rightconnection = ctx.connection.Where(con => con.person_id == rightperson.person_id).SingleOrDefault();
                var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();

                rightproject.description = assignmentdescription;
                ctx.SaveChanges();

                // Call updateAssignmentDescription method on the clients side
                Clients.OthersInGroup(rightproject.name + rightproject.project_id.ToString()).updateAssignmentDescription(assignmentdescription);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}