using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using TeeMs.UserContentManager;

public partial class Project : System.Web.UI.Page
{
    private TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    //private List<group> projectgroups_perm = new List<group>();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        ctx = new TeeMsEntities();

        string project_id = String.Empty;

        try
        {
            HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);

            project_id = Request.QueryString["Project"];
        }
        catch (HttpException ex)
        {

            lbMessages.Text = ex.Message;
        }

        if (!IsPostBack)
        {
            FillControls(int.Parse(project_id));
        }
    }

    protected void FillControls(int project_id)
    {
        try
        {
            UserContentManager contentmanager = new UserContentManager(ticket.Name);

            var usergroups = contentmanager.GetUserGroups();

            // Set the titles of the page to be the name of the group
            var rightproject = ctx.project.Where(g => g.project_id == project_id).SingleOrDefault();

            projectTitle.InnerText = rightproject.name;
            h1ProjectName.InnerText = rightproject.name;

            // Set project description
            txtProjectDescription.Text = rightproject.description;

            // Fill ddlMemberGroupList with groups currently working on the project
            var projectgroupquery = ctx.project_group.ToList();
            List<group> groups_in_project = new List<group>();

            if (projectgroupquery != null)
            {
                foreach (var projectgroup in projectgroupquery)
                {
                    if (projectgroup.project_id == rightproject.project_id)
                    {
                        groups_in_project.Add( ctx.group.Where(g => g.group_id == projectgroup.group_id).SingleOrDefault());
                    }
                }

                foreach (var group in groups_in_project)
                {
                    ddlMemberGroupList.Items.Add(group.name);
                }
            }
            ddlMemberGroupList.Items.Insert(0, "Choose Group");

            List<group> projectgroups_perm = groups_in_project;
            Session["projectgroups"] = projectgroups_perm;

            // Fill ddlAssignmentList with groups currently working on the project
            List<assignment> projectassignments = rightproject.assignment.ToList();

            if (projectassignments != null)
            {
                foreach (var assignment in projectassignments)
                {
                    ddlAssignmentList.Items.Add(assignment.name);
                }
            }
            ddlAssignmentList.Items.Insert(0, "Choose Assignment");

            // Set project picture
            string pictureuri = Request.ApplicationPath + "Images/no_image.png";

            if (rightproject.picture_url != null)
            {
                if (rightproject.picture_url != String.Empty)
                {
                    imgProjectPicture.Src = rightproject.picture_url;
                }
            }
            else
            {
                imgProjectPicture.Src = pictureuri;
            }

            // Set due date to calendar
            if (rightproject.due_date != null)
            {
                calendarDueDate.SelectedDate = (DateTime)rightproject.due_date;
                calendarDueDate.VisibleDate = (DateTime)rightproject.due_date; 
            }
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void btnEditDescription_Click(object sender, EventArgs e)
    {
        if (txtProjectDescription.ReadOnly == true)
        {
            txtProjectDescription.ReadOnly = false;
            btnEditDescription.Text = "Save changes";
        }
        else if (txtProjectDescription.ReadOnly == false)
        {
            try
            {
                string project_id = Request.QueryString["Project"];
                int parsed_project_id = int.Parse(project_id);

                var rightproject = ctx.project.Where(pr => pr.project_id == parsed_project_id).SingleOrDefault();

                if (rightproject != null)
                {
                    rightproject.description = txtProjectDescription.Text;
                    rightproject.edited = DateTime.Now;

                    ctx.SaveChanges();
                }

                txtProjectDescription.Text = rightproject.description;
                txtProjectDescription.ReadOnly = true;
                btnEditDescription.Text = "Edit description";
            }
            catch (Exception ex)
            {
                
                lbMessages.Text = ex.Message;
            }
        }
    }

    #region TITLE_AND_PICTURE_CHANGE_BUTTONS
    protected void lbtnTriggerTitleChange_Click(object sender, EventArgs e)
    {
        // Change the visibility of certain html elements to bring up a textbox
        divDefault.Visible = false;
        divDuringChange.Visible = true;
        divTitleChanger.Visible = true;
    }

    protected void btnTitleChanger_Click(object sender, EventArgs e)
    {
        // Change The name of the project
        UserContentManager contentmanager = new UserContentManager(ticket.Name);

        try
        {
            string project_id = Request.QueryString["Project"];

            int parsed_project_id = int.Parse(project_id);
            bool is_same_name = false;

            var rightproject = ctx.project.Where(pr => pr.project_id == parsed_project_id).SingleOrDefault();

            // Check that the user does not already belong to a project with the same name
            // as the one they're trying to give
            List<project> userprojectpquery = contentmanager.GetUserProjects();

            foreach (var item in userprojectpquery)
            {
                if (item.name == txtTitleChanger.Text)
                {
                    is_same_name = true;
                }
            }

            if (rightproject != null && is_same_name == false)
            {
                rightproject.name = txtTitleChanger.Text;
                rightproject.edited = DateTime.Now;

                ctx.SaveChanges();

                h1ProjectName.InnerText = txtTitleChanger.Text;
                projectTitle.InnerText = txtTitleChanger.Text;
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
            string project_id = Request.QueryString["Project"];
            int parsed_project_id = int.Parse(project_id);
            bool gooduri = Uri.IsWellFormedUriString(txtImageChanger.Text, UriKind.Absolute);
            string pictureuri = Request.ApplicationPath + "Images/no_image.png";
            var rightproject = ctx.project.Where(g => g.project_id == parsed_project_id).SingleOrDefault();

            if (gooduri == true)
            {
                pictureuri = txtImageChanger.Text;
            }
            else
            {
                lbMessages.Text = "Enter valid absolute url path of the picture";
            }

            if (rightproject != null && txtImageChanger.Text != String.Empty)
            {
                rightproject.picture_url = pictureuri;
                rightproject.edited = DateTime.Now;

                ctx.SaveChanges();

                imgProjectPicture.Src = rightproject.picture_url;
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
    #endregion

    #region ASSIGNMENT_AND_GROUP_BUTTONS
    protected void btnAddGroup_Click(object sender, EventArgs e)
    {
        if (divSearch.Visible == false)
        {
            divSearch.Visible = true;
            btnAddGroup.Text = "Close search";
        }
        else
        {
            divSearch.Visible = false;
            btnAddGroup.Text = "Add groups";
        }
    }

    protected void btnSearchGroups_Click(object sender, EventArgs e)
    {
        // Search the database for the persons the user is searching
        string searchgroup = txtSearchGroups.Text;

        List<group> groupstoadd = SearchGroups(searchgroup);

        DataTable dt = new DataTable();

        dt.Columns.Add("groupname", typeof(string));
        dt.Columns.Add("creation_date", typeof(string));

        // Then fill the gridview with the results
        try
        {
            if (groupstoadd != null)
            {
                foreach (var group in groupstoadd)
                {
                    dt.Rows.Add(group.name, group.creation_date);
                    gvGroups.DataSource = dt;
                    gvGroups.DataBind();
                }
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void btnRemoveGroup_Click(object sender, EventArgs e)
    {
        RemoveProjectGroup();
    }

    protected void btnShowGroupInfo_Click(object sender, EventArgs e)
    {
        UserContentManager contentmanager = new UserContentManager(ticket.Name);

        string groupname = ddlMemberGroupList.SelectedValue;

        try
        {
            List<group> usergroups = contentmanager.GetUserGroups();

            if (usergroups != null)
            {
                foreach (var group in usergroups)
                {
                    if (groupname == group.name)
                    {
                        Response.Redirect(String.Format("Group.aspx?Group={0}", group.group_id));
                    }
                } 
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void btnCreateNewAssignment_Click(object sender, EventArgs e)
    {
        try
        {
            string project_id = Request.QueryString["Project"];

            Response.Redirect(String.Format("CreateAssignment.aspx?Project={0}", project_id));
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void btnShowAssignmentInfo_Click(object sender, EventArgs e)
    {
        if (ddlAssignmentList.SelectedIndex != 0)
        {
            try
            {
                var rightassignment = ctx.assignment.Where(a => a.amt_tag == ddlAssignmentList.SelectedIndex).SingleOrDefault();

                Response.Redirect(String.Format("Assignment.aspx?Assignment={0}", rightassignment.amt_id));
            }
            catch (Exception ex)
            {
                
                lbMessages.Text = ex.Message;
            }
        }
    }

    #endregion

    #region SEARCH_FUNCTIONS
    // Search the database for the persons the user is searching
    protected List<group> SearchGroups(string searchgroup)
    {
        bool add_to_grid = true;
        List<group> grouptoadd = new List<group>();

        try
        {
            var groups = ctx.group.ToList();

            if (groups != null && searchgroup != String.Empty)
            {
                foreach (var group in groups)
                {
                    add_to_grid = true;

                    for (int i = 0; i < searchgroup.Count(); i++)
                    {
                        if (searchgroup[i] != group.name[i] && group.name[i] != null)
                        {
                            add_to_grid = false;
                        }
                    }

                    if (add_to_grid == true)
                    {
                        grouptoadd.Add(group);
                    }
                }
            }

            return grouptoadd;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
            return null;
        }
    }

    protected void gvGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Add the selected member to the group
        string group_name = (gvGroups.SelectedRow.Cells[0].Controls[0] as LinkButton).Text;
        string project_id = String.Empty;


        // Fill the group_member database table as well
        try
        {
            project_id = Request.QueryString["Project"];

            int to_add_projectid = int.Parse(project_id);

            if (project_id != String.Empty)
            {
                var addedgroup = ctx.group.Where(g => g.name == group_name).FirstOrDefault();
                var addedproject = ctx.project.Where(pr => pr.project_id == to_add_projectid).FirstOrDefault();

                if (addedgroup != null && addedproject != null)
                {
                    var prg = new project_group
                    {
                        group_id = addedgroup.group_id,
                        project_id = addedproject.project_id,
                        supporting = true
                    };

                    addedproject.edited = DateTime.Now;
                    addedgroup.edited = DateTime.Now;

                    addedproject.project_group.Add(prg);
                    addedgroup.project_group.Add(prg);
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

        if (project_id != String.Empty)
        {
            Response.Redirect(String.Format(Request.ApplicationPath + "Project.aspx?Project={0}", project_id));
        }
    }
    #endregion

    #region REMOVAL_FUNCTIONS

    protected void RemoveProjectGroup()
    {
        try
        {
            string project_textid = Request.QueryString["Project"];
            int project_id = int.Parse(project_textid);

            string groupname = ddlMemberGroupList.SelectedItem.Text;
            int groupindex = ddlMemberGroupList.SelectedIndex;

            // Because a project can contain multiple groups with the same name we have to identify the right group to remove
            List<group> projectgroups_perm = (List<group>)Session["projectgroups"];
            group group_to_remove = new group();

            int counter = 0;
            foreach (var projectgroup in projectgroups_perm)
            {
                if (projectgroup.name == groupname && counter == groupindex - 1)
                {
                    group_to_remove = projectgroup;
                }
                counter++;
            }

            var rightprojectgroup = ctx.project_group.Where(pg => pg.project_id == project_id && pg.group_id == group_to_remove.group_id).SingleOrDefault();

            if (rightprojectgroup != null)
            {
                ctx.project_group.Remove(rightprojectgroup);
                ctx.SaveChanges();

                Response.Redirect(String.Format(Request.ApplicationPath + "Project.aspx?Project={0}", project_id));
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    #endregion
}