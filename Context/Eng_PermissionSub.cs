//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Eng_PermissionSub
    {
        public int id_persub { get; set; }
        public string name_controler { get; set; }
        public int id_permissionuser { get; set; }
        public string name_function { get; set; }
        public string name_action { get; set; }
        public string name_statusfc { get; set; }
        public Nullable<int> id_mainmenu { get; set; }
    }
}
