﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public partial class assignment
{
    public assignment()
    {
        this.assignment_component = new HashSet<assignment_component>();
        this.assignment_person = new HashSet<assignment_person>();
    }

    public int amt_id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string amt_tag { get; set; }
    public System.DateTime creation_date { get; set; }
    public string percent_done { get; set; }
    public bool finished { get; set; }
    public int privacy { get; set; }
    public Nullable<System.DateTime> edited { get; set; }
    public int project_id { get; set; }

    public virtual ICollection<assignment_component> assignment_component { get; set; }
    public virtual ICollection<assignment_person> assignment_person { get; set; }
    public virtual project project { get; set; }
}

public partial class assignment_component
{
    public int amtc_id { get; set; }
    public string name { get; set; }
    public bool finished { get; set; }
    public Nullable<System.DateTime> edited { get; set; }
    public int amt_id { get; set; }
    public int project_id { get; set; }

    public virtual assignment assignment { get; set; }
}

public partial class assignment_person
{
    public int assignment_person_id { get; set; }
    public int amt_id { get; set; }
    public int project_id { get; set; }
    public int person_id { get; set; }

    public virtual assignment assignment { get; set; }
    public virtual person person { get; set; }
}

public partial class group
{
    public group()
    {
        this.group_member = new HashSet<group_member>();
        this.project_group = new HashSet<project_group>();
    }

    public int group_id { get; set; }
    public string name { get; set; }
    public string group_tag { get; set; }
    public string creator { get; set; }
    public int privacy { get; set; }
    public System.DateTime creation_date { get; set; }
    public Nullable<System.DateTime> edited { get; set; }
    public Nullable<int> organization_org_id { get; set; }

    public virtual ICollection<group_member> group_member { get; set; }
    public virtual organization organization { get; set; }
    public virtual ICollection<project_group> project_group { get; set; }
}

public partial class group_member
{
    public int group_member_id { get; set; }
    public int group_id { get; set; }
    public int person_id { get; set; }
    public int grouprole_id { get; set; }

    public virtual group group { get; set; }
    public virtual person person { get; set; }
    public virtual group_role group_role { get; set; }
}

public partial class group_role
{
    public group_role()
    {
        this.group_member = new HashSet<group_member>();
    }

    public int grouprole_id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int @class { get; set; }

    public virtual ICollection<group_member> group_member { get; set; }
}

public partial class login
{
    public int login_id { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string salt { get; set; }
}

public partial class organization
{
    public organization()
    {
        this.group = new HashSet<group>();
    }

    public int org_id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int privacy { get; set; }
    public System.DateTime creation_date { get; set; }
    public string org_tag { get; set; }

    public virtual ICollection<group> group { get; set; }
}

public partial class person
{
    public person()
    {
        this.assignment_person = new HashSet<assignment_person>();
        this.group_member = new HashSet<group_member>();
    }

    public int person_id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public int privacy { get; set; }
    public System.DateTime creation_date { get; set; }
    public Nullable<System.DateTime> edited { get; set; }
    public int role_id { get; set; }

    public virtual ICollection<assignment_person> assignment_person { get; set; }
    public virtual ICollection<group_member> group_member { get; set; }
    public virtual role role { get; set; }
}

public partial class project
{
    public project()
    {
        this.assignment = new HashSet<assignment>();
        this.project_group = new HashSet<project_group>();
    }

    public int project_id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string project_tag { get; set; }
    public System.DateTime creation_date { get; set; }
    public Nullable<System.DateTime> due_date { get; set; }
    public string percent_done { get; set; }
    public bool finished { get; set; }
    public string picture_url { get; set; }
    public int privacy { get; set; }
    public Nullable<System.DateTime> edited { get; set; }

    public virtual ICollection<assignment> assignment { get; set; }
    public virtual ICollection<project_group> project_group { get; set; }
}

public partial class project_group
{
    public int project_group_id { get; set; }
    public int project_id { get; set; }
    public int group_id { get; set; }

    public virtual group group { get; set; }
    public virtual project project { get; set; }
}

public partial class role
{
    public role()
    {
        this.person = new HashSet<person>();
    }

    public int role_id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int @class { get; set; }

    public virtual ICollection<person> person { get; set; }
}
