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
        string group_id = String.Empty;

        try
        {
            HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);

            group_id = Request.QueryString["Group"];
        }
        catch (HttpException ex)
        {

            lbMessages.Text = ex.Message;
        }

        if (!IsPostBack && group_id != String.Empty)
        {
            FillControls(int.Parse(group_id)); 
        }
    }

    protected void FillControls(int group_id)
    {
        try
        {
            // Set the titles of the page to be the name of the group

            var rightgroup = ctx.group.Where(g => g.group_id == group_id).SingleOrDefault();

            groupTitle.InnerText = rightgroup.name;
            h1GroupName.InnerText = rightgroup.name;

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

            string pictureuri = Request.ApplicationPath + "Images/no_image.png";

            if (rightgroup.group_picture_url != null)
            {
                if (rightgroup.group_picture_url != String.Empty)
                {
                    imgGroupPicture.Src = rightgroup.group_picture_url; 
                }
            }
            else
            {
                imgGroupPicture.Src = pictureuri;
            }

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
    protected void lbtnTriggerTitleChange_Click(object sender, EventArgs e)
    {
        // Change the visibility of certain html elements to bring up a textbox
        divDefault.Visible = false;
        divDuringChange.Visible = true;
        divTitleChanger.Visible = true;
    }

    protected void btnTitleChanger_Click(object sender, EventArgs e)
    {
        // Change The name of the group
        UserContentManager contentmanager = new UserContentManager(ticket.Name);

        try
        {
            string group_id = Request.QueryString["Group"];

            int parsed_group_id = int.Parse(group_id);
            bool is_same_name = false;

            var rightgroup = ctx.group.Where(g => g.group_id == parsed_group_id).SingleOrDefault();

            // Check that the user does not already belong to a group with the same name
            // as the one they're trying to give
            List<group> usergroupquery = contentmanager.GetUserGroups();

            foreach (var item in usergroupquery)
            {
                if (item.name == txtTitleChanger.Text)
                {
                    is_same_name = true;
                }
            }

            if (rightgroup != null && is_same_name == false)
            {
                rightgroup.name = txtTitleChanger.Text;
                rightgroup.edited = DateTime.Now;

                ctx.SaveChanges();

                h1GroupName.InnerText = txtTitleChanger.Text;
                groupTitle.InnerText = txtTitleChanger.Text;
                lbMessages.Text = String.Format("Changed group name");
            }
            else if (is_same_name == true)
            {
                lbMessages.Text = "You already have a group with that name";
            }
            else
            {
                lbMessages.Text = "Failed to change the name of the group";
            }

            divDuringChange.Visible = false;
            divTitleChanger.Visible = false;
            divDefault.Visible = true;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void btnTitleCancel_Click(object sender, EventArgs e)
    {
        divTitleChanger.Visible = false;
        divDuringChange.Visible = false;
        divDefault.Visible = true;
    }

    protected void btnChangePicture_Click(object sender, EventArgs e)
    {
        // Change the visibility of certain html elements to bring up a textbox
        divDefault.Visible = false;
        divDuringChange.Visible = true;
        divImageChanger.Visible = true;
    }

    protected void btnImageChanger_Click(object sender, EventArgs e)
    {
        try
        {
            string group_id = Request.QueryString["Group"];
            int parsed_group_id = int.Parse(group_id);
            bool gooduri = Uri.IsWellFormedUriString(txtImageChanger.Text, UriKind.Absolute);
            string pictureuri = Request.ApplicationPath + "Images/no_image.png";
            var rightgroup = ctx.group.Where(g => g.group_id == parsed_group_id).SingleOrDefault();

            if (gooduri == true)
            {
                pictureuri = txtImageChanger.Text;
            }
            else
            {
                lbMessages.Text = "Enter valid absolute url path of the picture";
            }

            if (rightgroup != null && txtImageChanger.Text != String.Empty)
            {
                rightgroup.group_picture_url = pictureuri;
                rightgroup.edited = DateTime.Now;

                ctx.SaveChanges();

                imgGroupPicture.Src = rightgroup.group_picture_url;
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }

        divImageChanger.Visible = false;
        divDuringChange.Visible = false;
        divDefault.Visible = true;
    }

    protected void btnImageCancel_Click(object sender, EventArgs e)
    {
        divImageChanger.Visible = false;
        divDuringChange.Visible = false;
        divDefault.Visible = true;
    }

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
        try
        {
            group_id = Request.QueryString["Group"];

            int to_add_groupid = int.Parse(group_id);

            if (group_id != String.Empty)
            {
                var addeduser = ctx.person.Where(p => p.username == username).FirstOrDefault();
                var addedgroup = ctx.group.Where(g => g.group_id == to_add_groupid).FirstOrDefault();
                var addedrole = ctx.group_role.Where(gr => gr.@class == 4).FirstOrDefault();

                if (addeduser != null && addedgroup != null && addedrole != null)
                {
                    var gm = new group_member
                    {
                        group_id = addedgroup.group_id,
                        person_id = addeduser.person_id,
                        grouprole_id = addedrole.grouprole_id
                    };

                    addeduser.edited = DateTime.Now;
                    addedgroup.edited = DateTime.Now;

                    addeduser.group_member.Add(gm);
                    addedgroup.group_member.Add(gm);
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
            }
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }
}