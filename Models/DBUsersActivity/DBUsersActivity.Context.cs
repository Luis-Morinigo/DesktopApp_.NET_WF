﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFS_Latam_cashApplicationTool.Models.DBUsersActivity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EntitiesUsersActivity : DbContext
    {
        public EntitiesUsersActivity()
            : base("name=EntitiesUsersActivity")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CASH_APPLICATION___Users_Activity> CASH_APPLICATION___Users_Activity { get; set; }
    }
}
