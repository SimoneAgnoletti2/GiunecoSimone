using System;

namespace GiunecoSimone.Models
{
    
    public partial class UsersTasks
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdUser { get; set; }
        public Guid IdTask { get; set; }
        public decimal WorkedHour { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CommentDate { get; set; }
    }
}
