using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewModelEx_S1.Models
{
    public class ModelPermission
    {
        public string idPer { get; set; }
        public string PerNameFun { get; set; }
        public string PerNameControl { get; set; }
        public string PerNameMainmanu { get; set; }
        public string PermissionName { get; set; }
        public string PerMainAction { get; set; }
        public string PerStatus { get; set; }
    }

    public class ModelSelPer
    {
        public string idPermission { get; set; }
        public string namePermission { get; set; }
    }
    public class ModelSelPermenu
    {
        public string idPermissionMenu { get; set; }
        public string namePermissionMenu { get; set; }
    }
}