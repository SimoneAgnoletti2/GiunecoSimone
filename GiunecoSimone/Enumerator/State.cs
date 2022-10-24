using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GiunecoSimone.Enumerator
{
    public enum State
    {
        [Display(Name = "Backlog")]
        [Description("Backlog")]
        Backlog = 1,
        [Display(Name = "In Progress")]
        [Description("In Progress")]
        Attivi = 2,
        [Display(Name = "Completed")]
        [Description("Completed")]
        Completati = 3,
        [Display(Name = "Tutti")]
        [Description("Tutti")]
        Tutti = 4,
    }
}