using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Routine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public int user_id { get; set; }

        [Required]
        public int activity_id { get; set; } // Renamed to follow PascalCase

        [Required]
        [MaxLength(50)]
        public string value { get; set; }

        [Required]
        [MaxLength(20)]
        public string unit { get; set; }

        public int repetitions { get; set; }

        public DateTime? Date { get; set; } // Changed to DateTime? for better type safety

        [Required]
        public RoutineType Type { get; set; } // Enum for type safety

        // Add navigation properties
        public User User { get; set; } // Navigation property for User
        public Activity Activity { get; set; } // Navigation property for Activity


        public Routine(int id, int user_id, int activity_id, string value, string unit, int repetitions = 0, DateTime? date = null, RoutineType type = RoutineType.Done)
        {
            this.Id = id;
            this.user_id = user_id;
            this.activity_id = activity_id;
            this.value = value;
            this.unit = unit;
            this.repetitions = repetitions;
            this.Date = date;
            this.Type = type;
        }
    }

    public enum RoutineType
    {
        Goal, // Represents "goal"
        Done  // Represents "done"
    }
}