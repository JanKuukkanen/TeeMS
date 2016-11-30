using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeeMs.Encoder;
using TeeMs.CookieManager;
using System.Web.Security;

public partial class login : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void loginUser_Authenticate(object sender, AuthenticateEventArgs e)
    {
        if (loginUser.UserName != string.Empty && loginUser.Password != string.Empty)
        {
            if (AuthenticateUser(loginUser.UserName, loginUser.Password))
            {
                // Redirect user to home.aspx
                FormsAuthentication.SetAuthCookie(loginUser.UserName, true);
                Server.Transfer("Home.aspx", true);
            }
        }
    }

    protected Boolean AuthenticateUser(string uname, string password)
    {
        try
        {
            // Query for the person with a username similar to the users input
            Encoder encoder = new Encoder();

            if (encoder.AuthenticateUser(uname, password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
            return false;
        }
    }
}