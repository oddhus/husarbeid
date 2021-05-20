using System;
using System.ComponentModel.DataAnnotations;

namespace husarbeid.Data
{
    public class FamilyTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string? ShortDescription { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int? Payment { get; set; }
        public bool? isCompleted { get; set; }
        public Family? Family { get; set; }
        public int? FamilyId { get; set; }
        public User? AssignedTo { get; set; }
        public int? AssignedToId { get; set; }
        public User? CreatedBy { get; set; }
        public int? CreatedById { get; set; }
    }
}