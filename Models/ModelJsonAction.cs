using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewModelEx_S1.Models
{
    public class ModelJsonAction
    {

        public List<jjuser> jobaction { get; set; }

           
    }

    public class jjuser
    {
        public string JobName { get; set; }
        public string JobInName { get; set; }
        public string JobDate { get; set; }
        public string JobCom { get; set; }
        public string JobEmail { get; set; }

     
    }
}