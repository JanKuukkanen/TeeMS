﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TeeMs.UserContentManager;

public partial class CreateProject : System.Web.UI.Page
{
    private TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        // set the database context using entity framework
        ctx = new TeeMsEntities();

        try
        {
            HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);
        }
        catch (HttpException ex)
        {
            
            lbmessages.Text = ex.Message;
        }
        
        if (!IsPostBack)
        {
            FillControls();
        }
    }

    protected void FillControls()
    {
        try
        {
            // Fill the dropdownlist with the groups the current user belongs to
            UserContentManager contentmanager = new UserContentManager(ticket.Name);

            List<group> groupquery = contentmanager.GetUserGroups();

            if (groupquery != null)
            {
                foreach (var group in groupquery)
                {
                    ddlGroupList.Items.Add(group.name);
                }
            }
            ddlGroupList.Items.Insert(0, "Choose Group");
        }
        catch (Exception ex)
        {

            lbmessages.Text = ex.Message;
        }
    }

    protected void btnCreateProject_Click(object sender, EventArgs e)
    {
        int project_id = CreateNewProject();

        if ((ddlGroupList.SelectedItem.Text != ddlGroupList.Items[0].Text) && (project_id != -1))
        {
            CreateProjectGroup(project_id); 
        }

        if (project_id != -1)
        {
            Response.Redirect("Project.aspx?Project=" + project_id.ToString()); 
        }
    }

    protected int CreateNewProject()
    {
        // Initialize usercontentmanager class with the users username
        UserContentManager contentmanager = new UserContentManager(ticket.Name);

        string projectname = txtProjectName.Text;
        string projectdesc = txtProjectDescription.Text;
        string projectcreator = ticket.Name;
        string grouptoaddname = String.Empty;
        double percent = 0;
        string applicationpath = Request.ApplicationPath;
        string pictureuri = applicationpath + "Images/no_image.png";
        List<group> groupquery = new List<group>();
        bool is_same_name = false;

        bool gooduri = Uri.IsWellFormedUriString(txtPictureURI.Text, UriKind.Absolute);

        if (gooduri == true)
        {
            pictureuri = txtPictureURI.Text;
        }
        else
        {
            lbmessages.Text = "Enter valid absolute url path of the picture";
        }

        if (ddlGroupList.SelectedItem.Text != ddlGroupList.Items[0].Text)
        {
            grouptoaddname = ddlGroupList.SelectedItem.Text;

            groupquery = contentmanager.GetUserGroups();
        }

        DateTime duedate = calendarDueDate.SelectedDate;

        try
        {
            var rightperson = ctx.person.Where(p => p.username == projectcreator).FirstOrDefault();

            // Check that the user does not already have a project with the same name
            List<project> userprojects = contentmanager.GetUserProjects();

            foreach (var project in userprojects)
            {
                if (project.name == projectname)
                {
                    is_same_name = true;
                }
            }

            if (is_same_name == false)
            {
                // Search the right group given in the dropdownlist
                if (grouptoaddname != String.Empty)
                {
                    var rightgroup = ctx.group.Where(g => g.name == grouptoaddname);
                }

                int emptyrow = 0;
                int addedrow = 0;

                // First check for any gaps in the number sequence inside the database
                for (int i = 0; i <= ctx.project.Count(); i++)
                {
                    project compareProject = new project();
                    var checkProject = ctx.project.Where(gr => gr.project_id == i).FirstOrDefault();

                    if (checkProject == compareProject)
                    {
                        emptyrow = i;
                    }
                    else
                    {
                        compareProject = checkProject;
                    }
                }

                if (emptyrow == 0)
                {
                    addedrow = ctx.project.Count() + 1;
                }
                else
                {
                    addedrow = emptyrow;
                }

                // Insert new project into database
                var newproject = new project
                {
                    name = projectname,
                    description = projectdesc,
                    project_creator = rightperson.username,
                    creation_date = DateTime.Now,
                    due_date = duedate,
                    percent_done = percent,
                    finished = false,
                    privacy = 1,
                    picture_url = pictureuri
                };

                var projectlist = ctx.project.ToList();

                if (projectlist != null)
                {
                    int tagindex = 100;

                    foreach (var project in projectlist)
                    {
                        tagindex = (int)project.project_tag + 1;
                    }

                    newproject.project_tag = tagindex;
                }
                else
                {
                    newproject.project_tag = 100;
                }

                ctx.project.Add(newproject);
                ctx.SaveChanges();
                return newproject.project_id;
            }
            else
            {
                lbmessages.Text = "Error: You already have a project with that name";
                return -1;
            }
        }
        catch (Exception ex)
        {

            lbmessages.Text = ex.Message;
            return -1;
        }
    }

    protected void CreateProjectGroup(int project_id)
    {
        try
        {
            // Add the selected group and project to a new project group
            var rightgroup = ctx.group.Where(g => g.name == ddlGroupList.SelectedItem.Text).FirstOrDefault();
            var rightproject = ctx.project.Where(pr => pr.project_id == project_id).FirstOrDefault();

            if (rightgroup != null && rightproject != null)
            {
                var projectgroup = new project_group
                    {
                        group_id = rightgroup.group_id,
                        project_id = rightproject.project_id,
                        supporting = false
                    };

                rightgroup.project_group.Add(projectgroup);
                rightproject.project_group.Add(projectgroup);
                ctx.SaveChanges(); 
            }

            // Insert a new project_person for each member of the selected group
            if (rightgroup != null && rightproject != null)
            {
                var groupmember_ids = ctx.group_member.Where(gm => gm.group_id == rightgroup.group_id).ToList();
                List<person> groupmembers = new List<person>();

                foreach (var groupmember in groupmember_ids)
                {
                    groupmembers.Add(ctx.person.Where(p => p.person_id == groupmember.person_id).SingleOrDefault());
                }

                foreach (var member in groupmembers)
                {
                    var prope = new project_person
                    {
                        project_id = rightproject.project_id,
                        person_id = member.person_id,
                        group_id = rightgroup.group_id,
                        project_person_supporting = false
                    };

                    member.edited = DateTime.Now;
                    member.project_person.Add(prope);
                    rightproject.project_person.Add(prope);
                }

                rightproject.edited = DateTime.Now;

                ctx.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            
            lbmessages.Text = ex.Message;
        }
    }
}