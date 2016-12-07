using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TeeMs.UserContentManager;

public partial class CreateAssignment : System.Web.UI.Page
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
            // Fill the dropdownlist with the users from the groups currently working
            // on the project the assignment will be a part of
            UserContentManager contentmanager = new UserContentManager(ticket.Name);
            string project_id = Request.QueryString["Project"];

            List<person> userquery = contentmanager.GetProjectUsers(project_id);

            lbmessages.Text = userquery.Count.ToString();

            if (userquery != null)
            {
                foreach (var user in userquery)
                {
                    ddlUserList.Items.Add(user.username);
                }
            }
            ddlUserList.Items.Insert(0, "Choose User");
        }
        catch (Exception ex)
        {

            lbmessages.Text = ex.Message;
        }
    }

    protected void btnCreateAssignment_Click(object sender, EventArgs e)
    {

    }
}