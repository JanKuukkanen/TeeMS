using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections.Specialized;
using TeeMs.UserContentManager;

public partial class Assignment : System.Web.UI.Page
{
    TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        ctx = new TeeMsEntities();

        string project_id = "";
        string assignment_id = "";

        try
        {
            HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);

            assignment_id = Request.QueryString["Assignment"];
            project_id = Request.QueryString["Project"];
        }
        catch (HttpException ex)
        {

            lbMessages.Text = ex.Message;
        }

        if (!IsPostBack)
        {
            try
            {
                FillControls(int.Parse(project_id), int.Parse(assignment_id));
            }
            catch (Exception ex)
            {
                
                lbMessages.Text = ex.Message;
            }
        }
    }

    protected void FillControls(int project_id, int assignment_id)
    {
        try
        {
            // Set the correct assignment name to the title and header
            var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();
            var rightassignment = ctx.assignment.Where(amt => amt.project_id == project_id && amt.amt_id == assignment_id).SingleOrDefault();

            titleAssignmentTitle.InnerText = rightassignment.name;
            h1AssignmentName.InnerText = rightassignment.name;

            // Set the assignment description to textbox
            if (rightassignment.description != null)
            {
                txtAssignmentDescription.Text = rightassignment.description;
            }
            else
            {
                txtAssignmentDescription.Text = String.Format("Assignment {0} does not have a description.", rightassignment.name);
            }

            // Fill Assignmentmember lists
            FillAssignmentMemberList();
            FillPanelAssignmentMemberList();
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }
    protected void btnEditAssignmentDescription_Click(object sender, EventArgs e)
    {
        if (txtAssignmentDescription.ReadOnly == true)
        {
            txtAssignmentDescription.ReadOnly = false;
            btnEditAssignmentDescription.Text = "Save changes";
        }
        else if (txtAssignmentDescription.ReadOnly == false)
        {
            try
            {
                string assignment_id = Request.QueryString["Assignment"];
                int parsed_assignment_id = int.Parse(assignment_id);

                var rightassignment = ctx.assignment.Where(a => a.amt_id == parsed_assignment_id).SingleOrDefault();

                if (rightassignment != null)
                {
                    rightassignment.description = txtAssignmentDescription.Text;
                    rightassignment.edited = DateTime.Now;

                    ctx.SaveChanges();
                }

                txtAssignmentDescription.Text = rightassignment.description;
                txtAssignmentDescription.ReadOnly = true;
                btnEditAssignmentDescription.Text = "Edit description";
            }
            catch (Exception ex)
            {

                lbMessages.Text = ex.Message;
            }
        }
    }

    protected void FillAssignmentMemberList()
    {
        try
        {
            int assignment_id = int.Parse(Request.QueryString["Assignment"]);
            int project_id = int.Parse(Request.QueryString["Project"]);

            var rightassignment = ctx.assignment.Where(amt => amt.project_id == project_id && amt.amt_id == assignment_id).SingleOrDefault();
            var assignmentpersons = ctx.assignment_person.Where(ap => ap.amt_id == rightassignment.amt_id).ToList();

            List<person> persons = new List<person>();
            List<int> ids = new List<int>();

            foreach (var assignmentperson in assignmentpersons)
            {
                if (!ids.Contains(assignmentperson.person_id))
                {
                    persons.Add(ctx.person.Where(p => p.person_id == assignmentperson.person_id).SingleOrDefault());
                    ids.Add(assignmentperson.person_id);
                }
            }

            foreach (var person in persons)
            {
                cblAssignmentMemberList.Items.Add(person.username);
            }

            Session["AssignmentMembers"] = persons;
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void FillPanelAssignmentMemberList()
    {
        try
        {
            int assignment_id = int.Parse(Request.QueryString["Assignment"]);
            int project_id = int.Parse(Request.QueryString["Project"]);

            var rightassignment = ctx.assignment.Where(amt => amt.project_id == project_id && amt.amt_id == assignment_id).SingleOrDefault();
            var assignmentpersons = ctx.assignment_person.Where(ap => ap.amt_id == rightassignment.amt_id).ToList();

            List<person> persons = new List<person>();
            List<int> ids = new List<int>();

            foreach (var assignmentperson in assignmentpersons)
            {
                if (!ids.Contains(assignmentperson.person_id))
                {
                    persons.Add(ctx.person.Where(p => p.person_id == assignmentperson.person_id).SingleOrDefault());
                    ids.Add(assignmentperson.person_id);
                }
            }

            foreach (var person in persons)
            {
                cblPanelAssignmentMembers.Items.Add(person.username); 
            }

            Session["assignmentpersons"] = persons;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    #region ASSIGNMENT_BUTTONS

    protected void btnAddAssignmentMember_Click(object sender, EventArgs e)
    {
        if (divSearch.Visible == false)
        {
            divSearch.Visible = true;
            btnAddAssignmentMember.Text = "Close search";
        }
        else
        {
            divSearch.Visible = false;
            btnAddAssignmentMember.Text = "Add Member";
        }
    }

    protected void btnRemoveAssignmentMember_Click(object sender, EventArgs e)
    {
        string assignment_id = String.Empty;
        string project_id = String.Empty;

        try
        {
            assignment_id = Request.QueryString["Assignment"];
            project_id = Request.QueryString["Project"];

            int toremoveassignment = int.Parse(assignment_id);
            int toremoveproject = int.Parse(project_id);

            foreach (ListItem item in cblAssignmentMemberList.Items)
            {
                if (item.Selected == true)
                {
                    var rightperson = ctx.person.Where(p => p.username == item.Text).SingleOrDefault();
                    var rightassignment = ctx.assignment.Where(amt => amt.amt_id == toremoveassignment).SingleOrDefault();

                    if (rightperson != null && rightassignment != null)
                    {
                        var rightassignmentperson = ctx.assignment_person.Where(aspe => aspe.person_id == rightperson.person_id && aspe.amt_id == rightassignment.amt_id && aspe.project_id == toremoveproject).SingleOrDefault();

                        if (rightassignmentperson != null)
                        {
                            ctx.assignment_person.Remove(rightassignmentperson);
                            ctx.SaveChanges();

                            Response.Redirect(String.Format(Request.ApplicationPath + "Assignment.aspx?Assignment={0}&Project={1}", rightassignment.amt_id, toremoveproject));
                        }
                    }  
                }
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    #endregion

    #region ASSIGNMENT_MODAL_POPUP

    protected void btnAddComponent_Click(object sender, EventArgs e)
    {
        try
        {
            string componentname = txtAddComponent.Text;

            List<person> assignmentpersonlist = (List<person>)Session["assignmentpersons"];
            List<person> personstoadd = new List<person>();

            foreach (ListItem item in cblPanelAssignmentMembers.Items)
            {
                if (item.Selected == true)
                {
                    personstoadd.Add(assignmentpersonlist.Where(amper => amper.username == item.Text).SingleOrDefault());
                }
            }

            CreateNewComponent(componentname, personstoadd);
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void CreateNewComponent(string componentname, List<person> personstoadd)
    {
        try
        {
            int assignment_id = int.Parse(Request.QueryString["Assignment"]);
            int project_id = int.Parse(Request.QueryString["Project"]);

            var rightassignment = ctx.assignment.Where(amt => amt.amt_id == assignment_id).SingleOrDefault();

            if (rightassignment != null)
            {
                // Create new assignment_component
                assignment_component newcomponent = new assignment_component
                {
                    amt_id = assignment_id,
                    project_id = project_id,
                    name = componentname,
                    finished = false,
                    edited = DateTime.Now
                };

                ctx.assignment_component.Add(newcomponent);

                // Create new assignment_component_persons
                foreach (var person in personstoadd)
                {
                    assignment_component_person componentmember = new assignment_component_person
                    {
                        amtc_id = newcomponent.amtc_id,
                        amt_id = assignment_id,
                        project_id = project_id,
                        person_id = person.person_id
                    };

                    ctx.assignment_component_person.Add(componentmember);
                    person.assignment_component_person.Add(componentmember);
                    newcomponent.assignment_component_person.Add(componentmember);
                }

                ctx.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    #endregion

    #region FILL_COMPONENT_LIST

    protected void FillComponentList()
    {
        try 
	    {	        
		    int assignment_id = int.Parse(Request.QueryString["Assignment"]);
            int project_id = int.Parse(Request.QueryString["Project"]);

            var rightassignment = ctx.assignment.Where(amt => amt.amt_id == assignment_id).SingleOrDefault();

            foreach (var assignmentcomponent in rightassignment.assignment_component.ToList())
            {
                HtmlGenericControl componentdiv = new HtmlGenericControl("div");
                HtmlGenericControl componentul = new HtmlGenericControl("ul");

                // Jatketaan tästä
            }
	    }
	    catch (Exception ex)
	    {
		
		    lbMessages.Text = ex.Message;
	    }
    }

    #endregion

    #region SEARCH_FUNCTIONS

    protected void btnSearchPersons_Click(object sender, EventArgs e)
    {
        // Search the database for the persons the user is searching
        string searchperson = txtSearchPersons.Text;

        List<person> personstoadd = SearchPersons(searchperson);

        DataTable dt = new DataTable();

        dt.Columns.Add("username", typeof(string));
        dt.Columns.Add("creation_date", typeof(string));

        // Then fill the gridview with the results
        try
        {
            if (personstoadd != null)
            {
                foreach (var person in personstoadd)
                {
                    dt.Rows.Add(person.username, person.creation_date);
                    gvPersons.DataSource = dt;
                    gvPersons.DataBind();
                }
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    // Search the database for the persons the user is searching
    protected List<person> SearchPersons(string searchperson)
    {
        bool check_username = true;
        bool check_eligibility = false;

        List<person> personstoadd = new List<person>();

        UserContentManager contentmanager = new UserContentManager(ticket.Name);

        try
        {
            string project_id = Request.QueryString["Project"];
            List<person> projectusers = contentmanager.GetProjectUsers(project_id);

            var persons = ctx.person.ToList();

            if (persons != null && searchperson != String.Empty)
            {
                foreach (var person in persons)
                {
                    check_username = true;

                    for (int i = 0; i < searchperson.Count(); i++)
                    {
                        if (searchperson[i] != person.username[i] && person.username[i] != null)
                        {
                            check_username = false;
                        }
                    }

                    foreach (var projectperson in projectusers)
                    {
                        if (projectperson.person_id == person.person_id)
                        {
                            check_eligibility = true;
                        }

                        if (!personstoadd.Contains(person))
                        {
                            if (check_username == true && check_eligibility == true)
                            {
                                personstoadd.Add(person);
                            }  
                        }
                    }
                }
            }

            return personstoadd;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
            return null;
        }
    }

    protected void gvPersons_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Add the selected person to the assignment
        string username = (gvPersons.SelectedRow.Cells[0].Controls[0] as LinkButton).Text;
        string assignment_id = String.Empty;
        string project_id = String.Empty;


        // Fill the group_member database table as well
        try
        {
            assignment_id = Request.QueryString["Assignment"];
            project_id = Request.QueryString["Project"];

            int to_add_assignmentid = int.Parse(assignment_id);
            int to_add_projectid = int.Parse(project_id);

            if (assignment_id != String.Empty && project_id != String.Empty)
            {
                var addedperson = ctx.person.Where(p => p.username == username).SingleOrDefault();
                var addedassignment = ctx.assignment.Where(amt => amt.amt_id == to_add_assignmentid).SingleOrDefault();

                if (addedperson != null && addedassignment != null)
                {
                    var aspe = new assignment_person
                    {
                        amt_id = addedassignment.amt_id,
                        project_id = to_add_projectid,
                        person_id = addedperson.person_id
                    };

                    addedassignment.edited = DateTime.Now;
                    addedperson.edited = DateTime.Now;

                    addedassignment.assignment_person.Add(aspe);
                    addedperson.assignment_person.Add(aspe);
                    ctx.SaveChanges();
                }
            }
            else
            {
                lbMessages.Text = "No Person Specified";
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }

        if (assignment_id != String.Empty && project_id != String.Empty)
        {
            Response.Redirect(String.Format(Request.ApplicationPath + "Assignment.aspx?Assignment={0}&Project={1}",assignment_id, project_id));
        }
    }

#endregion
}