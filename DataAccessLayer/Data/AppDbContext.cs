using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection.Emit;


namespace DataAccessLayer
{

    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

          
            builder.Entity<EnrollProjectsUsers>()
                .HasKey(epu => new { epu.userID, epu.porjectID });

            builder.Entity<EnrollProjectsUsers>()
                .HasOne(epu => epu.User)
                .WithMany()
                .HasForeignKey(epu => epu.userID).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<EnrollProjectsUsers>()
                .HasOne(epu => epu.Project)
                .WithMany(p => p.EnrollUsers)
                .HasForeignKey(epu => epu.porjectID).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProfileInfo>()
           .HasOne(p => p.User)
           .WithOne(u => u.ProfileInfo)
           .HasForeignKey<ProfileInfo>(p => p.UserId);

                    builder.Entity<Specialization>()
            .HasOne(s => s.College)
            .WithMany(c => c.Specializations)
            .HasForeignKey(s => s.CollegeId);

            builder.Entity<Notification>()
          .HasOne(n => n.User)
          .WithMany(u => u.Notifications)
          .HasForeignKey(n => n.UserId)
          .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithMany()
                .HasForeignKey(n => n.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Notification>()
            .HasOne(n => n.Project)
            .WithMany()
            .HasForeignKey(n => n.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Participation>()
           .HasOne(p => p.User)
           .WithMany(u => u.Participations)
           .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Participation>()
                .HasOne(p => p.Project)
                .WithMany()
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            // Project → Team (1:1)
            builder.Entity<Team>()
                .HasOne(t => t.Project)
                .WithOne(p => p.Team)
                .HasForeignKey<Team>(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Team → Tasks (1:M)
            builder.Entity<TeamTask>()
                .HasOne(t => t.Team)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
                        builder.Entity<ChatRoomUser>()
                .HasIndex(x => new { x.ChatRoomId, x.UserId })
                .IsUnique();
                        builder.Entity<ChatRoom>()
                .HasMany(c => c.Members)
                .WithOne(m => m.ChatRoom)
                .HasForeignKey(m => m.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade);
                       builder.Entity<ChatRoomUser>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                        builder.Entity<Message>()
                .HasOne(m => m.ChatRoom)
                .WithMany()
                .HasForeignKey(m => m.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade);
                        builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<UniversityMajor> UniversityMajor { get; set; }
        public DbSet<ProfileInfo> ProfileInfos { get; set; }

        public DbSet<College> Colleges { get; set; }

        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<EnrollProject> EnrollProjects { get; set; }

        public DbSet<EnrollProjectsUsers> EnrollProjectsUsers { get; set; }


        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Participation> Participations { get; set; }
        public DbSet<TeamTask> TeamTasks { get; set; }

        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatRoomUser> ChatRoomUsers { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }

    }
    public class Message
    {
        public int Id { get; set; }

        public int ChatRoomId { get; set; }
        public ChatRoom? ChatRoom { get; set; }

        public int SenderId { get; set; }
        public ApplicationUser? Sender { get; set; }

        public string? Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
    public class ChatRoomUser
    {
        public int Id { get; set; }

        public int ChatRoomId { get; set; }
        public ChatRoom? ChatRoom { get; set; }

        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public DateTime JoinedAt { get; set; }
    }
    public class ChatRoom
    {
        public int Id { get; set; }

        public string? Name { get; set; }// اختياري (مثل Group Name)

        public bool IsGroup { get; set; } // true = group chat / false = private
        public string? RoomKey { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ChatRoomUser> Members { get; set; } = new List<ChatRoomUser>();
    }

    public class TeamTask
    {
        [Key]
        public int TaskId { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public string TaskName { get; set; }

        public string? Description { get; set; }

        public bool IsDone { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        public string Name { get; set; }

        // 1 Project → 1 Team (حسب تصميمك)
        public int ProjectId { get; set; }
        public EnrollProject Project { get; set; }

        public ICollection<TeamTask> Tasks { get; set; } = new List<TeamTask>();
    }

    [Table("universitymajor")]
    public class UniversityMajor
    {
        [Key]
        public int universitymajorID { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

    
       
    }

    [Table("ProfileInfo")]
    public class ProfileInfo
    {
        public int Id { get; set; }

        public string? Description { get; set; }
        public string? skills { get; set; }
        public int? rating { get; set; }

        // FK to User (One-to-One)
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string? RoleName { get; set; }
    }

    [Table("College")]
    public class College
    {
        public int CollegeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public ICollection<Specialization>? Specializations { get; set; }
        public ICollection<ProfileInfo>? Profiles { get; set; }
    }
    [Table("EnrollProjects")]
    public class EnrollProject
    {
        [Key]
        public int ProjectID { get; set; }

        [MaxLength(250)]
        public string? Name { get; set; }

        [MaxLength(300)]
        public string? Descriptions { get; set; }

        [MaxLength(100)]
        public string? Rating { get; set; }
        [MaxLength(100)]

        public string? ProjectType { get; set; }
        [MaxLength(100)]
        public string? status { get; set; }
        public bool? isGraduationProject { get; set; }

        public string? TeamType { get; set; }
        public string? ProjectLocation { get; set; }
        public Team? Team { get; set; }
        public DateTime? EndDate { get; set; }


        [MaxLength(250)]
        public string? Skills { get; set; }

        public int? numberOFAvailableSeats { get; set; }
        public int? AvailableSeats { get; set; }

        // 🔗 Navigation Property: Users enrolled
        public ICollection<EnrollProjectsUsers>? EnrollUsers { get; set; }
    }

    [Table("EnrollProjectsUsers")]
    public class EnrollProjectsUsers
    {
        public int? userID { get; set; }
        public int? porjectID { get; set; }

        [MaxLength(50)]
        public string PojectRole { get; set; }

     
        public int? Rating { get; set; }

        // 🔗 Navigation Properties
        [ForeignKey("userID")]
        public ApplicationUser? User { get; set; }  // إذا تستخدم Identity

        [ForeignKey("porjectID")]
        public EnrollProject? Project { get; set; }
    }

    public class ApplicationUser : IdentityUser<int> // أو <string> حسب نوع الـ PK عندك
    {
        // خصائص إضافية إذا تريد
        public string? FullName { get; set; }

        public ProfileInfo ProfileInfo { get; set; }
        public string? ImagePath { get; set; }
        public string? githubUrl { get; set; }
        public int? SpecializationId { get; set; }
        public string? Description { get; set; }
        public string? skills { get; set; }
        public Specialization? Specialization { get; set; }

        public ICollection<Participation> Participations { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }


    [Table("Specialization")]
    public class Specialization
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public int CollegeId { get; set; }
        public College College { get; set; }
    }

        [Table("Notification")]
        public class Notification
        {
            public int Id { get; set; }

            // 👤 الشخص الذي استلم الإشعار (Receiver)
            public int UserId { get; set; }
            public ApplicationUser User { get; set; }

            // 👤 الشخص الذي أرسل الإشعار (Sender)
            public int? SenderId { get; set; }
            public ApplicationUser? Sender { get; set; }

            public string Type { get; set; }

            public string Title { get; set; }

            public string Message { get; set; }

            public bool IsRead { get; set; } = false;

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            // 📌 related project (اختياري)
            public int? ProjectId { get; set; }
            public EnrollProject Project { get; set; }
        }

    public class Participation
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ProjectId { get; set; }
        public EnrollProject Project { get; set; }

        public string Status { get; set; } = "pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}