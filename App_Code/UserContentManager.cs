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

        // Get any group where the user is a group_member
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

        // Get any project which includes the users group as a project_group
        public List<project> GetUserProjects()
        {
            try
            {
                List<group> usergroups = GetUserGroups();
                List<project_group> userprojectgroups = ctx.project_group.ToList(); 
                List<project> userprojects = new List<project>();

                foreach (var group in usergroups)
                {
                    foreach (var projectgroup in userprojectgroups)
                    {
                        if (projectgroup.group_id == group.group_id)
                        {
                            userprojects.Add(ctx.project.Where(pr => pr.project_id == projectgroup.project_id).FirstOrDefault());
                        }
                    }
                }

                return userprojects;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public List<assignment> GetProjectAssignments()
        {
            try
            {
                List<assignment> projectassignments = new List<assignment>();

                return projectassignments;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    } 
}