using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SignOut : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["AuthCookie"] != null)
        {
            HttpCookie endCookie = new HttpCookie("AuthCookie");
            endCookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(endCookie);

            Server.Transfer("Login.aspx", true);
        }
    }
}