using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewModelEx_S1.Models
{
    public class ModelAdd
    {

        [Required]
        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Required]       
        [Display(Name = "Model Name")]
        public string ModelName { get; set; }

        [Required]
        [Display(Name = "Program Name")]
        public string ProgramName { get; set; }

        [Required]
        [Display(Name = "Part Name")]
        public string PartName { get; set; }
        
        [Required]
        [Display(Name = "Drawing#")]
        public string DrawingNo { get; set; }

        [Required]
        [Display(Name = "Other Info")]
        public string OderInfo { get; set; }

        [Required]
        [Display(Name = "Date")]
        public string DateModel { get; set; }

    }
}