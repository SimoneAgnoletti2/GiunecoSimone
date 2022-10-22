using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiunecoSimone.Models
{
    [MetadataType(typeof(TasksMetaData))]
    public partial class Tasks
    {
    }

    public class TasksMetaData
    {
        [Display(Name = "Titolo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Titolo obbligatorio")]
        [StringLength(20, MinimumLength =3)]
        public string Title { get; set; }

        [Display(Name = "Descrizione")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Descrizione obbligatoria")]
        [StringLength(20,MinimumLength = 1)]
        public string Description { get; set; }

        [Display(Name = "Data inserimento")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}