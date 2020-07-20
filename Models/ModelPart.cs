using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewModelEx_S1.Models
{
    public class ModelPart
    {
        public string MotherDrawing { get; set; }
        public string NodeDrawing { get; set; }
        public string NodeModelName { get; set; }
        public string NodePartName { get; set; }
        public string NodeRevision { get; set; }
        public string NodeOtherComment { get; set; }
        public string SubId { get; set; }
    }
}