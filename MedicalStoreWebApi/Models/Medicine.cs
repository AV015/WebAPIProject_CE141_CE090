//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedicalStoreWebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Medicine
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
    }
}