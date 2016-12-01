using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TeeMs.Encoder;

public partial class ViewProfile : System.Web.UI.Page
{
    private TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        ticket = FormsAuthentication.Decrypt(authcookie.Value);

        if (!IsPostBack)
        {
            FillLabels(); 
        }
    }

    protected void FillLabels()
    {
        try
        {
            ctx = new TeeMsEntities();

            var rightperson = ctx.person.Where(p => p.username == ticket.Name).SingleOrDefault();

            lbUsernameInsert.Text = rightperson.username;
            lbEmailInsert.Text = rightperson.email;
            lbFirstNameInsert.Text = rightperson.first_name;
            lbLastNameInsert.Text = rightperson.last_name;
        }
        catch (Exception ex)
        {

            lbErrorMessages.Text = ex.Message;
        }
    }

    protected void btnEditProfile_Click(object sender, EventArgs e)
    {
        UpdateUserProfile();
    }

    protected void btnEditPassword_Click(object sender, EventArgs e)
    {
        UpdateUserPassword();
    }

    protected void UpdateUserProfile()
    {
        ctx = new TeeMsEntities();

        try
        {
            var rightperson = ctx.person.Where(p => p.username == ticket.Name).FirstOrDefault();
            var rightlogin = ctx.login.Where(l => l.login_name == ticket.Name).FirstOrDefault();

            if (rightperson != null && rightlogin != null)
            {
                var changedperson = ctx.person.Where(p => p.person_id == rightperson.person_id).FirstOrDefault();

                string uname = txtUserName.Text;
                string fname = txtFirstName.Text;
                string lname = txtLastName.Text;
                string email = txtEmail.Text;

                if (changedperson != null)
                {
                    changedperson.username = uname;
                    changedperson.first_name = fname;
                    changedperson.last_name = lname;
                    changedperson.email = email;
                    changedperson.edited = DateTime.Now;

                    int num = ctx.SaveChanges();

                    lbErrorMessages.Text = String.Format("Updated {0} database entries", num.ToString());
                }
            }
            else
            {
                lbErrorMessages.Text = "Something went wrong in the database transaction";
            }
        }
        catch (Exception ex)
        {

            lbErrorMessages.Text = ex.Message;
        }
    }

    protected void UpdateUserPassword()
    {
        Encoder encoder = new Encoder();
    }
}