using GiunecoSimone.Enumerator;
using System;

namespace GiunecoSimone.Models
{
    public partial class Tasks
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int State { get; set; }
    }
}
