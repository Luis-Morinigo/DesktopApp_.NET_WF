//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFS_Latam_cashApplicationTool.Models.DBHistoricalDocNumbers
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SSC_Finance_DWEntitiesHistoricalDocNumbers : DbContext
    {
        public SSC_Finance_DWEntitiesHistoricalDocNumbers()
            : base("name=SSC_Finance_DWEntitiesHistoricalDocNumbers")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CASH_APPLICATION___Historical_Document_Numbers> CASH_APPLICATION___Historical_Document_Numbers { get; set; }
    }
}
