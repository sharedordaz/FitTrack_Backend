using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
  public class Routine
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [JsonPropertyName("user_id")]
    public int User_id { get; set; }

    [Required]
    [JsonPropertyName("activity_id")]
    public int Activity_id { get; set; }

    [Required]
    [MaxLength(50)]
    [JsonPropertyName("value")]
    public string Value { get; set; }

    [Required]
    [MaxLength(20)]
    [JsonPropertyName("unit")]
    public string Unit { get; set; }

    [JsonPropertyName("repetitions")]
    public int Repetitions { get; set; }

    public DateTime? Date { get; set; }

    [Required]
    public RoutineType Type { get; set; }

    // Mark navigation properties with [JsonIgnore] and nullable type
    [NotMapped]
    [JsonIgnore]
    public User? User { get; set; }

    [NotMapped]
    [JsonIgnore]
    public Activity? Activity { get; set; }

    public Routine() { }

    public Routine(int id, int user_id, int activity_id, string value, string unit, int repetitions = 0, DateTime? date = null, RoutineType type = RoutineType.Done)
    {
      this.Id = id;
      this.User_id = user_id;
      this.Activity_id = activity_id;
      this.Value = value;
      this.Unit = unit;
      this.Repetitions = repetitions;
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