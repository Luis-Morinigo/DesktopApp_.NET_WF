//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFS_Latam_cashApplicationTool.Models
{
    using System;
    
    public partial class SP_SELECTFBL5N_Result
    {
        public string Company_Code { get; set; }
        public Nullable<decimal> Account { get; set; }
        public string Document_Type { get; set; }
        public string Document_Type_Description { get; set; }
        public Nullable<decimal> Document_Number { get; set; }
        public string Reference { get; set; }
        public Nullable<System.DateTime> Document_Date { get; set; }
        public Nullable<System.DateTime> Baseline_Payment_Dte { get; set; }
        public Nullable<System.DateTime> Net_due_date { get; set; }
        public Nullable<decimal> Arrears_for_discount_1 { get; set; }
        public Nullable<decimal> Amount_in_doc__curr_ { get; set; }
        public string Document_currency { get; set; }
        public Nullable<decimal> Amount_in_local_currency { get; set; }
        public string Local_Currency { get; set; }
        public string Text { get; set; }
    }
}
