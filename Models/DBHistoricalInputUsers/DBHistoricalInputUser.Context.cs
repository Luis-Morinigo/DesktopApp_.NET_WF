//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFS_Latam_cashApplicationTool.Models.DBHistoricalInputUsers
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SSC_Finance_DWEntitiesHistoricalInputUsers : DbContext
    {
        public SSC_Finance_DWEntitiesHistoricalInputUsers()
            : base("name=SSC_Finance_DWEntitiesHistoricalInputUsers")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CASH_APPLICATION___Historical_Input_Users> CASH_APPLICATION___Historical_Input_Users { get; set; }
    }
}
