using Microsoft.EntityFrameworkCore;

using Backend.Models;

namespace Backend.Data
{
  public class FitTrackContext : DbContext
  {
    public FitTrackContext(DbContextOptions<FitTrackContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Routine> Routines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>().ToTable("users", "FitTrack_Database");
      modelBuilder.Entity<Activity>().ToTable("activities", "FitTrack_Database");
      modelBuilder.Entity<Routine>().ToTable("routine", "FitTrack_Database");

      modelBuilder.Entity<Routine>()
     .Property(r => r.Type)
     .HasConversion<string>()
     .HasMaxLength(10);

      modelBuilder.Entity<Routine>()
          .HasOne(r => r.User)
          .WithMany(u => u.Routines)
          .HasForeignKey(r => r.User_id);

      modelBuilder.Entity<Routine>()
          .HasOne(r => r.Activity)
          .WithMany(a => a.Routines)
          .HasForeignKey(r => r.Activity_id);
    }
  }
}