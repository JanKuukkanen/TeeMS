﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text.RegularExpressions;
using AjaxControlToolkit;
using TeeMs.UserContentManager;

public partial class Project : System.Web.UI.Page
{
    private TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;
    

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
        else if (IsPostBack)
        {
            FillComments();
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

            Session["ProjectAssignments"] = projectassignments;

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

            FillComments();
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
                string project_id = Request.QueryString["Project"];
                int index = ddlAssignmentList.SelectedIndex;

                List<assignment> projectassignments = (List<assignment>)Session["ProjectAssignments"];

                var rightassignment = projectassignments[index - 1];

                if (rightassignment.name == ddlAssignmentList.Items[index].Text)
                {
                    Response.Redirect(String.Format("Assignment.aspx?Assignment={0}&Project={1}", rightassignment.amt_id, project_id)); 
                }
            }
            catch (Exception ex)
            {
                
                lbMessages.Text = ex.Message;
            }
        }
    }

#endregion

#region SEARCH_FUNCTIONS

    // Search the database for the group the user is searching
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

            Session["SearchListGroups"] = grouptoadd;

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
        // Add the selected group to the project
        string group_name = (gvGroups.SelectedRow.Cells[0].Controls[0] as LinkButton).Text;
        string project_id = String.Empty;


        // Fill the project_group database table as well
        try
        {
            List<group> searchlistgroups = (List<group>)Session["SearchListGroups"];
            project_id = Request.QueryString["Project"];

            int to_add_projectid = int.Parse(project_id);

            if (project_id != String.Empty)
            {
                var addedgroup = ctx.group.Where(g => g.name == group_name).SingleOrDefault();
                var addedproject = ctx.project.Where(pr => pr.project_id == to_add_projectid).SingleOrDefault();

                bool checklegit = false;

                foreach (var group in searchlistgroups)
                {
                    if (group.group_id == addedgroup.group_id)
                    {
                        checklegit = true;
                    }
                }

                // Add the selected group and project to a new project group
                if (addedgroup != null && addedproject != null && checklegit == true)
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
                }

                // Insert a new project_person for each member of the selected group
                if (addedgroup != null && addedproject != null && checklegit == true)
                {
                    var groupmember_ids = ctx.group_member.Where(gm => gm.group_id == addedgroup.group_id).ToList();
                    List<person> groupmembers = new List<person>();

                    foreach (var groupmember in groupmember_ids)
                    {
                        groupmembers.Add(ctx.person.Where(p => p.person_id == groupmember.person_id).SingleOrDefault());
                    }

                    foreach (var member in groupmembers)
                    {
                        var prope = new project_person
                        {
                            project_id = addedproject.project_id,
                            person_id = member.person_id,
                            group_id = addedgroup.group_id,
                            project_person_supporting = true
                        };

                        member.edited = DateTime.Now;
                        member.project_person.Add(prope);
                        addedproject.project_person.Add(prope);
                    }

                    addedproject.edited = DateTime.Now;

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
            var rightgroupmembers = ctx.group_member.Where(gm => gm.group_id == group_to_remove.group_id).ToList();

            if (rightgroupmembers != null)
            {
                foreach (var member in rightgroupmembers)
                {
                    var rightperson = ctx.person.Where(p => p.person_id == member.person_id).SingleOrDefault();

                    var rightprojectperson = ctx.project_person.Where(prope => prope.person_id == rightperson.person_id && prope.project_id == project_id).SingleOrDefault();

                    ctx.project_person.Remove(rightprojectperson);
                }
            }

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

#region COMMENT_FUNCTIONS

    protected void btnSaveComment_Click(object sender, EventArgs e)
    {
        string comment = txtWriteComment.Text;

        try
        {
            if (comment != String.Empty)
            {
                int project_id = int.Parse(Request.QueryString["Project"]);
                string username = ticket.Name;

                var rightperson = ctx.person.Where(p => p.username == username).SingleOrDefault();

                comment newcomment = new comment
                {
                    comment_content = comment,
                    creation_date = DateTime.Now,
                    person_id = rightperson.person_id,
                    project_id = project_id
                };

                ctx.comment.Add(newcomment);
                ctx.SaveChanges();

                txtWriteComment.Text = String.Empty;
                FillComments();
            }
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void FillComments()
    {
        try
        {
            divProjectCommentMessages.Controls.Clear();

            string username = ticket.Name;
            var rightperson = ctx.person.Where(p => p.username == username).SingleOrDefault();

            var commentlist = ctx.comment.Where(co => co.amt_id == null).ToList();
            int project_id = int.Parse(Request.QueryString["Project"]);

            List<HtmlGenericControl> divlist = new List<HtmlGenericControl>();

            foreach (var comment in commentlist)
            {
                if (comment.project_id == project_id)
                {
                    HtmlGenericControl commentdiv = new HtmlGenericControl("div");
                    HtmlGenericControl commentcontentdiv = new HtmlGenericControl("div");
                    HtmlGenericControl commentbuttonsdiv = new HtmlGenericControl("div");
                    HtmlGenericControl commenttextboxdiv = new HtmlGenericControl("div");
                    Label usernamelabel = new Label();

                    TextBox commentedittxt = new TextBox();
                    HtmlGenericControl br = new HtmlGenericControl("br");
                    LinkButton commentsavebutton = new LinkButton();
                    LinkButton commentcancelbutton = new LinkButton();

                    LinkButton commenteditbutton = new LinkButton();
                    LinkButton commentdeletebutton = new LinkButton();

                    commentdiv.ID = String.Format("commentdiv{0}", comment.comment_id);
                    commentdiv.Attributes.Add("Style", "border: thin solid black; margin-top:10px;");

                    commenttextboxdiv.ID = String.Format("commenttextboxdiv{0}", comment.comment_id);
                    commenttextboxdiv.Attributes.Add("Style", "border: thin solid black; margin-top:10px;");
                    commenttextboxdiv.Visible = false;

                    usernamelabel.Text = String.Format("Posted by {0} on {1}", comment.person.username, comment.creation_date.ToString("d/M/yyyy HH:mm"));
                    usernamelabel.Font.Bold = true;
                    commentcontentdiv.InnerText = comment.comment_content;

                    commentedittxt.ID = String.Format("txt{0}", comment.comment_id);
                    commentedittxt.Text = comment.comment_content;
                    commentedittxt.TextMode = TextBoxMode.MultiLine;
                    commentedittxt.Width = 400;
                    commentedittxt.Height = 100;

                    commentsavebutton.ID = String.Format("Savebtn{0}", comment.comment_id);
                    commentsavebutton.Text = "Save Changes";
                    commentsavebutton.CommandArgument = "Hello";
                    commentsavebutton.Command += new CommandEventHandler(SaveCommentClick);
                    commentsavebutton.CssClass = "w3-btn";

                    commentcancelbutton.ID = String.Format("Cancelbtn{0}", comment.comment_id);
                    commentcancelbutton.Text = "Cancel";
                    commentcancelbutton.CommandArgument = "Hello";
                    commentcancelbutton.Command += new CommandEventHandler(CancelCommentClick);
                    commentcancelbutton.CssClass = "w3-btn";
                    commentcancelbutton.Attributes.Add("style", "margin-left:10px;");

                    commenteditbutton.ID = String.Format("lbtnEditComment{0}", comment.comment_id);
                    commenteditbutton.Text = "Edit";
                    commenteditbutton.CommandArgument = "Hello";
                    commenteditbutton.Command += new CommandEventHandler(EditCommentClick);
                    commenteditbutton.CssClass = "CommentLinkButtons";

                    commentdeletebutton.ID = String.Format("lbtnDeleteComment{0}", comment.comment_id);
                    commentdeletebutton.Text = "Delete";
                    commentdeletebutton.CommandArgument = "Hello";
                    commentdeletebutton.Command += new CommandEventHandler(DeleteCommentClick);
                    commentdeletebutton.CssClass = "CommentLinkButtons";
                    commentdeletebutton.Attributes.Add("style", "margin-left:10px;");

                    commentdiv.Controls.Add(usernamelabel);
                    commentdiv.Controls.Add(commentcontentdiv);

                    if (comment.person_id == rightperson.person_id)
                    {
                        commenttextboxdiv.Controls.Add(commentedittxt);
                        commenttextboxdiv.Controls.Add(br);
                        commenttextboxdiv.Controls.Add(commentsavebutton);
                        commenttextboxdiv.Controls.Add(commentcancelbutton);

                        commentbuttonsdiv.Controls.Add(commenteditbutton);
                        commentbuttonsdiv.Controls.Add(commentdeletebutton); 
                    }

                    divProjectCommentMessages.Controls.Add(commentdiv);
                    divProjectCommentMessages.Controls.Add(commenttextboxdiv);
                    divProjectCommentMessages.Controls.Add(commentbuttonsdiv);

                    divlist.Add(commentdiv);
                    divlist.Add(commenttextboxdiv);
                }
            }

            Session["Divs"] = divlist;
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void EditCommentClick(object sender, CommandEventArgs e)
    {
        // Parse the correct comment id from the linkbutton's id
        var btn = sender as LinkButton;
        string parseid = Regex.Match(btn.ID, @"-?\d+\.?\d*$").Value;
        int commentid = int.Parse(parseid);

        try
        {
            var divlist = (List<HtmlGenericControl>)Session["Divs"];

            var rightcommentdiv = divlist.Where(div => div.ID == String.Format("commentdiv{0}", commentid)).SingleOrDefault();
            var rightcommenttextboxdiv = divlist.Where(div => div.ID == String.Format("commenttextboxdiv{0}", commentid)).SingleOrDefault();

            rightcommenttextboxdiv.Visible = true;
            rightcommentdiv.Visible = false;
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void DeleteCommentClick(object sender, CommandEventArgs e)
    {
        // Parse the correct comment id from the linkbutton's id
        var btn = sender as LinkButton;
        string parseid = Regex.Match(btn.ID, @"-?\d+\.?\d*$").Value;
        int commentid = int.Parse(parseid);

        try
        {
            string username = ticket.Name;
            var rightperson = ctx.person.Where(p => p.username == username).SingleOrDefault();
            var rightcomment = ctx.comment.Where(com => com.comment_id == commentid).SingleOrDefault();

            if (rightcomment.person_id == rightperson.person_id)
            {
                ctx.comment.Remove(rightcomment);
                ctx.SaveChanges();

                FillComments();
            }
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void SaveCommentClick(object sender, CommandEventArgs e)
    {
        // Parse the correct comment id from the linkbutton's id
        var btn = sender as LinkButton;
        string parseid = Regex.Match(btn.ID, @"-?\d+\.?\d*$").Value;
        int commentid = int.Parse(parseid);

        try
        {
            string username = ticket.Name;
            var rightperson = ctx.person.Where(p => p.username == username).SingleOrDefault();
            var rightcomment = ctx.comment.Where(com => com.comment_id == commentid).SingleOrDefault();

            if (rightcomment.person_id == rightperson.person_id)
            {
                var divlist = (List<HtmlGenericControl>)Session["Divs"];

                var rightcommentdiv = divlist.Where(div => div.ID == String.Format("commentdiv{0}", commentid)).SingleOrDefault();
                var rightcommenttextboxdiv = divlist.Where(div => div.ID == String.Format("commenttextboxdiv{0}", commentid)).SingleOrDefault();

                TextBox divtxt = rightcommenttextboxdiv.Controls[0] as TextBox;

                string newcontent = divtxt.Text;

                rightcomment.comment_content = newcontent;
                rightcomment.edited = DateTime.Now;

                ctx.SaveChanges();

                rightcommenttextboxdiv.Visible = false;
                rightcommentdiv.Visible = true;

                FillComments();
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void CancelCommentClick(object sender, CommandEventArgs e)
    {
        // Parse the correct comment id from the linkbutton's id
        var btn = sender as LinkButton;
        string parseid = Regex.Match(btn.ID, @"-?\d+\.?\d*$").Value;
        int commentid = int.Parse(parseid);

        try
        {
            var divlist = (List<HtmlGenericControl>)Session["Divs"];

            var rightcommentdiv = divlist.Where(div => div.ID == String.Format("commentdiv{0}", commentid)).SingleOrDefault();
            var rightcommenttextboxdiv = divlist.Where(div => div.ID == String.Format("commenttextboxdiv{0}", commentid)).SingleOrDefault();

            rightcommenttextboxdiv.Visible = false;
            rightcommentdiv.Visible = true;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

#endregion
    
}