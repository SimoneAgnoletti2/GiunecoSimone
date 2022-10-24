using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace GiunecoSimone.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class Users
    {
        public string ConfirmPwd { get; set; }
    }
    public class UserMetaData
    {
        [Display(Name = "Nome")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nome obbligatorio")]
        public string FirstName { get; set; }

        [Display(Name = "Cognome")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cognome obbligatorio")]
        public string LastName { get; set; }

        [Display(Name = "Indirizzo Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Indirizzo Email obbligatorio")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password obbligatoria")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La password deve essere almeno di 6 caratteri")]
        public string Pwd { get; set; }

        [Display(Name = "Conferma password")]
        [DataType(DataType.Password)]
        [Compare("Pwd", ErrorMessage = "Le due password non corrispondono")]
        public string ConfirmPwd { get; set; }

    }
}