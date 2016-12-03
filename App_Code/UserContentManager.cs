using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserContentManager
/// </summary>
namespace TeeMs.UserContentManager
{
    public class UserContentManager
    {
        private string username;
        private TeeMsEntities ctx;

        public UserContentManager(string uname)
        {
            username = uname;
            ctx = new TeeMsEntities();
        }

        public List<group> GetUserGroups()
        {
            try
            {
                var rightperson = ctx.person.Where(p => p.username == username).FirstOrDefault();
                var usergroupids = ctx.group_member.Where(gm => gm.person_id == rightperson.person_id).ToList();

                List<group> usergroups = new List<group>();

                foreach (var item in usergroupids)
                {
                    usergroups.Add(ctx.group.Where(g => g.group_id == item.group_id).FirstOrDefault());
                }

                return usergroups;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    } 
}