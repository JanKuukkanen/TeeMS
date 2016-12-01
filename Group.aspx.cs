using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Group : System.Web.UI.Page
{
    private TeeMsEntities ctx;

    protected void Page_Load(object sender, EventArgs e)
    {
        string group_id = Request.QueryString["Group"];

        FillControls(int.Parse(group_id));
    }

    protected void FillControls(int group_id)
    {
        try
        {
            // Set the titles of the page to be the name of the group
            ctx = new TeeMsEntities();

            var rightgroup = ctx.group.Where(g => g.group_id == group_id).SingleOrDefault();

            groupTitle.InnerText = rightgroup.name;
            h1GroupName.InnerText = rightgroup.name;

            var groupMembers = rightgroup.group_member.Where(gm => gm.group_id == group_id).Select(gm => gm.person_id).ToList();

            foreach (var item in groupMembers)
	        {
                var person = ctx.person.Where(p => p.person_id == item).SingleOrDefault();
                rblGroupMembers.Items.Add(person.username);
	        }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }
}