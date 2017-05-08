using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using TeeMs.UserContentManager;

public partial class ViewUser : System.Web.UI.Page
{
    private TeeMsEntities ctx;

    protected void Page_Load(object sender, EventArgs e)
    {
        ctx = new TeeMsEntities();

        string user_id = String.Empty;

        try
        {
            user_id = Request.QueryString["Person"];
            int usernro_id = int.Parse(user_id);

            if (user_id != null)
            {
                var rightperson = ctx.person.Where(p => p.person_id == usernro_id).SingleOrDefault();

                if (rightperson.privacy == 1 || rightperson.username == User.Identity.Name)
                {
                    FillControls(usernro_id);
                    FillDivs(usernro_id); 
                }
                else if (Request.UrlReferrer.ToString() != String.Empty)
                {
                    Response.Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    Response.Redirect(String.Format(Request.ApplicationPath + "Home.aspx"));
                }
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void FillControls(int user_id)
    {
        try
        {
            var rightperson = ctx.person.Where(p => p.person_id == user_id).SingleOrDefault();

            userTitle.InnerText = rightperson.username;
            h1UserName.InnerText = rightperson.username;

            lbFirstName.Text = rightperson.first_name;
            lbLastName.Text = rightperson.last_name;
            lbEmail.Text = rightperson.email;
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void FillDivs(int user_id)
    {
        // Fill your projects section with the users projects and assignments
        // and fill your groups with the users groups

        try
        {
            var rightperson = ctx.person.Where(p => p.person_id == user_id).SingleOrDefault();


            if (rightperson != null)
            {
                UserContentManager contentmanager = new UserContentManager(rightperson.username);

                List<group> usergroups = contentmanager.GetUserGroups();
                List<project> userprojects = contentmanager.GetUserProjects();

                foreach (var project in userprojects)
                {
                    HtmlGenericControl projectdiv = new HtmlGenericControl("div");
                    HtmlGenericControl projectinnerdiv = new HtmlGenericControl("div");
                    HtmlGenericControl projectlink = new HtmlGenericControl("a");
                    HtmlGenericControl projectimage = new HtmlGenericControl("img");
                    HtmlGenericControl projectcut = new HtmlGenericControl("hr");

                    projectdiv.Attributes.Add("class", "w3-container");
                    projectdiv.ID = "divProject" + project.project_id;

                    projectinnerdiv.Attributes.Add("class", "w3-container");

                    projectimage.Attributes.Add("src", project.picture_url);
                    projectimage.Attributes.Add("alt", "Project image");
                    projectimage.Attributes.Add("height", "150px");
                    projectimage.Attributes.Add("width", "150px");
                    projectimage.Attributes.Add("style", "float:left;");

                    projectinnerdiv.Controls.Add(projectimage);

                    projectlink.Attributes.Add("href", String.Format("Project.aspx?Project={0}", project.project_id));
                    projectlink.Attributes.Add("style", "float:left; margin-top:5%; margin-left:5%;");
                    projectlink.InnerText = project.name;

                    projectinnerdiv.Controls.Add(projectlink);

                    projectcut.Attributes.Add("style", "color:#000;background-color:#000; height:5px;");

                    projectdiv.Controls.Add(projectinnerdiv);
                    projectdiv.Controls.Add(projectcut);

                    divUserProjects.Controls.Add(projectdiv);
                }

                foreach (var group in usergroups)
                {
                    HtmlGenericControl groupdiv = new HtmlGenericControl("div");
                    HtmlGenericControl groupinnerdiv = new HtmlGenericControl("div");
                    HtmlGenericControl grouplink = new HtmlGenericControl("a");
                    HtmlGenericControl groupimage = new HtmlGenericControl("img");
                    HtmlGenericControl groupcut = new HtmlGenericControl("hr");

                    groupdiv.Attributes.Add("class", "w3-container");
                    groupdiv.ID = "divGroup" + group.group_id;

                    groupinnerdiv.Attributes.Add("class", "w3-container");

                    groupimage.Attributes.Add("src", group.group_picture_url);
                    groupimage.Attributes.Add("alt", "Group image");
                    groupimage.Attributes.Add("height", "150px");
                    groupimage.Attributes.Add("width", "150px");
                    groupimage.Attributes.Add("style", "float:left;");

                    if (group.group_picture_url != null)
                    {
                        if (group.group_picture_url != String.Empty)
                        {
                            groupimage.Attributes.Add("src", group.group_picture_url);
                        }
                    }
                    else
                    {
                        groupimage.Attributes.Add("src", Request.ApplicationPath + "Images/no_image.png");
                    }

                    groupinnerdiv.Controls.Add(groupimage);

                    grouplink.Attributes.Add("href", String.Format("Group.aspx?Group={0}", group.group_id));
                    grouplink.Attributes.Add("style", "float:left; margin-top:5%; margin-left:5%;");
                    grouplink.InnerText = group.name;

                    groupinnerdiv.Controls.Add(grouplink);

                    groupcut.Attributes.Add("style", "color:#000;background-color:#000; height:5px;");

                    groupdiv.Controls.Add(groupinnerdiv);
                    groupdiv.Controls.Add(groupcut);

                    divUserGroups.Controls.Add(groupdiv);
                } 
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }
}