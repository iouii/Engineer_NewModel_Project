using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewModelEx_S1.Models
{
    public class ModelUser
    {
        public string EmpId { get; set; }
        public string EmpUserName { get; set; }
        public string EmpPassWord { get; set; }
        public string EmpFName { get; set; }
        public string EmpLName { get; set; }
        public string EmpNickName { get; set; }
        public string EmpDep { get; set; }
        public string EmpPos { get; set; }
        public string EmpEmail { get; set; }
        public string EmpPermission { get; set; }
    }
    public class ModelDepartment
    {
        public int idDept { get; set; }
        public string department { get; set; }
    }
    public class ModelPosition
    {
        public int idPos { get; set; }
        public string Position { get; set; }
    }
}