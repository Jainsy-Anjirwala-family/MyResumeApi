// using Microsoft.EntityFrameworkCore;

// // public class WorkExperience {
// //     public int work_id { get; set; }
// //     public string company_name { get; set; } = "";
// //     public string job_title { get; set; } = "";
// //     public string year_id { get; set; } = "";
// // }


// // public class AppDbContext : DbContext {
// //     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

// //     public DbSet<WorkExperience> WorkExperiences { get; set; }

// //     protected override void OnModelCreating(ModelBuilder modelBuilder) {
// //         // Mapping to your specific table name and primary key
// //         modelBuilder.Entity<WorkExperience>().ToTable("workexperience").HasKey(w => w.work_id);
// //     }
// // }


// public class ProfileResponseDto
// {
//     public List<Persondetail> PersonDetails { get; set; } = new();
//     public List<Educationdetail> EducationDetails { get; set; } = new();
//     public List<Technicaldetail> TechnicalDetails { get; set; } = new();
//     public List<Achievementdetail> AchievementDetails { get; set; } = new();
//     public List<Achievementdesdetail> AchievementDesDetails { get; set; } = new();
//     public List<WorkExperience> Experience { get; set; } = new();
//     public List<WorkExperienceDes> ExperienceDes { get; set; } = new();
// }



// public class AppDbContext : DbContext {
//     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//     public DbSet<WorkExperience> WorkExperiences { get; set; }

//     protected override void OnModelCreating(ModelBuilder modelBuilder) {
//         // Mapping to your specific table name and primary key
//         modelBuilder.Entity<WorkExperience>().ToTable("workexperience").HasKey(w => w.work_id);
//         modelBuilder.Entity<WorkExperienceDes>().ToTable("workexperiencedes").HasKey(w => w.work_id);
//         modelBuilder.Entity<Persondetail>().ToTable("persondetail").HasKey(p => p.person_id);
//         modelBuilder.Entity<Educationdetail>().ToTable("educationdetail").HasKey(e => e.education_id);
//         modelBuilder.Entity<Technicaldetail>().ToTable("technicaldetail").HasKey(t => t.technical_id);
//         modelBuilder.Entity<Achievementdetail>().ToTable("achievementdetail").HasKey(a => a.achievement_id);
//         modelBuilder.Entity<Achievementdesdetail>().ToTable("achievementdesdetail").HasKey(a => a.achievement_id);
//     }
// }
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyResumeApi.Models
{
    // The DTO for your custom API response
    public class ProfileResponseDto
    {
        public int Status { get; set; }
        public bool Ok { get; set; }    
        public Persondetail PersonDetails { get; set; }
        // public List<Persondetail> PersonDetails { get; set; } = new();
        public List<Educationdetail> EducationDetails { get; set; } = new();
        public List<Technicaldetail> TechnicalDetails { get; set; } = new();
        public List<Achievementdetail> AchievementDetails { get; set; } = new();
        // public List<Achievementdesdetail> AchievementDesDetails { get; set; } = new();
        public List<WorkExperience> Experience { get; set; } = new();
        // public List<WorkExperienceDes> ExperienceDes { get; set; } = new();
    }

    // Individual Table Models
    public class Persondetail {
        public int person_id { get; set; }
        public string first_name { get; set; } = "";
        public string last_name { get; set; } = "";
        public long mobile_no { get; set; }
        public string email_id { get; set; } = "";
        public string job_title { get; set; } = "";
    }

    public class WorkExperience {
        public int work_id { get; set; }
        public string company_name { get; set; } = "";
        public string job_title { get; set; } = "";
        public string year_id { get; set; } = "";
        public string symbol_name { get; set; } = "";
        [NotMapped] // <--- ADD THIS LINE
        public List<string> description { get; set; } = new List<string>();
    }

    public class WorkExperienceDes {
        public int work_id { get; set; }
        public string company_name { get; set; } = "";
        public string job_description { get; set; } = "";
        public string description { get; set; } = "";
    }

    public class Educationdetail { 
            public int education_id { get; set; }
            public string symbol_name { get; set; } = "";
            public string educations_name { get; set; } = "";
            public string year_id { get; set; } = "";
            public string educations_field { get; set; } = "";
        }
    public class Technicaldetail { 
            public int technical_id { get; set; } 
            public string technical_detail { get; set; } = "";
            
        }
    public class Achievementdetail { 
            public int achievement_id { get; set; } 
            public string achievement_name { get; set; } = "";
            [NotMapped]
            public List<string> achievements_details { get; set; } = new List<string>();
        }
    public class Achievementdesdetail { 
            public int achievement_id { get; set; } 
            public string achievement_name { get; set; } = "";
            public string achievements_detail { get; set; } = "";
        }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Persondetail> PersonDetails { get; set; }
        public DbSet<Educationdetail> EducationDetails { get; set; }
        public DbSet<Technicaldetail> TechnicalDetails { get; set; }
        public DbSet<Achievementdetail> AchievementDetails { get; set; }
        public DbSet<Achievementdesdetail> AchievementDesDetails { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
        public DbSet<WorkExperienceDes> WorkExperienceDescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkExperience>().ToTable("workexperience").HasKey(w => w.work_id);
            modelBuilder.Entity<WorkExperienceDes>().ToTable("workexperiencedes").HasKey(w => w.work_id);
            modelBuilder.Entity<Persondetail>().ToTable("persondetail").HasKey(p => p.person_id);
            modelBuilder.Entity<Educationdetail>().ToTable("educationdetail").HasKey(e => e.education_id);
            modelBuilder.Entity<Technicaldetail>().ToTable("technicaldetail").HasKey(t => t.technical_id);
            modelBuilder.Entity<Achievementdetail>().ToTable("achievementdetail").HasKey(a => a.achievement_id);
            modelBuilder.Entity<Achievementdesdetail>().ToTable("achievementdesdetail").HasKey(a => a.achievement_id);
        }
    }
}