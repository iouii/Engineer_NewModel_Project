using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewModelEx_S1.Models
{
    public class ModelLogin
    {

        public string NameFunction { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string idPermission { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string statusMenu { get; set; }
        public string idmainMenu { get; set; }


    }
    public class ModelMainMenu
    {
        public string idMainMenu { get; set; }
        public string MainMenuname { get; set; }

    }

    public class ModelChkSetRep
    {
        public string idPermis { get; set; }
        public string idSetRep { get; set; }
        public string idMain { get; set; }
    }
}