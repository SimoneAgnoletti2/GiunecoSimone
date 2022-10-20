using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiunecoSimone.Models
{
    public class ResetPwd
    {
        [Display(Name ="Nuova Password")]
        [Required(ErrorMessage = "Nuova password obbligatoria", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Conferma Password")]        
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Le due password non corrispondono")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetCode { get; set; }
    }
}