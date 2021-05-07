using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace husarbeid.Data
{
    public class Family
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? FamilyName { get; set; }

        public ICollection<User> Members { get; set; } =
           new List<User>();

        public ICollection<FamilyTask> Tasks { get; set; } =
            new List<FamilyTask>();
    }
}