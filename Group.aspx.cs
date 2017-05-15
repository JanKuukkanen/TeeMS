using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Web.UI.HtmlControls;
using TeeMs.UserContentManager;

public partial class Group : System.Web.UI.Page
{
    private TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        // set the database context using entity framework
        ctx = new TeeMsEntities();
        string group_textid = String.Empty;
        bool alloweduser = false;

        try
        {
            HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);

            group_textid = Request.QueryString["Group"];
            int group_id = int.Parse(group_textid);

            var rightgroup = ctx.group.Where(g => g.group_id == group_id).SingleOrDefault();
            var rightperson = ctx.person.Where(p => p.username == User.Identity.Name).SingleOrDefault();
            var rightmember = rightperson.group_member.Where(gm => gm.group_id == rightgroup.group_id).SingleOrDefault();

            if (rightmember != null)
            {
                alloweduser = true;
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }

        if (alloweduser != true)
        {
            Response.Redirect(String.Format(Request.ApplicationPath + "Home.aspx"));
        }

        if (!IsPostBack && group_textid != String.Empty)
        {
            FillControls(int.Parse(group_textid)); 
        }
    }

    protected void FillControls(int group_id)
    {
        try
        {
            // Set the titles of the page to be the name of the group

            var rightgroup = ctx.group.Where(g => g.group_id == group_id).SingleOrDefault();

            groupTitle.InnerText = rightgroup.name;

            var groupMembers = rightgroup.group_member.Where(gm => gm.group_id == group_id).Select(gm => gm.person_id).ToList();

            foreach (var item in groupMembers)
	        {
                var person = ctx.person.Where(p => p.person_id == item).SingleOrDefault();
                rblGroupMembers.Items.Add(person.username);
	        }

            if (rblGroupMembers.Items.Count > 0)
            {
                rblGroupMembers.Items[0].Selected = true; 
            }

            rblGroupRoleChange.Items.Add("Group Member");
            rblGroupRoleChange.Items.Add("Group Moderator");
            rblGroupRoleChange.Items.Add("Group Administrator");

            // Fill ddlProjecyList with groups currently working on the project
            var projectgroupquery = ctx.project_group.ToList();
            List<project> projects_in_group = new List<project>();

            if (projectgroupquery != null)
            {
                foreach (var projectgroup in projectgroupquery)
                {
                    if (projectgroup.group_id == rightgroup.group_id)
                    {
                        projects_in_group.Add(ctx.project.Where(pr => pr.project_id == projectgroup.project_id).SingleOrDefault());
                    }
                }

                foreach (var project in projects_in_group)
                {
                    ddlProjectList.Items.Add(project.name);
                }
            }
            ddlProjectList.Items.Insert(0, "Choose Project");
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

#region BUTTONS

    protected void btnAddMembers_Click(object sender, EventArgs e)
    {
        if (divSearch.Visible == false)
        {
            divSearch.Visible = true;
            btnAddMembers.Text = "Close search";
        }
        else
        {
            divSearch.Visible = false;
            btnAddMembers.Text = "Add members";
        }
    }

    protected void btnShowInfo_Click(object sender, EventArgs e)
    {
        string user_to_show = rblGroupMembers.SelectedValue;
        string group_id = String.Empty;

        try
        {
            var rightperson = ctx.person.Where(p => p.username == user_to_show).SingleOrDefault();
            Response.Redirect(String.Format(Request.ApplicationPath + "ViewUser.aspx?Person={0}", rightperson.person_id));
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void btnDeleteMember_Click(object sender, EventArgs e)
    {
        string user_to_delete = rblGroupMembers.SelectedValue;
        string group_id = String.Empty;

        try
        {
            group_id = Request.QueryString["Group"];
            int removefromgroup = int.Parse(group_id);

            var rightperson = ctx.person.Where(p => p.username == user_to_delete).SingleOrDefault();
            var rightgroup = ctx.group.Where(g => g.group_id == removefromgroup).SingleOrDefault();

            if (rightperson != null && rightgroup != null)
            {
                var rightgroupmember = ctx.group_member.Where(gm => gm.person_id == rightperson.person_id && gm.group_id == rightgroup.group_id).SingleOrDefault();

                foreach (var projectperson in rightperson.project_person.ToList())
                {
                    if (projectperson.group_id == rightgroup.group_id)
                    {
                        ctx.project_person.Remove(projectperson);
                    }
                }

                if (rightgroupmember != null)
                {
                    ctx.group_member.Remove(rightgroupmember);
                    ctx.SaveChanges();

                    Response.Redirect(String.Format(Request.ApplicationPath + "Group.aspx?Group={0}", rightgroup.group_id));
                }
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void btnSearchGroupMembers_Click(object sender, EventArgs e)
    {
        // Search the database for the persons the user is searching
        string searchmember = txtSearchGroupMembers.Text;

        List<person> userstoadd = SearchMembers(searchmember);

        DataTable dt = new DataTable();

        dt.Columns.Add("username", typeof(string));
        dt.Columns.Add("first_name", typeof(string));
        dt.Columns.Add("last_name", typeof(string));
        dt.Columns.Add("email", typeof(string));

        // Then fill the gridview with the results
        try
        {
            if (userstoadd != null)
            {
                foreach (var person in userstoadd)
                {
                    dt.Rows.Add(person.username, person.first_name, person.last_name, person.email);
                    gvGroupMembers.DataSource = dt;
                    gvGroupMembers.DataBind();
                }
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void btnConfirmAssignRole_Click(object sender, EventArgs e)
    {
        string newrole = rblGroupRoleChange.SelectedValue;
        string user_to_change = rblGroupMembers.SelectedValue;

        try
        {
            int group_id = int.Parse(Request.QueryString["Group"]);

            var rightrole = ctx.group_role.Where(gro => gro.name == newrole).SingleOrDefault();
            var rightgroupmember = ctx.group_member.Where(grm => grm.person.username == user_to_change && grm.group_id == group_id).SingleOrDefault();

            if (rightgroupmember.group_role.@class != 1)
            {
                rightgroupmember.group_role = rightrole;
                ctx.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

#endregion

#region SEARCH_FUNCTIONS

    // Search the database for the persons the user is searching
    protected List<person> SearchMembers(string searchmember)
    {
        bool add_to_grid = true;
        List<person> userstoadd = new List<person>();

        try
        {
            var users = ctx.person.ToList();

            if (users != null && searchmember != String.Empty)
            {
                foreach (var person in users)
                {
                    add_to_grid = true;

                    for (int i = 0; i < searchmember.Count(); i++)
                    {
                        if (searchmember[i] != person.username[i])
                        {
                            add_to_grid = false;
                        }
                    }

                    if (add_to_grid == true)
                    {
                        userstoadd.Add(person);
                    }
                }
            }

            return userstoadd;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
            return userstoadd;
        }
    }

    protected void gvGroupMembers_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Add the selected member to the group
        string username = (gvGroupMembers.SelectedRow.Cells[0].Controls[0] as LinkButton).Text;
        string group_id = String.Empty;


        // Fill the group_member database table as well
        // Send an invitation to the group
        try
        {
            group_id = Request.QueryString["Group"];

            int to_add_groupid = int.Parse(group_id);

            if (group_id != String.Empty)
            {
                var usertoinvite = ctx.person.Where(p => p.username == username).FirstOrDefault();
                var invitegroup = ctx.group.Where(g => g.group_id == to_add_groupid).FirstOrDefault();

                if (usertoinvite != null && invitegroup != null)
                {
                    invite newinvitation = new invite() 
                    {
                        person = usertoinvite,
                        group = invitegroup,
                        invite_content = String.Format("You have been invited to group {0}, do you wish to accept?", invitegroup.name)
                    };

                    ctx.invite.Add(newinvitation);

                    ctx.SaveChanges();
                } 
            }
            else
            {
                lbMessages.Text = "No Group Specified";
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }

        if (group_id != String.Empty)
        {
            Response.Redirect(String.Format(Request.ApplicationPath + "Group.aspx?Group={0}", group_id));
        }
    }

#endregion

    protected void ddlProjectList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string projectname = ddlProjectList.SelectedValue;
        string group_id = String.Empty;

        try
        {
            lbProjectInfo.Text = "";

            group_id = Request.QueryString["Group"];
            int groupnro_id = int.Parse(group_id);
            var rightproject = ctx.project.Where(pr => pr.name == projectname).SingleOrDefault();

            if (rightproject != null)
            {
                var group_project = ctx.project_group.Where(pg => pg.project_id == rightproject.project_id && pg.group_id == groupnro_id).SingleOrDefault();

                if (group_project != null)
                {

                    lbProjectInfo.Text = group_project.project.description;
                }

                if (rightproject.assignment != null)
                {
                    FillAssignmentProgress(rightproject);
                }
            }
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void FillAssignmentProgress(project rightproject)
    {
        try
        {
            divProjectAssignmentList.Controls.Clear();

            var assignmentlist = ctx.assignment.Where(amt => amt.project_id == rightproject.project_id).ToList();

            foreach (var assignment in assignmentlist)
            {
                var finishedcomponentlist = assignment.assignment_component.Where(amtc => amtc.finished == true).ToList();

                int componentcount = assignment.assignment_component.Count;
                int finishedcomponents = finishedcomponentlist.Count;

                double assignmentpercent = 0;

                if (finishedcomponents > 0)
                {
                    assignmentpercent = ((double)finishedcomponents * 100) / (double)componentcount;
                }

                HtmlGenericControl assignmentdiv = new HtmlGenericControl("div");
                HtmlGenericControl assignmentprogressdiv = new HtmlGenericControl("div");
                HtmlGenericControl assignmentprogressbardiv = new HtmlGenericControl("div");
                Label assignmentnamelabel = new Label();
                Label assignmentinfolabel = new Label();

                assignmentinfolabel.Text = String.Format("Assignments: {0}. Finished assignments: {1}", componentcount, finishedcomponents);
                assignmentinfolabel.Attributes.Add("style", "display:inline-block");

                assignmentnamelabel.Text = assignment.name;
                assignmentnamelabel.Attributes.Add("style", "float:left; margin-right:10px;");

                assignmentdiv.Attributes.Add("class", "projectprogressbars");
                assignmentdiv.Attributes.Add("style", "padding-top:10px; padding-bottom:5px; padding-left:5px;");

                assignmentprogressdiv.Attributes.Add("class", "progress");
                assignmentprogressdiv.Attributes.Add("style", "float:left; width:60%; background-color:gray;");

                assignmentprogressbardiv.Attributes.Add("class", "progress-bar");
                assignmentprogressbardiv.Attributes.Add("role", "progressbar");
                assignmentprogressbardiv.Attributes.Add("aria-valuemin", "0");
                assignmentprogressbardiv.Attributes.Add("aria-valuemax", "100");
                assignmentprogressbardiv.Attributes.Add("style", String.Format("width:{0}%; background-color:#337ab7; color:#fff;", assignmentpercent));

                if (finishedcomponents > 0)
                {
                    assignmentprogressbardiv.InnerText = String.Format("{0}% Complete", assignmentpercent);
                }

                // Assign right elements to their parent elements
                assignmentprogressdiv.Controls.Add(assignmentprogressbardiv);

                assignmentdiv.Controls.Add(assignmentnamelabel);
                assignmentdiv.Controls.Add(assignmentprogressdiv);
                assignmentdiv.Controls.Add(assignmentinfolabel);

                divProjectAssignmentList.Controls.Add(assignmentdiv);
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void btnShowProjectPage_Click(object sender, EventArgs e)
    {
        string projectname = ddlProjectList.SelectedValue;
        string group_id = String.Empty;

        try
        {
            group_id = Request.QueryString["Group"];
            int groupnro_id = int.Parse(group_id);
            var rightproject = ctx.project.Where(pr => pr.name == projectname).SingleOrDefault();

            if (rightproject != null)
            {
                Response.Redirect(String.Format(Request.ApplicationPath + "Project.aspx?Project={0}", rightproject.project_id));
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }
}