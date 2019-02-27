using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Project.Entity.Context
{
    public partial class ProjectdbContext : DbContext
    {
        public ProjectdbContext()
        {
        }

        public ProjectdbContext(DbContextOptions<ProjectdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AcademicPosition> AcademicPosition { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Download> Download { get; set; }
        public virtual DbSet<EducationLevel> EducationLevel { get; set; }
        public virtual DbSet<Faculty> Faculty { get; set; }
        public virtual DbSet<FiscalSchedule> FiscalSchedule { get; set; }
        public virtual DbSet<Institute> Institute { get; set; }
        public virtual DbSet<Keyword> Keyword { get; set; }
        public virtual DbSet<Picture> Picture { get; set; }
        public virtual DbSet<PictureBinary> PictureBinary { get; set; }
        public virtual DbSet<Professor> Professor { get; set; }
        public virtual DbSet<ProfessorAcademicPosition> ProfessorAcademicPosition { get; set; }
        public virtual DbSet<ProfessorAddress> ProfessorAddress { get; set; }
        public virtual DbSet<Program> Program { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectHistory> ProjectHistory { get; set; }
        public virtual DbSet<ProjectKeyword> ProjectKeyword { get; set; }
        public virtual DbSet<ProjectProgress> ProjectProgress { get; set; }
        public virtual DbSet<ProjectResearcher> ProjectResearcher { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<ResearchIssue> ResearchIssue { get; set; }
        public virtual DbSet<Researcher> Researcher { get; set; }
        public virtual DbSet<ResearcherAcademicPosition> ResearcherAcademicPosition { get; set; }
        public virtual DbSet<ResearcherAddresses> ResearcherAddresses { get; set; }
        public virtual DbSet<ResearcherEducation> ResearcherEducation { get; set; }
        public virtual DbSet<ResearcherHistory> ResearcherHistory { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleProgram> RoleProgram { get; set; }
        public virtual DbSet<Subdistrict> Subdistrict { get; set; }
        public virtual DbSet<Title> Title { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ProjectDb;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<AcademicPosition>(entity =>
            {
                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasColumnName("NameEN")
                    .HasMaxLength(200);

                entity.Property(e => e.NameTh)
                    .IsRequired()
                    .HasColumnName("NameTH")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Address1)
                    .HasColumnName("Address")
                    .HasMaxLength(400);

                entity.Property(e => e.AddressType)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode).HasMaxLength(5);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_Address_District");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_Address_Province");

                entity.HasOne(d => d.Subdistrict)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.SubdistrictId)
                    .HasConstraintName("FK_Address_Subdistrict");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.District)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_District_Province");
            });

            modelBuilder.Entity<Download>(entity =>
            {
                entity.Property(e => e.ContentType).HasMaxLength(50);

                entity.Property(e => e.DownloadUrl).HasMaxLength(256);

                entity.Property(e => e.Extension).HasMaxLength(10);

                entity.Property(e => e.Filename).HasMaxLength(256);
            });

            modelBuilder.Entity<EducationLevel>(entity =>
            {
                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<FiscalSchedule>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.FiscalCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Institute>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.Property(e => e.AltAttribute).HasMaxLength(50);

                entity.Property(e => e.MimeType)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.SeoFilename).HasMaxLength(300);

                entity.Property(e => e.TitleAttribute).HasMaxLength(20);
            });

            modelBuilder.Entity<PictureBinary>(entity =>
            {
                entity.HasOne(d => d.Picture)
                    .WithMany(p => p.PictureBinary)
                    .HasForeignKey(d => d.PictureId);
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.PhoneNumber).HasMaxLength(10);

                entity.Property(e => e.ProfessorCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ProfessorType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProfessorAcademicPosition>(entity =>
            {
                entity.HasKey(e => new { e.ProfessorId, e.AcademicPostionId });

                entity.HasOne(d => d.AcademicPostion)
                    .WithMany(p => p.ProfessorAcademicPosition)
                    .HasForeignKey(d => d.AcademicPostionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProfessorAcademicPosition_AcademicPosition");

                entity.HasOne(d => d.Professor)
                    .WithMany(p => p.ProfessorAcademicPosition)
                    .HasForeignKey(d => d.ProfessorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProfessorAcademicPosition_Professor");
            });

            modelBuilder.Entity<ProfessorAddress>(entity =>
            {
                entity.HasKey(e => new { e.ProfessorId, e.AddressId });

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.ProfessorAddress)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProfessorAddress_Address");

                entity.HasOne(d => d.Professor)
                    .WithMany(p => p.ProfessorAddress)
                    .HasForeignKey(d => d.ProfessorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProfessorAddress_Professor");
            });

            modelBuilder.Entity<Program>(entity =>
            {
                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProgramCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProgramName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProgramUrl)
                    .HasColumnName("ProgramURL")
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.AbstractTitle)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndContractDate).HasColumnType("date");

                entity.Property(e => e.FundAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectNameEn)
                    .HasColumnName("ProjectNameEN")
                    .HasMaxLength(500);

                entity.Property(e => e.ProjectNameTh)
                    .IsRequired()
                    .HasColumnName("ProjectNameTH")
                    .HasMaxLength(500);

                entity.Property(e => e.Published)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.StartContractDate).HasColumnType("date");

                entity.Property(e => e.Utilization)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompletedUpload)
                    .WithMany(p => p.ProjectCompletedUpload)
                    .HasForeignKey(d => d.CompletedUploadId)
                    .HasConstraintName("FK_Project_Download2");

                entity.HasOne(d => d.FiscalSchedule)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.FiscalScheduleId)
                    .HasConstraintName("FK_Project_FiscalSchedule");


                entity.HasOne(d => d.ProposalUpload)
                    .WithMany(p => p.ProjectProposalUpload)
                    .HasForeignKey(d => d.ProposalUploadId)
                    .HasConstraintName("FK_Project_Download1");

                entity.HasOne(d => d.ResearchIssue)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ResearchIssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_ResearchIssue");
            });

            modelBuilder.Entity<ProjectHistory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectNameTh)
                    .IsRequired()
                    .HasColumnName("ProjectNameTH")
                    .HasMaxLength(500);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectHistory)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectHistory_Project");
            });

            modelBuilder.Entity<ProjectKeyword>(entity =>
            {
                entity.HasKey(e => new { e.KeywordId, e.ProjectId });

                entity.Property(e => e.KeywordId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Keyword)
                    .WithMany(p => p.ProjectKeyword)
                    .HasForeignKey(d => d.KeywordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectKeyword_Keyword");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectKeyword)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectKeyword_Project");
            });

            modelBuilder.Entity<ProjectProgress>(entity =>
            {
                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.ProgressEndDate).HasColumnType("date");

                entity.Property(e => e.ProgressStartDate).HasColumnType("date");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectProgress)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectProgress_Project");

            });

            modelBuilder.Entity<ProjectResearcher>(entity =>
            {
                entity.HasKey(e => new { e.ProjectId, e.ResearcherId });

                entity.Property(e => e.ProjectRole)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectResearcher)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectResearcher_Project");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.ProjectResearcher)
                    .HasForeignKey(d => d.ResearcherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectResearcher_Researcher");
            });


            modelBuilder.Entity<Province>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ResearchIssue>(entity =>
            {
                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IssueCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });


            modelBuilder.Entity<Researcher>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(1000);

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.IDCard)
                    .HasColumnName("IDCard")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber).HasMaxLength(10);

                entity.Property(e => e.ResearcherCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.Researcher)
                    .HasForeignKey(d => d.FacultyId)
                    .HasConstraintName("FK_Researcher_Faculty");

                entity.HasOne(d => d.Picture)
                    .WithMany(p => p.Researcher)
                    .HasForeignKey(d => d.PictureId)
                    .HasConstraintName("FK_Researcher_PictureBinary");

                entity.HasOne(d => d.Title)
                    .WithMany(p => p.Researcher)
                    .HasForeignKey(d => d.TitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Researcher_Title");
            });

            modelBuilder.Entity<ResearcherAcademicPosition>(entity =>
            {
                entity.HasKey(e => new { e.ResearcherId, e.AcademicPostionId })
                    .HasName("PK_PersonAcademicPosition");

                entity.HasOne(d => d.AcademicPostion)
                    .WithMany(p => p.ResearcherAcademicPosition)
                    .HasForeignKey(d => d.AcademicPostionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PersonAcademicPosition_AcademicPosition");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.ResearcherAcademicPosition)
                    .HasForeignKey(d => d.ResearcherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResearcherAcademicPosition_Researcher");
            });

            modelBuilder.Entity<ResearcherAddresses>(entity =>
            {
                entity.HasKey(e => new { e.ResearcherId, e.AddressId })
                    .HasName("PK_CustomerAddresses");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.ResearcherAddresses)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResearcherAddresses_Address");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.ResearcherAddresses)
                    .HasForeignKey(d => d.ResearcherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResearcherAddresses_Researcher");
            });

            modelBuilder.Entity<ResearcherEducation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.ResearcherEducation)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResearcherEducation_Country");

                entity.HasOne(d => d.EducationLevel)
                    .WithMany(p => p.ResearcherEducation)
                    .HasForeignKey(d => d.EducationLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResearcherEducation_EducationLevel");

                entity.HasOne(d => d.Institute)
                    .WithMany(p => p.ResearcherEducation)
                    .HasForeignKey(d => d.InstituteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResearcherEducation_Institute");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.ResearcherEducation)
                    .HasForeignKey(d => d.ResearcherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResearcherEducation_Researcher");
            });

            modelBuilder.Entity<ResearcherHistory>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.ResearcherHistory)
                    .HasForeignKey(d => d.ResearcherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResearcherHistory_Researcher");

                entity.HasOne(d => d.Title)
                    .WithMany(p => p.ResearcherHistory)
                    .HasForeignKey(d => d.TitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResearcherHistory_Title");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<RoleProgram>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.ProgramId });

                entity.Property(e => e.CanAdd)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CanDelete)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CanEdit)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CanPrint)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CanView)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Modified).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.RoleProgram)
                    .HasForeignKey(d => d.ProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleProgram_Program");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleProgram)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleProgram_Role");
            });

            modelBuilder.Entity<Subdistrict>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ZipPostalCode).HasMaxLength(5);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Subdistrict)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subdistrict_District");
            });

            modelBuilder.Entity<Title>(entity =>
            {
                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TitleName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Modified).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MobileNumber).HasMaxLength(10);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.SessionId).HasMaxLength(500);

                entity.Property(e => e.UserType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.FacultyId)
                    .HasConstraintName("FK_User_Faculty");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_User_Researcher");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}