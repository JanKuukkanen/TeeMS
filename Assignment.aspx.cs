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
using System.Text.RegularExpressions;
using AjaxControlToolkit;
using TeeMs.UserContentManager;

public partial class Assignment : System.Web.UI.Page
{
    TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        // set the database context using entity framework
        ctx = new TeeMsEntities();

        int project_id = 0;
        int assignment_id = 0;
        bool isarchived = false;
        bool alloweduser = false;

        try
        {
            HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);

            assignment_id = int.Parse(Request.QueryString["Assignment"]);
            project_id = int.Parse(Request.QueryString["Project"]);

            var rightproject = ctx.project.Where(g => g.project_id == project_id).SingleOrDefault();
            var rightperson = ctx.person.Where(p => p.username == User.Identity.Name).SingleOrDefault();
            var rightprojectmember = rightperson.project_person.Where(prope => prope.project_id == rightproject.project_id).SingleOrDefault();

            if (rightprojectmember != null)
            {
                alloweduser = true;
            }

            isarchived = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault().finished;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }

        if (alloweduser != true)
        {
            Response.Redirect(String.Format(Request.ApplicationPath + "Home.aspx"));
        }

        if (!IsPostBack)
        {
            try
            {
                FillControls(project_id, assignment_id);
            }
            catch (Exception ex)
            {
                
                lbMessages.Text = ex.Message;
            }
        }
        else if (IsPostBack)
        {
            FillComponentList();
            FillComments();
        }

        if (isarchived == true)
        {
            ArchiveProjectPage();
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

            // Fill Assignmentmember lists
            FillAssignmentMemberList();
            FillPanelAssignmentMemberList();
            FillComponentList();
            FillComments();
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void btnBackToProject_Click(object sender, EventArgs e)
    {
        int project_id = int.Parse(Request.QueryString["Project"]);

        Response.Redirect(String.Format(Request.ApplicationPath + "Project.aspx?Project={0}", project_id));
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
            cblPanelAssignmentMembers.Items.Clear();

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

    protected void FillPanelComboboxMemberList()
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

            cblShowComponentMembers.Items.Clear();

            foreach (var person in persons)
            {
                cblShowComponentMembers.Items.Add(person.username);
            }

            Session["assignmentcomponentpersons"] = persons;
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
                        var assignmentcomponentpersons = rightperson.assignment_component_person.ToList();

                        foreach (var member in assignmentcomponentpersons)
                        {
                            var rightassignmentcomponentperson = ctx.assignment_component_person.Where(acompe => acompe.amtc_id == member.amtc_id).SingleOrDefault();

                            ctx.assignment_component_person.Remove(rightassignmentcomponentperson);
                        }

                        var rightassignmentperson = ctx.assignment_person.Where(aspe => aspe.person_id == rightperson.person_id && aspe.amt_id == rightassignment.amt_id && aspe.project_id == toremoveproject).SingleOrDefault();

                        // If the assignmentperson was found remove it from the database and redirect the user to the assignmnet page
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

    protected void btnConfirmDeleteAssignment_Click(object sender, EventArgs e)
    {
        // Delete Assignment and assignment persons, Assignment components associated with the deleted assignment and assignment components persons associated with that assignment component
        try
        {
            int assignment_id = int.Parse(Request.QueryString["Assignment"]);
            int project_id = int.Parse(Request.QueryString["Project"]);

            var rightassignment = ctx.assignment.Where(amt => amt.amt_id == assignment_id).SingleOrDefault();
            var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();

            if (rightassignment != null && rightproject != null)
	        {
		        var assignmentcomponents = rightassignment.assignment_component.ToList();

                    foreach (var component in assignmentcomponents)
                    {
                        var rightcomponent = ctx.assignment_component.Where(amtc => amtc.amtc_id == component.amtc_id).SingleOrDefault();
                        var assignmentcomponentpersons = rightcomponent.assignment_component_person.ToList();

                        foreach (var componentperson in assignmentcomponentpersons)
                        {
                            var rightcomponentperson = ctx.assignment_component_person.Where(acompe => acompe.assignment_component_person_id == componentperson.assignment_component_person_id).SingleOrDefault();

                            ctx.assignment_component_person.Remove(rightcomponentperson);
                        }

                        ctx.assignment_component.Remove(rightcomponent);
                    }

                    var assignmentpersons = rightassignment.assignment_person.ToList();

                    foreach (var assignmentperson in assignmentpersons)
                    {
                        var rightassignmentperson = ctx.assignment_person.Where(aspe => aspe.assignment_person_id == assignmentperson.assignment_person_id).SingleOrDefault();

                        ctx.assignment_person.Remove(rightassignmentperson);
                    }

                ctx.assignment.Remove(rightassignment);
                ctx.SaveChanges();

                Response.Redirect(String.Format(Request.ApplicationPath + "Project.aspx?Project={0}", rightproject.project_id));
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

            if (personstoadd.Count != 0)
            {
                CreateNewComponent(componentname, personstoadd); 
            }
            else if (personstoadd.Count == 0)
            {
                lbMessages.Text = "You must have atleast 1 person selected!";
            }

            txtAddComponent.Text = "";

            foreach (ListItem member in cblPanelAssignmentMembers.Items)
            {
                member.Selected = false;
            }
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

                divAssignmentComponents.Controls.Clear();
                FillComponentList();
                lbMessages.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    #endregion

    #region COMPONENT_LIST_FUNCTIONS

    protected void FillComponentList()
    {
        try 
	    {	        
		    int assignment_id = int.Parse(Request.QueryString["Assignment"]);
            int project_id = int.Parse(Request.QueryString["Project"]);

            UserContentManager contentmanager = new UserContentManager(ticket.Name);

            var rightassignment = ctx.assignment.Where(amt => amt.amt_id == assignment_id).SingleOrDefault();

            foreach (var assignmentcomponent in rightassignment.assignment_component.ToList())
            {
                List<person> assignmentpersons = contentmanager.GetAssignmentUsers(rightassignment.amt_id);
                List<person> assignmentcomponentpersons = new List<person>();

                List<assignment_component_person> amtcompers = assignmentcomponent.assignment_component_person.ToList();

                foreach (var amtcomper in amtcompers)
                {
                    foreach (var assignmentperson in assignmentpersons)
                    {
                        if (assignmentperson.person_id == amtcomper.person_id && !assignmentcomponentpersons.Contains(assignmentperson))
                        {
                            assignmentcomponentpersons.Add(assignmentperson);
                        }
                    }
                }

                HtmlGenericControl componentdiv = new HtmlGenericControl("div");
                HtmlGenericControl componentborderdiv = new HtmlGenericControl("div");
                HtmlGenericControl componentlabeldiv = new HtmlGenericControl("div");
                HtmlGenericControl componentul = new HtmlGenericControl("ul");
                LinkButton componentlabelbutton = new LinkButton();

                componentdiv.Attributes.Add("class", "divComponent");
                componentborderdiv.Attributes.Add("class", "divComponentBorder");

                if (assignmentcomponent.finished == false)
                {
                    componentborderdiv.Attributes.Add("style", "background-color:#6fa9d6;"); 
                }
                else if (assignmentcomponent.finished == true)
                {
                    componentborderdiv.Attributes.Add("style", "background-color:#28fc60;"); 
                }

                componentlabeldiv.Attributes.Add("class", "divComponentLabel");

                componentlabelbutton.ID = String.Format("lbtnComponentName{0}", assignmentcomponent.amtc_id);
                componentlabelbutton.Text = assignmentcomponent.name;
                componentlabelbutton.CommandArgument = "Hello";
                componentlabelbutton.Command += new CommandEventHandler(LinkButtonClick);
                componentlabelbutton.CssClass = "ComponentLabels";

                componentul.Attributes.Add("id", "assignmentcomponentul");

                componentlabeldiv.Controls.Add(componentlabelbutton);

                foreach (var componentperson in assignmentcomponentpersons)
                {
                    HtmlGenericControl componentli = new HtmlGenericControl("li");
                    HtmlGenericControl componentpersonlink = new HtmlGenericControl("a");

                    componentpersonlink.Attributes.Add("href", String.Format("ViewUser.aspx?Person={0}", componentperson.person_id));
                    componentpersonlink.InnerText = componentperson.username;

                    componentli.Controls.Add(componentpersonlink);
                    componentul.Controls.Add(componentli);
                }

                componentborderdiv.Controls.Add(componentlabeldiv);
                componentborderdiv.Controls.Add(componentul);
                componentdiv.Controls.Add(componentborderdiv);

                divAssignmentComponents.Controls.Add(componentdiv);
            }
        }
	    catch (Exception ex)
	    {
		
		    lbMessages.Text = ex.Message;
	    }
    }

    // When the user clicks an assignment component linkbutton it will make a new
    // modalpopup show up and display info on the chosen assignment component
    private void LinkButtonClick(object sender, CommandEventArgs e)
    {
        // Parse the correct assignment component id from the linkbutton's id
        var btn = sender as LinkButton;
        string parseid = Regex.Match(btn.ID, @"-?\d+\.?\d*$").Value;
        int assignmentcomponentid = int.Parse(parseid);

        try
        {
            var rightassignmentcomponent = ctx.assignment_component.Where(amtc => amtc.amtc_id == assignmentcomponentid).SingleOrDefault();

            int assignment_id = int.Parse(Request.QueryString["Assignment"]);
            int project_id = int.Parse(Request.QueryString["Project"]);

            var rightassignment = ctx.assignment.Where(amt => amt.project_id == project_id && amt.amt_id == assignment_id).SingleOrDefault();
            var assignmentpersons = ctx.assignment_person.Where(ap => ap.amt_id == rightassignment.amt_id).ToList();

            txtShowComponentName.Text = rightassignmentcomponent.name;

            if (rightassignmentcomponent.finished == true)
            {
                cbComponentFinished.Checked = true;
            }
            else
            {
                cbComponentFinished.Checked = false;
            }

            // Cut off here
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

            cblShowComponentMembers.Items.Clear();
            int personnro = 0;

            foreach (var person in persons)
            {
                cblShowComponentMembers.Items.Add(person.username);

                if (rightassignmentcomponent.assignment_component_person.Any(acompe => acompe.person_id == person.person_id))
                {
                    cblShowComponentMembers.Items[personnro].Selected = true;
                }

                personnro++;
            }

            Session["assignmentcomponentpersons"] = persons;
            Session["rightassignmentcomponent"] = rightassignmentcomponent;

            mpeShowComponentModal.Show();
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void btnSaveChanges_Click(object sender, EventArgs e)
    {
        try
        {
            List<person> assignmentcomponentpersonlist = (List<person>)Session["assignmentcomponentpersons"];
            assignment_component sessionassignmentcomponent = (assignment_component)Session["rightassignmentcomponent"];

            var rightassignmentcomponent = ctx.assignment_component.Where(amtc => amtc.amtc_id == sessionassignmentcomponent.amtc_id).SingleOrDefault();

            List<person> members_to_remove = new List<person>();
            List<person> members_to_add = new List<person>();

            foreach (ListItem item in cblShowComponentMembers.Items)
            {
                if (item.Selected == false && rightassignmentcomponent.assignment_component_person.Any(acompe => acompe.person.username == item.Text))
                {
                    members_to_remove.Add(assignmentcomponentpersonlist.Where(p => p.username == item.Text).SingleOrDefault());
                }
                else if (item.Selected == true && !rightassignmentcomponent.assignment_component_person.Any(acompe => acompe.person.username == item.Text))
                {
                    members_to_add.Add(assignmentcomponentpersonlist.Where(p => p.username == item.Text).SingleOrDefault());
                }
            }

            bool madechanges = false;

            if (txtShowComponentName.Text != rightassignmentcomponent.name)
            {
                rightassignmentcomponent.name = txtShowComponentName.Text;
                madechanges = true;
            }

            if (members_to_remove != null)
            {
                RemoveComponentMembers(members_to_remove, rightassignmentcomponent);
                madechanges = true;
            }

            if (members_to_add != null)
            {
                AddComponentMembers(members_to_add, rightassignmentcomponent);
                madechanges = true;
            }

            if (rightassignmentcomponent.finished != cbComponentFinished.Checked)
            {
                rightassignmentcomponent.finished = cbComponentFinished.Checked;
                ctx.SaveChanges();

                madechanges = true;
            }

            if (madechanges == true)
            {
                divAssignmentComponents.Controls.Clear();
                FillComponentList();
                lbMessages.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void RemoveComponentMembers(List<person> members, assignment_component rightassignmentcomponent)
    {
        try
        {
            foreach (var member in members)
            {
                var assignmentcomponentperson_to_remove = ctx.assignment_component_person.Where(acompe => acompe.person_id == member.person_id && acompe.amtc_id == rightassignmentcomponent.amtc_id).SingleOrDefault();

                ctx.assignment_component_person.Remove(assignmentcomponentperson_to_remove); 
            }

            ctx.SaveChanges();
        }
        catch (Exception ex)
        {
                
            lbMessages.Text = ex.Message;
        }
    }

    protected void AddComponentMembers(List<person> members, assignment_component rightassignmentcomponent)
    {
        try
        {
            int assignment_id = int.Parse(Request.QueryString["Assignment"]);
            int project_id = int.Parse(Request.QueryString["Project"]);

            // Create new assignment_component_persons
            foreach (var member in members)
            {
                assignment_component_person componentmember = new assignment_component_person
                {
                    amtc_id = rightassignmentcomponent.amtc_id,
                    amt_id = assignment_id,
                    project_id = project_id,
                    person_id = member.person_id
                };

                ctx.assignment_component_person.Add(componentmember);
                member.assignment_component_person.Add(componentmember);
                rightassignmentcomponent.assignment_component_person.Add(componentmember);
            }

            ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            
            lbMessages.Text = ex.Message;
        }
    }

    protected void btnRemoveAssignmentComponent_Click(object sender, EventArgs e)
    {
        try
        {
            var sessionassignmentcomponent = (assignment_component)Session["rightassignmentcomponent"];

            foreach (var componentmember in sessionassignmentcomponent.assignment_component_person.ToList())
            {
                var rightassignmentcomponentperson = ctx.assignment_component_person.Where(acompe => acompe.assignment_component_person_id == componentmember.assignment_component_person_id).SingleOrDefault();

                ctx.assignment_component_person.Remove(rightassignmentcomponentperson);
            }

            var rightassignmentcomponent = ctx.assignment_component.Where(amtc => amtc.amtc_id == sessionassignmentcomponent.amtc_id).SingleOrDefault();

            ctx.assignment_component.Remove(rightassignmentcomponent);

            ctx.SaveChanges();

            divAssignmentComponents.Controls.Clear();
            FillComponentList();
            lbMessages.Text = String.Empty;

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

            lbMessages.Text = String.Empty;
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
                        if (searchperson[i] != person.username[i])
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

    #region COMMENT_FUNCTIONS

    protected void FillComments()
    {
        try
        {
            divAssignmentCommentMessages.Controls.Clear();

            string username = ticket.Name;
            var rightperson = ctx.person.Where(p => p.username == username).SingleOrDefault();

            var commentlist = ctx.comment.Where(co => co.amt_id != null).ToList();
            int assignment_id = int.Parse(Request.QueryString["Assignment"]);
            int project_id = int.Parse(Request.QueryString["Project"]);

            List<HtmlGenericControl> divlist = new List<HtmlGenericControl>();

            foreach (var comment in commentlist)
            {
                if (comment.project_id == project_id && comment.amt_id == assignment_id)
                {
                    HtmlGenericControl commentdiv = new HtmlGenericControl("div");
                    HtmlGenericControl commentcontentdiv = new HtmlGenericControl("div");
                    HtmlGenericControl commentbuttonsdiv = new HtmlGenericControl("div");
                    HtmlGenericControl commenttextboxdiv = new HtmlGenericControl("div");
                    Label usernamelabel = new Label();

                    TextBox commentedittxt = new TextBox();
                    HtmlGenericControl br = new HtmlGenericControl("br");
                    LinkButton commentsavebutton = new LinkButton();
                    LinkButton commentcancelbutton = new LinkButton();

                    LinkButton commenteditbutton = new LinkButton();
                    LinkButton commentdeletebutton = new LinkButton();

                    commentdiv.ID = String.Format("commentdiv{0}", comment.comment_id);
                    commentdiv.Attributes.Add("Style", "border: thin solid black; margin-top:10px;");

                    commenttextboxdiv.ID = String.Format("commenttextboxdiv{0}", comment.comment_id);
                    commenttextboxdiv.Attributes.Add("Style", "border: thin solid black; margin-top:10px;");
                    commenttextboxdiv.Visible = false;

                    usernamelabel.Text = String.Format("Posted by {0} on {1}", comment.person.username, comment.creation_date.ToString("d/M/yyyy HH:mm"));
                    usernamelabel.Font.Bold = true;
                    commentcontentdiv.InnerText = comment.comment_content;

                    commentedittxt.ID = String.Format("txt{0}", comment.comment_id);
                    commentedittxt.Text = comment.comment_content;
                    commentedittxt.TextMode = TextBoxMode.MultiLine;
                    commentedittxt.Width = 400;
                    commentedittxt.Height = 100;

                    commentsavebutton.ID = String.Format("Savebtn{0}", comment.comment_id);
                    commentsavebutton.Text = "Save Changes";
                    commentsavebutton.CommandArgument = "Hello";
                    commentsavebutton.Command += new CommandEventHandler(SaveCommentClick);
                    commentsavebutton.CssClass = "w3-btn";

                    commentcancelbutton.ID = String.Format("Cancelbtn{0}", comment.comment_id);
                    commentcancelbutton.Text = "Cancel";
                    commentcancelbutton.CommandArgument = "Hello";
                    commentcancelbutton.Command += new CommandEventHandler(CancelCommentClick);
                    commentcancelbutton.CssClass = "w3-btn";
                    commentcancelbutton.Attributes.Add("style", "margin-left:10px;");

                    commenteditbutton.ID = String.Format("lbtnEditComment{0}", comment.comment_id);
                    commenteditbutton.Text = "Edit";
                    commenteditbutton.CommandArgument = "Hello";
                    commenteditbutton.Command += new CommandEventHandler(EditCommentClick);
                    commenteditbutton.CssClass = "CommentLinkButtons";

                    commentdeletebutton.ID = String.Format("lbtnDeleteComment{0}", comment.comment_id);
                    commentdeletebutton.Text = "Delete";
                    commentdeletebutton.CommandArgument = "Hello";
                    commentdeletebutton.Command += new CommandEventHandler(DeleteCommentClick);
                    commentdeletebutton.CssClass = "CommentLinkButtons";
                    commentdeletebutton.Attributes.Add("style", "margin-left:10px;");

                    commentdiv.Controls.Add(usernamelabel);
                    commentdiv.Controls.Add(commentcontentdiv);

                    if (comment.person_id == rightperson.person_id)
                    {
                        commenttextboxdiv.Controls.Add(commentedittxt);
                        commenttextboxdiv.Controls.Add(br);
                        commenttextboxdiv.Controls.Add(commentsavebutton);
                        commenttextboxdiv.Controls.Add(commentcancelbutton);

                        commentbuttonsdiv.Controls.Add(commenteditbutton);
                        commentbuttonsdiv.Controls.Add(commentdeletebutton);
                    }

                    divAssignmentCommentMessages.Controls.Add(commentdiv);
                    divAssignmentCommentMessages.Controls.Add(commenttextboxdiv);
                    divAssignmentCommentMessages.Controls.Add(commentbuttonsdiv);

                    divlist.Add(commentdiv);
                    divlist.Add(commenttextboxdiv);
                }
            }

            Session["Divs"] = divlist;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void EditCommentClick(object sender, CommandEventArgs e)
    {
        // Parse the correct comment id from the linkbutton's id
        var btn = sender as LinkButton;
        string parseid = Regex.Match(btn.ID, @"-?\d+\.?\d*$").Value;
        int commentid = int.Parse(parseid);

        try
        {
            var divlist = (List<HtmlGenericControl>)Session["Divs"];

            var rightcommentdiv = divlist.Where(div => div.ID == String.Format("commentdiv{0}", commentid)).SingleOrDefault();
            var rightcommenttextboxdiv = divlist.Where(div => div.ID == String.Format("commenttextboxdiv{0}", commentid)).SingleOrDefault();

            rightcommenttextboxdiv.Visible = true;
            rightcommentdiv.Visible = false;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void DeleteCommentClick(object sender, CommandEventArgs e)
    {
        // Parse the correct comment id from the linkbutton's id
        var btn = sender as LinkButton;
        string parseid = Regex.Match(btn.ID, @"-?\d+\.?\d*$").Value;
        int commentid = int.Parse(parseid);

        try
        {
            string username = ticket.Name;
            var rightperson = ctx.person.Where(p => p.username == username).SingleOrDefault();
            var rightcomment = ctx.comment.Where(com => com.comment_id == commentid).SingleOrDefault();

            if (rightcomment.person_id == rightperson.person_id)
            {
                ctx.comment.Remove(rightcomment);
                ctx.SaveChanges();

                FillComments();
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void SaveCommentClick(object sender, CommandEventArgs e)
    {
        // Parse the correct comment id from the linkbutton's id
        var btn = sender as LinkButton;
        string parseid = Regex.Match(btn.ID, @"-?\d+\.?\d*$").Value;
        int commentid = int.Parse(parseid);

        try
        {
            string username = ticket.Name;
            var rightperson = ctx.person.Where(p => p.username == username).SingleOrDefault();
            var rightcomment = ctx.comment.Where(com => com.comment_id == commentid).SingleOrDefault();

            if (rightcomment.person_id == rightperson.person_id)
            {
                var divlist = (List<HtmlGenericControl>)Session["Divs"];

                var rightcommentdiv = divlist.Where(div => div.ID == String.Format("commentdiv{0}", commentid)).SingleOrDefault();
                var rightcommenttextboxdiv = divlist.Where(div => div.ID == String.Format("commenttextboxdiv{0}", commentid)).SingleOrDefault();

                TextBox divtxt = rightcommenttextboxdiv.Controls[0] as TextBox;

                string newcontent = divtxt.Text;

                rightcomment.comment_content = newcontent;
                rightcomment.edited = DateTime.Now;

                ctx.SaveChanges();

                rightcommenttextboxdiv.Visible = false;
                rightcommentdiv.Visible = true;

                FillComments();
            }
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    protected void CancelCommentClick(object sender, CommandEventArgs e)
    {
        // Parse the correct comment id from the linkbutton's id
        var btn = sender as LinkButton;
        string parseid = Regex.Match(btn.ID, @"-?\d+\.?\d*$").Value;
        int commentid = int.Parse(parseid);

        try
        {
            var divlist = (List<HtmlGenericControl>)Session["Divs"];

            var rightcommentdiv = divlist.Where(div => div.ID == String.Format("commentdiv{0}", commentid)).SingleOrDefault();
            var rightcommenttextboxdiv = divlist.Where(div => div.ID == String.Format("commenttextboxdiv{0}", commentid)).SingleOrDefault();

            rightcommenttextboxdiv.Visible = false;
            rightcommentdiv.Visible = true;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }

    #endregion

    #region SignalR

    protected void UpdateComments(object sender, EventArgs e)
    {
        FillComments();
    }

    protected void UpdateAssignmentComponents(object sender, EventArgs e)
    {
        divAssignmentComponents.Controls.Clear();
        FillComponentList();
    }

    #endregion

    protected void ArchiveProjectPage()
    {
        int project_id = int.Parse(Request.QueryString["Project"]);

        try
        {
            var rightperson = ctx.person.Where(p => p.username == Context.User.Identity.Name).SingleOrDefault();
            var rightproject = ctx.project.Where(pr => pr.project_id == project_id).SingleOrDefault();


            // Disable the buttons on the assignment page
            btnAddAssignmentMember.Enabled = false;
            btnAddComponent.Enabled = false;
            btnDeleteAssignment.Enabled = false;
            btnRemoveAssignmentComponent.Enabled = false;
            btnRemoveAssignmentMember.Enabled = false;
            btnSaveChanges.Enabled = false;
            btnSearchPersons.Enabled = false;
            btnShowAddComponentModal.Enabled = false;
            btnConfirmDeleteAssignment.Enabled = false;
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }
}