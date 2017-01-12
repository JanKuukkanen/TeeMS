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
                HtmlGenericControl assignmentmemberdiv = new HtmlGenericControl("div");
                assignmentmemberdiv.Attributes.Add("class", "w3-container");
                assignmentmemberdiv.InnerText = person.username;

                divMemberList.Controls.Add(assignmentmemberdiv);
            }
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
        }
        catch (Exception ex)
        {

            lbMessages.Text = ex.Message;
        }
    }
}