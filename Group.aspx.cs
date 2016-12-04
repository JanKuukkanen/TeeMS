using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TeeMs.UserContentManager;

public partial class Group : System.Web.UI.Page
{
    private TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        ctx = new TeeMsEntities();

        string group_id = Request.QueryString["Group"];

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
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

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
}