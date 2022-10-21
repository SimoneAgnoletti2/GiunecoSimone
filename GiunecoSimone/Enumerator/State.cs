using System;
using System.ComponentModel.DataAnnotations;

namespace GiunecoSimone.Enumerator
{
    public enum State
    {
        [Display(Name = "Backlog")]
        Backlog = 1,
        [Display(Name = "In Progress")]
        InProgress = 2,
        [Display(Name = "Completed")]
        Completed = 3,
    }
}