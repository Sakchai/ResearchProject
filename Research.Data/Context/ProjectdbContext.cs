using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Research.Data;

namespace Research.Data
{
    public partial class ProjectdbContext : DbContext
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        public ProjectdbContext()
        {
        }
        public ProjectdbContext(string connection)
        {
            _connection = new SqlConnection(connection);
        }

        public ProjectdbContext(DbContextOptions<ProjectdbContext> options)
            : base(options)
        {
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set => _transaction = value; }

        public virtual DbSet<AcademicRank> AcademicRank { get; set; }
        public virtual DbSet<ActivityLogType> ActivityLogType { get; set; }
        public virtual DbSet<ActivityLog> ActivityLog { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Download> Download { get; set; }
        public virtual DbSet<EducationLevel> EducationLevel { get; set; }
        public virtual DbSet<EmailAccount> EmailAccount { get; set; }
        public virtual DbSet<Faculty> Faculty { get; set; }
        public virtual DbSet<FiscalSchedule> FiscalSchedule { get; set; }
        public virtual DbSet<Institute> Institute { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<MessageTemplate> MessageTemplate { get; set; }
        public virtual DbSet<Picture> Picture { get; set; }
        public virtual DbSet<PictureBinary> PictureBinary { get; set; }
        public virtual DbSet<Professor> Professor { get; set; }
        public virtual DbSet<Program> Program { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectHistory> ProjectHistory { get; set; }
        public virtual DbSet<ProjectProgress> ProjectProgress { get; set; }
        public virtual DbSet<ProjectResearcher> ProjectResearcher { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<ResearchIssue> ResearchIssue { get; set; }
        public virtual DbSet<Researcher> Researcher { get; set; }
        public virtual DbSet<ResearcherEducation> ResearcherEducation { get; set; }
        public virtual DbSet<ResearcherHistory> ResearcherHistory { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleProgram> RoleProgram { get; set; }
        public virtual DbSet<StrategyGroup> StrategyGroup { get; set; }
        public virtual DbSet<ScheduleTask> ScheduleTask { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<Title> Title { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connection.ConnectionString);
                //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ProjectApi;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}