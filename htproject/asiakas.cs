//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JAMK.ICT
{
    using System;
    using System.Collections.Generic;
    
    public partial class asiakas
    {
        public asiakas()
        {
            this.tilaus = new HashSet<tilaus>();
        }
    
        public int idasiakas { get; set; }
        public string nimi { get; set; }
        public string osoite { get; set; }
        public string asiakascol { get; set; }
    
        public virtual ICollection<tilaus> tilaus { get; set; }
    }
}
