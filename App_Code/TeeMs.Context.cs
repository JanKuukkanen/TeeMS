﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

public partial class TeeMsEntities : DbContext
{
    public TeeMsEntities()
        : base("name=TeeMsEntities")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }

    public virtual DbSet<assignment> assignment { get; set; }
    public virtual DbSet<assignment_component> assignment_component { get; set; }
    public virtual DbSet<assignment_component_person> assignment_component_person { get; set; }
    public virtual DbSet<assignment_person> assignment_person { get; set; }
    public virtual DbSet<comment> comment { get; set; }
    public virtual DbSet<connection> connection { get; set; }
    public virtual DbSet<group> group { get; set; }
    public virtual DbSet<group_member> group_member { get; set; }
    public virtual DbSet<group_role> group_role { get; set; }
    public virtual DbSet<invite> invite { get; set; }
    public virtual DbSet<login> login { get; set; }
    public virtual DbSet<message> message { get; set; }
    public virtual DbSet<organization> organization { get; set; }
    public virtual DbSet<person> person { get; set; }
    public virtual DbSet<project> project { get; set; }
    public virtual DbSet<project_group> project_group { get; set; }
    public virtual DbSet<project_person> project_person { get; set; }
    public virtual DbSet<role> role { get; set; }
}
