using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace GiunecoSimone.Class
{
    public class UserLogin
    {
        [Display(Name = "Indirizzo Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Indirizzo email obbligatorio")]
        public string EmailID { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password obbligatoria")]
        [DataType(DataType.Password)]
        public string Pwd { get; set; }

    }
}