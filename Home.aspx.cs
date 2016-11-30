using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Home : System.Web.UI.Page
{

    TeeMsEntities ctx;
    FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        ticket = FormsAuthentication.Decrypt(authcookie.Value);
    }

    protected void btnCreateGroup_Click(object sender, EventArgs e)
    {
        int group_id = CreateNewGroup();
        GroupMemberConnection(group_id);
    }

    #region CREATEMETHODS
    protected int CreateNewGroup()
    {
        ctx = new TeeMsEntities();
        int emptyrow = 0;
        int addedrow = 0;

        // First check for any gaps in the number sequence inside the database
        for (int i = 0; i <= ctx.group.Count(); i++)
        {
            group compareGroup = new group();
            var checkGroup = ctx.group.Where(gr => gr.group_tag == i).SingleOrDefault();

            if (checkGroup == compareGroup)
            {
                emptyrow = i;
            }
            else
            {
                compareGroup = checkGroup;
            }
        }

        if (emptyrow == 0)
        {
            addedrow = ctx.group.Count() + 1;
        }
        else
        {
            addedrow = emptyrow;
        }

        try
        {
            var g = new group
            {
                name = "New Group",
                group_tag = addedrow,
                creator = ticket.Name,
                privacy = 1,
                creation_date = DateTime.Now,
            };
            ctx.group.Add(g);
            ctx.SaveChanges();
            return g.group_id;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
            return 0;
        }
    }

    protected void GroupMemberConnection(int group_id)
    {
        // There is a Many to Many relationship between a person and the group
        // so we need to fill the extra database table as well
        
    }
    #endregion
}