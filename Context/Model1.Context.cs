﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewModelEx_S1.Context
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OCTIIS_WEBAPPEntities3 : DbContext
    {
        public OCTIIS_WEBAPPEntities3()
            : base("name=OCTIIS_WEBAPPEntities3")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Eng_AccountUser> Eng_AccountUser { get; set; }
        public virtual DbSet<Eng_AddDepartmentModel> Eng_AddDepartmentModel { get; set; }
        public virtual DbSet<Eng_Clusifiation> Eng_Clusifiation { get; set; }
        public virtual DbSet<Eng_Customer> Eng_Customer { get; set; }
        public virtual DbSet<Eng_fixPermission> Eng_fixPermission { get; set; }
        public virtual DbSet<Eng_JobItemList> Eng_JobItemList { get; set; }
        public virtual DbSet<Eng_ModelAction> Eng_ModelAction { get; set; }
        public virtual DbSet<Eng_ModelSub> Eng_ModelSub { get; set; }
        public virtual DbSet<Eng_newModel> Eng_newModel { get; set; }
        public virtual DbSet<Eng_PermissionSub> Eng_PermissionSub { get; set; }
        public virtual DbSet<Eng_PermissionUser> Eng_PermissionUser { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Eng_PermissionMainmenu> Eng_PermissionMainmenu { get; set; }
    }
}
