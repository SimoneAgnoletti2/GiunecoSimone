using GiunecoSimone.Class;
using GiunecoSimone.Enumerator;
using GiunecoSimone.Models.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiunecoSimone.Models
{
    [MetadataType(typeof(EditTaskMetaData))]
    public class EditTask
    {
        
    }
    public class EditTaskMetaData
    {
        [Display(Name = "Titolo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Titolo obbligatorio")]
        [StringLength(40, MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Descrizione")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Descrizione obbligatoria")]
        [StringLength(100, MinimumLength = 1)]
        public string Description { get; set; }

        [Display(Name = "Data inserimento")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Stato")]
        public State State { get; set; }

        [Display(Name = "Ore lavorate")]
        [RegularExpression(@"[1-9][0-9]*", ErrorMessage = "Valore inserito non valido")]

        public int WorkedHour { get; set; } = 0;

        [Display(Name = "Totale ore lavorate")]
        [DataType(DataType.Text)]

        public int TotalWorkedHour { get; set; } = 0;
        public IEnumerable<Comments> Comments { get; set; }

        [Display(Name = "Team di sviluppo")]
        public IEnumerable<Users> Users { get; set; }
    }
}