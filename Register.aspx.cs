﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeeMs.Encoder;

public partial class Register : System.Web.UI.Page
{
    private TeeMsEntities ctx;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnRegisterUser_Click(object sender, EventArgs e)
    {
        CreateNewUser();
    }

    protected void CreateNewUser()
    {

        // Create a new person and add them to the database
        string uname = txtUserName.Text;
        string fname = txtFirstName.Text;
        string lname = txtLastName.Text;
        string password = txtPassword.Text;
        string email = txtEmail.Text;
        DateTime cdate = DateTime.Now;
        int privacynro = 1;

        Encoder encoder = new Encoder();

        string salt = encoder.GetSalt();

        string saltedhash = encoder.GenerateSaltedHash(password, salt);

        ctx = new TeeMsEntities();

        if (password == txtConfirmPassword.Text)
        {
            try
            {
                var rightrole = ctx.role.Where(r => r.@class == 3).SingleOrDefault();
                int role_to_input = rightrole.role_id;

                person p = new person
                {
                    first_name = fname,
                    last_name = lname,
                    username = uname,
                    email = email,
                    creation_date = cdate,
                    privacy = privacynro,
                    role_id = role_to_input
                };

                login l = new login
                {
                    login_name = uname,
                    salt = salt,
                    password = saltedhash
                };

                connection newconnection = new connection()
                {
                    connected = false,
                    connection_username = uname,
                    person_id = p.person_id
                };
                
                ctx.person.Add(p);
                ctx.login.Add(l);
                ctx.connection.Add(newconnection);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {

                lbMessages.Text = ex.Message;

                if (ex.InnerException != null)
                {
                    lbMessages.Text = ex.InnerException.ToString();
                }
            }

            Response.Redirect("Login.aspx");
        }
    }
}