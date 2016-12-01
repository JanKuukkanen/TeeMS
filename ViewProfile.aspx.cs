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
        ctx = new TeeMsEntities();

        HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        ticket = FormsAuthentication.Decrypt(authcookie.Value);

        FillLabels(); 
    }

    protected void FillLabels()
    {
        try
        {
            string cookieusername = ticket.Name;
            var correctperson = ctx.person.Where(p => p.username == cookieusername).FirstOrDefault();

            lbUsernameInsert.Text = correctperson.username;
            lbEmailInsert.Text = correctperson.email;
            lbFirstNameInsert.Text = correctperson.first_name;
            lbLastNameInsert.Text = correctperson.last_name;
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
        try
        {
            var rightperson = ctx.person.Where(p => p.username == ticket.Name).FirstOrDefault();
            var rightlogin = ctx.login.Where(l => l.login_name == ticket.Name).FirstOrDefault();

            if (rightperson != null && rightlogin != null)
            {
                string uname = rightperson.username;
                string fname = rightperson.first_name;
                string lname = rightperson.last_name;
                string email = rightperson.email;
                bool ischanged = false;

                if (txtUserName.Text != String.Empty)
                {
                    uname = txtUserName.Text;
                    rightperson.username = uname;
                    rightlogin.login_name = uname;
                    ischanged = true;
                }

                if (txtFirstName.Text != String.Empty)
                {
                    fname = txtFirstName.Text;
                    rightperson.first_name = fname;
                    ischanged = true;
                }

                if (txtLastName.Text != String.Empty)
                {
                    lname = txtLastName.Text;
                    rightperson.last_name = lname;
                    ischanged = true;
                }

                if (txtEmail.Text != String.Empty)
                {
                    email = txtEmail.Text;
                    rightperson.email = email;
                    ischanged = true;
                }


                if (ischanged == true)
                {
                    rightperson.edited = DateTime.Now;

                    int num = ctx.SaveChanges();

                    FormsAuthentication.SetAuthCookie(uname, true);

                    lbErrorMessages.Text = String.Format("Updated {0} database entries", num.ToString());

                    lbUsernameInsert.Text = uname;
                    lbEmailInsert.Text = email;
                    lbFirstNameInsert.Text = fname;
                    lbLastNameInsert.Text = lname;
                }
                else if (ischanged == false)
                {
                    lbErrorMessages.Text = "Please insert the data you wish to change in the correct textbox";
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
        try
        {
            Encoder encoder = new Encoder();

            var rightperson = ctx.person.Where(p => p.username == ticket.Name).FirstOrDefault();
            var rightlogin = ctx.login.Where(l => l.login_name == ticket.Name).FirstOrDefault();

            if (rightperson != null && rightlogin != null)
            {
                string password = rightlogin.password;

                string newsalt = encoder.GetSalt();
                string newpassword = encoder.GenerateSaltedHash(txtPassword.Text, newsalt);

                rightlogin.password = newpassword;
                rightlogin.salt = newsalt;
                rightperson.edited = DateTime.Now;

                int num = ctx.SaveChanges();

                lbErrorMessages.Text = String.Format("Updates {0} database entries", num.ToString());
            }
        }
        catch (Exception ex)
        {

            lbErrorMessages.Text = ex.Message;
        }
    }
}