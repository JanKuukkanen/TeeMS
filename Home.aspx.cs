using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TeeMs.UserContentManager;

public partial class Home : System.Web.UI.Page
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
    }

    protected void btnCreateGroup_Click(object sender, EventArgs e)
    {
        int group_id = CreateNewGroup();
        GroupMemberConnection(group_id);
        Response.Redirect("Group.aspx?Group=" + group_id.ToString());
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

        // Next add a new group to the database

        try
        {
            UserContentManager contentmanager = new UserContentManager(ticket.Name);
            List<group> usergroupquery = contentmanager.GetUserGroups();
            int newgroupnamenro = 1;

            if (usergroupquery != null)
            {
                for (int i = 0; i < usergroupquery.Count; i++)
                {
                    if (usergroupquery[i].name == String.Format("New Group {0}", usergroupquery.Count + newgroupnamenro))
                    {
                        newgroupnamenro = newgroupnamenro + 1;
                        i = 0;
                    }
                } 
            }

            string newgroupname = String.Format("New Group {0}", usergroupquery.Count + newgroupnamenro);

            var g = new group
            {
                name = newgroupname,
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

    protected void GroupMemberConnection(int addgroup_id)
    {
        // There is a Many to Many relationship between a person and the group
        // so we need to fill the group_member database table as well

        try
        {
            var addeduser = ctx.person.Where(p => p.username == ticket.Name).FirstOrDefault();
            var addedgroup = ctx.group.Where(g => g.group_id == addgroup_id).FirstOrDefault();
            var addedrole = ctx.group_role.Where(gr => gr.@class == 1).FirstOrDefault();

            if (addeduser != null && addedgroup != null && addedrole != null)
            {
                var gm = new group_member
                    {
                        group_id = addedgroup.group_id,
                        person_id = addeduser.person_id,
                        grouprole_id = addedrole.grouprole_id
                    };

                addeduser.group_member.Add(gm);
                addedgroup.group_member.Add(gm);
                ctx.SaveChanges(); 
            }
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }
    #endregion
}