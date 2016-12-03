using System;
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
        UserContentManager contentmanager = new UserContentManager(ticket.Name);

        string projectname = txtProjectName.Text;
        string projectdesc = txtProjectDescription.Text;
        string projectcreator = ticket.Name;
        string grouptoaddname = String.Empty;

        if (ddlGroupList.SelectedItem.Text != ddlGroupList.Items[0].Text)
        {
            grouptoaddname = ddlGroupList.SelectedItem.Text;

            List<group> groupquery = contentmanager.GetUserGroups();
        }

        string duedate = calendarDueDate.SelectedDate.ToShortDateString();

        try
        {
            var rightperson = ctx.person.Where(p => p.username == projectcreator).FirstOrDefault();

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
                var checkProject = ctx.project.Where(gr => gr.project_tag == i).SingleOrDefault();

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

            // Still missing: Due date, picture url. Change the percent_done in the database to DOUBLE
            var newproject = new project
            {
                name = projectname,
                description = projectdesc,
                project_tag = addedrow,
                project_creator = rightperson.username,
                creation_date = DateTime.Now,

                privacy = 1,
            };
        }
        catch (Exception ex)
        {

            lbmessages.Text = ex.Message;
        }
    }
}