using System;
using System.ComponentModel.DataAnnotations;

namespace GiunecoSimone.Enumerator
{
    public enum State
    {
        [Display(Name = "Backlog")]
        Backlog = 1,
        [Display(Name = "In Progress")]
        Attivi = 2,
        [Display(Name = "Completed")]
        Completati = 3,
        [Display(Name = "Tutti")]
        Tutti = 4,
    }

    public enum StateEdit
    {
        [Display(Name = "Backlog")]
        Backlog = 1,
        [Display(Name = "In Progress")]
        Attivi = 2,
        [Display(Name = "Completed")]
        Completati = 3,
    }
}