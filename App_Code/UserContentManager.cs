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
        private string project_id;
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

                List<group> allgroups = ctx.group.ToList();
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

                List<project> allcreatedprojects = ctx.project.Where(pr => pr.project_creator == username).ToList();
                List<project> userprojects = new List<project>();

                foreach (var group in usergroups)
                {
                    foreach (var projectgroup in userprojectgroups)
                    {
                        if (projectgroup.group_id == group.group_id)
                        {
                            project to_add_project = ctx.project.Where(pr => pr.project_id == projectgroup.project_id).FirstOrDefault();

                            if (!userprojects.Contains(to_add_project))
                            {
                                userprojects.Add(ctx.project.Where(pr => pr.project_id == projectgroup.project_id).FirstOrDefault()); 
                            }
                        }
                    }
                }

                foreach (var project in allcreatedprojects)
                {
                    if (!userprojects.Contains(project))
                    {
                        userprojects.Add(project);
                    }
                }

                return userprojects;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        // Get all the person's currently working on a project
        public List<person> GetProjectUsers(string project_textid)
        {
            try
            {
                int project_id = int.Parse(project_textid);
                var projectgroupquery = ctx.project_group.Where(pg => pg.project_id == project_id).ToList();

                List<group> groups = new List<group>();
                List<group_member> groupmembers = new List<group_member>();
                List<person> users = new List<person>();

                if (projectgroupquery != null)
	            {
		            foreach (var projectgroup in projectgroupquery)
                    {
                        groups = ctx.group.Where(g => g.group_id == projectgroup.group_id).ToList();
                    } 
	            }

                if (groups != null)
	            {
		            foreach (var group in groups)
                    {
                        groupmembers = ctx.group_member.Where(gm => gm.group_id == group.group_id).ToList();
                    } 
	            }

                if (groupmembers != null)
	            {
		            foreach (var groupmember in groupmembers)
                    {
                        users.Add(ctx.person.Where(p => p.person_id == groupmember.person_id).SingleOrDefault());
                    } 
	            }

                return users;
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