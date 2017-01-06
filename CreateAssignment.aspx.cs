using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TeeMs.UserContentManager;

public partial class CreateAssignment : System.Web.UI.Page
{
    private TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        ctx = new TeeMsEntities();

        try
        {
            HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);
        }
        catch (HttpException ex)
        {

            lbmessages.Text = ex.Message;
        }

        if (!IsPostBack)
        {
            FillControls();
        }
    }

    protected void FillControls()
    {
        try
        {
            // Fill the dropdownlist with the users from the groups currently working
            // on the project the assignment will be a part of
            UserContentManager contentmanager = new UserContentManager(ticket.Name);
            string project_id = Request.QueryString["Project"];

            List<person> userquery = contentmanager.GetProjectUsers(project_id);

            lbmessages.Text = userquery.Count.ToString();

            if (userquery != null)
            {
                foreach (var user in userquery)
                {
                    ddlUserList.Items.Add(user.username);
                }
            }
            ddlUserList.Items.Insert(0, "Choose User");
        }
        catch (Exception ex)
        {

            lbmessages.Text = ex.Message;
        }
    }

    protected void btnCreateAssignment_Click(object sender, EventArgs e)
    {
        int assignment_id = CreateNewAssignment();

        if ((ddlUserList.SelectedItem.Text != ddlUserList.Items[0].Text) && (assignment_id != -1))
        {
            CreateAssignmentPerson(assignment_id);
        }

        if (assignment_id != -1)
        {
            Response.Redirect(String.Format("Assignment.aspx?Assignment={0}", assignment_id));
        }
    }

    protected int CreateNewAssignment()
    {
        string project_textid = Request.QueryString["Project"];
        int project_id = int.Parse(project_textid);

        string name = txtAssignmentName.Text;
        string description = txtAssignmentDescription.Text;
        string percent_done = "0";
        bool finished = false;

        DateTime duedate = calendarDueDate.SelectedDate;

        try
        {
            int emptyrow = 0;
            int addedrow = 0;

            // First check for any gaps in the number sequence inside the database
            for (int i = 0; i <= ctx.assignment.Count(); i++)
            {
                assignment compareAssignment = new assignment();
                var checkAssignment = ctx.assignment.Where(gr => gr.amt_tag == i).FirstOrDefault();

                if (checkAssignment == compareAssignment)
                {
                    emptyrow = i;
                }
                else
                {
                    compareAssignment = checkAssignment;
                }
            }

            if (emptyrow == 0)
            {
                addedrow = ctx.assignment.Count() + 1;
            }
            else
            {
                addedrow = emptyrow;
            }

            var newassignment = new assignment
            {
                name = name,
                description = description,
                amt_tag = addedrow,
                creation_date = DateTime.Today,
                assignment_due_date = duedate,
                percent_done = percent_done,
                finished = finished,
                privacy = 1,
                project_id = project_id
            };

            ctx.assignment.Add(newassignment);
            ctx.SaveChanges();

            return newassignment.amt_id;
        }
        catch (Exception ex)
        {

            lbmessages.Text = ex.Message;
            return -1;
        }
    }

    protected void CreateAssignmentPerson(int assignment_id)
    {
        try
        {
            var rightperson = ctx.person.Where(p => p.username == ticket.Name).SingleOrDefault();
            var rightassignment = ctx.assignment.Where(a => a.amt_id == assignment_id).SingleOrDefault();

            if (rightperson != null && rightassignment != null)
            {
                var assignmentperson = new assignment_person
                {
                    person_id = rightperson.person_id,
                    amt_id = rightassignment.amt_id,
                    project_id = rightassignment.project_id
                };

                rightperson.assignment_person.Add(assignmentperson);
                rightassignment.assignment_person.Add(assignmentperson);
                ctx.SaveChanges();
            }
        }
        catch (Exception ex)
        {

            lbmessages.Text = ex.Message;
        }
    }
}