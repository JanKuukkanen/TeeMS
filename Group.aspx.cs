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
        ctx = new TeeMsEntities();

        string group_id = Request.QueryString["Group"];

        if (!IsPostBack)
        {
            FillControls(int.Parse(group_id)); 
        }
    }

    protected void FillControls(int group_id)
    {
        try
        {
            // Set the titles of the page to be the name of the group

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

    protected void lbtnTriggerTitleChange_Click(object sender, EventArgs e)
    {
        divDefault.Visible = false;
        divDuringChange.Visible = true;
    }

    protected void btnTitleChanger_Click(object sender, EventArgs e)
    {
        string group_id = Request.QueryString["Group"];
        string errormessage = "Before anything";

        try
        {
            errormessage = "Before if";
            int num = 0;
            int parsed_group_id = int.Parse(group_id);

            var rightgroup = ctx.group.Where(g => g.group_id == parsed_group_id).SingleOrDefault();

            errormessage = "After context";

            if (rightgroup != null)
            {
                rightgroup.name = txtTitleChanger.Text;
                rightgroup.edited = DateTime.Now;

                errormessage = "After if";

                num = ctx.SaveChanges();

                h1GroupName.InnerText = txtTitleChanger.Text;
                groupTitle.InnerText = txtTitleChanger.Text;
                lbMessages.Text = String.Format("Changed group name", num.ToString());
            }
            else
            {
                lbMessages.Text = "Failed to change the name of the group";
            }

            divDuringChange.Visible = false;
            divDefault.Visible = true;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message + " " + errormessage;
        }
    }

    protected void btnTitleCancel_Click(object sender, EventArgs e)
    {
        divDuringChange.Visible = false;
        divDefault.Visible = true;
    }
}