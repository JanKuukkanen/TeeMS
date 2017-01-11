using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Collections.Specialized;
using TeeMs.UserContentManager;

public partial class Assignment : System.Web.UI.Page
{
    TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        ctx = new TeeMsEntities();

        string project_id = "Hello";
        string assignment_id = "Double Hello";

        try
        {
            HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);

            assignment_id = Request.QueryString["Assignment"];
            project_id = Request.QueryString["Project"];
        }
        catch (HttpException ex)
        {

            lbMessages.Text = ex.Message;
        }

        if (!IsPostBack)
        {
            try
            {
                FillControls(int.Parse(project_id), int.Parse(assignment_id));
            }
            catch (Exception ex)
            {
                
                lbMessages.Text = ex.Message;
            }
        }
    }

    protected void FillControls(int project_id, int assignment_id)
    {
        try
        {
            // Set the correct assignment name to the title and header
            var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();
            var rightassignment = ctx.assignment.Where(amt => amt.project_id == project_id && amt.amt_id == assignment_id).SingleOrDefault();

            titleAssignmentTitle.InnerText = rightassignment.name;
            h1AssignmentName.InnerText = rightassignment.name;
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }
}