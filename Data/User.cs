using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate.Types.Relay;

namespace husarbeid.Data
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Username { get; set; }

        [Required]
        public string? HashedPassword { get; set; }

        public DateTime? BirthDate { get; set; }

        [ID(nameof(Family))]
        public int? FamilyId { get; set; }
        public Family? Family { get; set; }

        public ICollection<FamilyTask> UserTasks { get; set; } =
            new List<FamilyTask>();

        public ICollection<FamilyTask> UserCreatedTasks { get; set; } =
            new List<FamilyTask>();
    }
}