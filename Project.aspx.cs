using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TeeMs.UserContentManager;

public partial class Project : System.Web.UI.Page
{
    private TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        ctx = new TeeMsEntities();

        string project_id = Request.QueryString["Project"];

        try
        {
            HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);
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
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
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
    protected void btnShowGroupInfo_Click(object sender, EventArgs e)
    {

    }

    protected void btnCreateNewAssignment_Click(object sender, EventArgs e)
    {

    }

    protected void btnShowAssignmentInfo_Click(object sender, EventArgs e)
    {

    }
    #endregion
}