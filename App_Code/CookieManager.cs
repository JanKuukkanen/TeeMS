using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity.Migrations;
using System.Web.Security;

/// <summary>
/// Summary description for CookieManager
/// </summary>
namespace TeeMs.CookieManager
{
    public class CookieManager
    {
        private TeeMsEntities ctx;
        private FormsAuthenticationTicket ticket;

        public CookieManager()
        {

        }

        public FormsAuthenticationTicket DecryptAuthCookie(HttpCookie authcookie)
        {
            ticket = FormsAuthentication.Decrypt(authcookie.Value);
            return ticket;
        }
    } 
}