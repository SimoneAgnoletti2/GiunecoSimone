using GiunecoSimone.Enumerator;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GiunecoSimone.Models
{
    public partial class Tasks
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Stato")]
        public int State { get; set; }
    }
}
