using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using TeeMs.UserContentManager;

public partial class Projects : System.Web.UI.Page
{

    private TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);
        }
        catch (HttpException ex)
        {

            lbMessages.Text = ex.Message;
        }

        FillDivs();
    }

    protected void FillDivs()
    {
        // Fill your projects section with the users projects and assignments
        // and fill your groups with the users groups
        UserContentManager contentmanager = new UserContentManager(ticket.Name);

        try
        {
            List<group> usergroups = contentmanager.GetUserGroups();
            List<project> userprojects = contentmanager.GetUserProjects();

            foreach (var project in userprojects)
            {

                var projectassignments = project.assignment.ToList(); 

                HtmlGenericControl projectdiv = new HtmlGenericControl("div");
                HtmlGenericControl projectinnerdiv = new HtmlGenericControl("div");
                HtmlGenericControl projectassignmentdiv = new HtmlGenericControl("div");
                HtmlGenericControl projectassignmentinnerdiv = new HtmlGenericControl("div");
                HtmlGenericControl projectlink = new HtmlGenericControl("a");
                HtmlGenericControl projectimage = new HtmlGenericControl("img");
                HtmlGenericControl projectcut = new HtmlGenericControl("hr");
                HtmlGenericControl projectassignmenttitle = new HtmlGenericControl("h3");
                HtmlGenericControl projectassignmentul = new HtmlGenericControl("ul");

                projectdiv.Attributes.Add("class", "w3-container");
                projectdiv.ID = "divProject" + project.project_id;

                projectinnerdiv.Attributes.Add("class", "w3-container");

                projectimage.Attributes.Add("src", project.picture_url);
                projectimage.Attributes.Add("alt", "Project image");
                projectimage.Attributes.Add("height", "150px");
                projectimage.Attributes.Add("width", "150px");
                projectimage.Attributes.Add("style", "float:left;");

                projectinnerdiv.Controls.Add(projectimage);

                projectlink.Attributes.Add("href", String.Format("Project.aspx?Project={0}", project.project_id));
                projectlink.Attributes.Add("style", "float:left; margin-top:5%; margin-left:5%;");
                projectlink.InnerText = project.name;

                projectinnerdiv.Controls.Add(projectlink);

                projectcut.Attributes.Add("style", "color:#000;background-color:#000; height:5px;");

                projectassignmentdiv.Attributes.Add("class", "w3-container");

                projectassignmenttitle.InnerText = "Assignments";

                projectassignmentdiv.Controls.Add(projectassignmenttitle);

                projectassignmentinnerdiv.Attributes.Add("style", "width:15%;");

                projectassignmentul.Attributes.Add("class", "w3-ul w3-border");
                projectassignmentul.Attributes.Add("id", "projectassignmentsul");

                foreach (var assignment in projectassignments)
                {
                    HtmlGenericControl projectassignmentli = new HtmlGenericControl("li");
                    HtmlGenericControl projectassignmentlink = new HtmlGenericControl("a");

                    projectassignmentlink.Attributes.Add("href", String.Format("Assignment.aspx?Assignment={0}&Project={1}", assignment.amt_id, project.project_id));
                    projectassignmentlink.InnerText = assignment.name;

                    projectassignmentli.Controls.Add(projectassignmentlink);
                    projectassignmentul.Controls.Add(projectassignmentli);
                }

                projectassignmentinnerdiv.Controls.Add(projectassignmentul);
                projectassignmentdiv.Controls.Add(projectassignmentinnerdiv);

                projectdiv.Controls.Add(projectinnerdiv);
                projectdiv.Controls.Add(projectassignmentdiv);
                projectdiv.Controls.Add(projectcut);

                divYourProjects.Controls.Add(projectdiv);
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }
}