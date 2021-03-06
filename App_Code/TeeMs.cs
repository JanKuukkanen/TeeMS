﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public partial class assignment
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public assignment()
    {
        this.assignment_component = new HashSet<assignment_component>();
        this.assignment_person = new HashSet<assignment_person>();
        this.comment = new HashSet<comment>();
        this.connection = new HashSet<connection>();
    }

    public int amt_id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public Nullable<int> amt_tag { get; set; }
    public System.DateTime creation_date { get; set; }
    public Nullable<System.DateTime> assignment_due_date { get; set; }
    public string percent_done { get; set; }
    public bool finished { get; set; }
    public int privacy { get; set; }
    public Nullable<System.DateTime> edited { get; set; }
    public int project_id { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<assignment_component> assignment_component { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<assignment_person> assignment_person { get; set; }
    public virtual project project { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<comment> comment { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<connection> connection { get; set; }
}

public partial class assignment_component
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public assignment_component()
    {
        this.assignment_component_person = new HashSet<assignment_component_person>();
    }

    public int amtc_id { get; set; }
    public string name { get; set; }
    public bool finished { get; set; }
    public Nullable<System.DateTime> edited { get; set; }
    public int amt_id { get; set; }
    public int project_id { get; set; }

    public virtual assignment assignment { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<assignment_component_person> assignment_component_person { get; set; }
}

public partial class assignment_component_person
{
    public int assignment_component_person_id { get; set; }
    public int amtc_id { get; set; }
    public int amt_id { get; set; }
    public int project_id { get; set; }
    public int person_id { get; set; }

    public virtual assignment_component assignment_component { get; set; }
    public virtual person person { get; set; }
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

public partial class comment
{
    public int comment_id { get; set; }
    public string comment_content { get; set; }
    public System.DateTime creation_date { get; set; }
    public Nullable<System.DateTime> edited { get; set; }
    public int person_id { get; set; }
    public int project_id { get; set; }
    public Nullable<int> amt_id { get; set; }
    public Nullable<int> assignment_project_id { get; set; }

    public virtual assignment assignment { get; set; }
    public virtual person person { get; set; }
    public virtual project project { get; set; }
}

public partial class connection
{
    public int connection_id { get; set; }
    public bool connected { get; set; }
    public string connection_username { get; set; }
    public Nullable<System.DateTime> connection_time { get; set; }
    public int person_id { get; set; }
    public Nullable<int> group_id { get; set; }
    public Nullable<int> project_id { get; set; }
    public Nullable<int> amt_id { get; set; }
    public Nullable<int> assignment_project_id { get; set; }

    public virtual assignment assignment { get; set; }
    public virtual group group { get; set; }
    public virtual person person { get; set; }
    public virtual project project { get; set; }
}

public partial class group
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public group()
    {
        this.connection = new HashSet<connection>();
        this.group_member = new HashSet<group_member>();
        this.invite = new HashSet<invite>();
        this.message = new HashSet<message>();
        this.project_group = new HashSet<project_group>();
        this.project_person = new HashSet<project_person>();
    }

    public int group_id { get; set; }
    public string name { get; set; }
    public Nullable<int> group_tag { get; set; }
    public string creator { get; set; }
    public int privacy { get; set; }
    public System.DateTime creation_date { get; set; }
    public Nullable<System.DateTime> edited { get; set; }
    public string group_picture_url { get; set; }
    public Nullable<int> organization_org_id { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<connection> connection { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<group_member> group_member { get; set; }
    public virtual organization organization { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<invite> invite { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<message> message { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<project_group> project_group { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<project_person> project_person { get; set; }
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public group_role()
    {
        this.group_member = new HashSet<group_member>();
    }

    public int grouprole_id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int @class { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<group_member> group_member { get; set; }
}

public partial class invite
{
    public int invite_id { get; set; }
    public string invite_content { get; set; }
    public int person_id { get; set; }
    public int group_id { get; set; }

    public virtual group group { get; set; }
    public virtual person person { get; set; }
}

public partial class login
{
    public int login_id { get; set; }
    public string login_name { get; set; }
    public string password { get; set; }
    public string salt { get; set; }
}

public partial class message
{
    public int message_id { get; set; }
    public string message_content { get; set; }
    public System.DateTime creation_date { get; set; }
    public int person_id { get; set; }
    public int group_id { get; set; }

    public virtual group group { get; set; }
    public virtual person person { get; set; }
}

public partial class organization
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public organization()
    {
        this.group = new HashSet<group>();
    }

    public int org_id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int privacy { get; set; }
    public System.DateTime creation_date { get; set; }
    public int org_tag { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<group> group { get; set; }
}

public partial class person
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public person()
    {
        this.assignment_component_person = new HashSet<assignment_component_person>();
        this.assignment_person = new HashSet<assignment_person>();
        this.comment = new HashSet<comment>();
        this.connection = new HashSet<connection>();
        this.group_member = new HashSet<group_member>();
        this.invite = new HashSet<invite>();
        this.message = new HashSet<message>();
        this.project_person = new HashSet<project_person>();
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<assignment_component_person> assignment_component_person { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<assignment_person> assignment_person { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<comment> comment { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<connection> connection { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<group_member> group_member { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<invite> invite { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<message> message { get; set; }
    public virtual role role { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<project_person> project_person { get; set; }
}

public partial class project
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public project()
    {
        this.assignment = new HashSet<assignment>();
        this.comment = new HashSet<comment>();
        this.connection = new HashSet<connection>();
        this.project_group = new HashSet<project_group>();
        this.project_person = new HashSet<project_person>();
    }

    public int project_id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public Nullable<int> project_tag { get; set; }
    public string project_creator { get; set; }
    public System.DateTime creation_date { get; set; }
    public Nullable<System.DateTime> due_date { get; set; }
    public double percent_done { get; set; }
    public bool finished { get; set; }
    public string picture_url { get; set; }
    public int privacy { get; set; }
    public Nullable<System.DateTime> edited { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<assignment> assignment { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<comment> comment { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<connection> connection { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<project_group> project_group { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<project_person> project_person { get; set; }
}

public partial class project_group
{
    public int project_group_id { get; set; }
    public int project_id { get; set; }
    public int group_id { get; set; }
    public bool supporting { get; set; }

    public virtual group group { get; set; }
    public virtual project project { get; set; }
}

public partial class project_person
{
    public int project_person_id { get; set; }
    public int project_id { get; set; }
    public int person_id { get; set; }
    public int group_id { get; set; }
    public bool project_person_supporting { get; set; }

    public virtual group group { get; set; }
    public virtual person person { get; set; }
    public virtual project project { get; set; }
}

public partial class role
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public role()
    {
        this.person = new HashSet<person>();
    }

    public int role_id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int @class { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<person> person { get; set; }
}
