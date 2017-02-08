﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Microsoft.AspNet.SignalR;
using SignalRChat;

public partial class Chat : System.Web.UI.Page
{
    private TeeMsEntities ctx;
    private FormsAuthenticationTicket ticket;

    protected void Page_Load(object sender, EventArgs e)
    {
        ctx = new TeeMsEntities();

        string project_id = String.Empty;

        try
        {
            HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            ticket = FormsAuthentication.Decrypt(authcookie.Value);

            project_id = Request.QueryString["Project"];
        }
        catch (HttpException ex)
        {

            lbMessages.Text = ex.Message;
        }

        if (!IsPostBack)
        {
            FillControls();
        }

        var context = GlobalHost.ConnectionManager.GetHubContext<TeeMsHub>();
    }

    protected void FillControls()
    {

    }

    #region JAVASCRIPT

    [WebMethod]
    public static string SendUserInfo()
    {
        var UserDataRetriever = new Chat();
        string username = UserDataRetriever.GetUserData();

        return username;
    }

    public string GetUserData()
    {
        try
        {
            return "In progress";
        }
        catch (Exception ex)
        {
            
            return ex.Message;
        }
    }

    #endregion
}