using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuizManager.Models;

namespace QuizManager.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DbConnectionString"));
        }

       
        public DbSet<Student> Students { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Professor> Professors { get; set; }
         
        public DbSet<CompanyJob> CompanyJobs { get; set; }
        public DbSet<CompanyJobApplied> CompanyJobsApplied { get; set; }
        public DbSet<CompanyJobApplied_StudentDetails> CompanyJobApplied_StudentDetails { get; set; }
        public DbSet<CompanyJobApplied_CompanyDetails> CompanyJobApplied_CompanyDetails { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Area> Areas { get; set; }

        // Company Thesis
        public DbSet<ThesisApplication> ThesisApplications { get; set; }
        public DbSet<CompanyThesis> CompanyTheses { get; set; }
        public DbSet<CompanyThesisApplied> CompanyThesesApplied { get; set; }
        public DbSet<CompanyThesisApplied_CompanyDetails> CompanyThesisApplied_CompanyDetails { get; set; }
        public DbSet<CompanyThesisApplied_StudentDetails> CompanyThesisApplied_StudentDetails { get; set; }

        // Proferssor Thesis
        public DbSet<ProfessorThesis> ProfessorTheses { get; set; }
        public DbSet<ProfessorThesisApplied> ProfessorThesesApplied { get; set; }
        public DbSet<ProfessorThesisApplied_StudentDetails> ProfessorThesisApplied_StudentDetails { get; set; }
        public DbSet<ProfessorThesisApplied_ProfessorDetails> ProfessorThesisApplied_ProfessorDetails { get; set; }

        // Company Internships
        public DbSet<CompanyInternship> CompanyInternships { get; set; }
        public DbSet<InternshipApplied> InternshipsApplied { get; set; }
        public DbSet<InternshipApplied_CompanyDetails> InternshipApplied_CompanyDetails { get; set; }
        public DbSet<InternshipApplied_StudentDetails> InternshipApplied_StudentDetails { get; set; }


        // Professor Internships
        public DbSet<ProfessorInternship> ProfessorInternships { get; set; }
        public DbSet<ProfessorInternshipApplied> ProfessorInternshipsApplied { get; set; }
        public DbSet<ProfessorInternshipsApplied_StudentDetails> ProfessorInternshipsApplied_StudentDetails { get; set; }
        public DbSet<ProfessorInternshipsApplied_ProfessorDetails> ProfessorInternshipsApplied_ProfessorDetails { get; set; }

        // ResearchGroup Profile
        public DbSet<ResearchGroup> ResearchGroups { get; set; }
        public DbSet<ResearchGroup_Professors> ResearchGroup_Professors { get; set; }
        public DbSet<ResearchGroup_NonFacultyMembers> ResearchGroup_NonFacultyMembers { get; set; }
        public DbSet<ResearchGroup_Publications> ResearchGroup_Publications { get; set; }
        public DbSet<ResearchGroup_Patents> ResearchGroup_Patents { get; set; }
        public DbSet<ResearchGroup_SpinOffCompany> ResearchGroup_SpinOffCompany { get; set; }
        public DbSet<ResearchGroup_ResearchActions> ResearchGroup_ResearchActions { get; set; }


        // Announcements
        public DbSet<AnnouncementAsCompany> AnnouncementsAsCompany { get; set; }
        public DbSet<AnnouncementAsProfessor> AnnouncementsAsProfessor { get; set; }
        
        // Events
        public DbSet<CompanyEvent> CompanyEvents { get; set; }
        public DbSet<InterestInCompanyEvent> InterestInCompanyEvents { get; set; }

        public DbSet<ProfessorEvent> ProfessorEvents { get; set; }
        public DbSet<InterestInProfessorEvent> InterestInProfessorEvents { get; set; }
        public DbSet<InterestInProfessorEvent_ProfessorDetails> InterestInProfessorEvent_ProfessorDetails { get; set; }
        public DbSet<InterestInProfessorEvent_StudentDetails> InterestInProfessorEvent_StudentDetails { get; set; }


        public DbSet<InterestInCompanyEventAsProfessor> InterestInCompanyEventsAsProfessor { get; set; }

		public DbSet<InterestInProfessorEventAsCompany> InterestInProfessorEventsAsCompany { get; set; }
		public DbSet<InterestInProfessorEventAsCompany_CompanyDetails> InterestInProfessorEventAsCompany_CompanyDetails { get; set; }
		public DbSet<InterestInProfessorEventAsCompany_ProfessorDetails> InterestInProfessorEventAsCompany_ProfessorDetails { get; set; }

		public DbSet<InterestInCompanyEventAsProfessor> InterestInCompanyEventAsProfessor { get; set; }
		public DbSet<InterestInCompanyEventAsProfessor_CompanyDetails> InterestInCompanyEventAsProfessor_CompanyDetails { get; set; }
		public DbSet<InterestInCompanyEventAsProfessor_ProfessorDetails> InterestInCompanyEventAsProfessor_ProfessorDetails { get; set; }



		public DbSet<PlatformActions> PlatformActions { get; set; }

        // Helper methods
        public async Task<List<CompanyJobApplied>> GetJobApplicationsAsync()
        {
            return await CompanyJobsApplied.ToListAsync();
        }

        public async Task<List<ThesisApplication>> GetThesisApplicationsAsync()
        {
            return await ThesisApplications.ToListAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			//COMPANY JOBS APPLIED AS STUDENT
			modelBuilder.Entity<CompanyJobApplied>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationships
                entity.HasOne(e => e.StudentDetails)
                      .WithOne(s => s.Application)
                      .HasForeignKey<CompanyJobApplied_StudentDetails>(s => s.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.CompanyDetails)
                      .WithOne(c => c.Application)
                      .HasForeignKey<CompanyJobApplied_CompanyDetails>(c => c.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique constraint to prevent duplicate applications
                entity.HasIndex(e => new { e.StudentEmailAppliedForCompanyJob, e.RNGForCompanyJobApplied })
                      .IsUnique();
            });

            modelBuilder.Entity<CompanyJobApplied_CompanyDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Non-unique index for company email
                entity.HasIndex(e => e.CompanysEmailWhereStudentAppliedForCompanyJob)
                      .IsUnique(false);
            });

            modelBuilder.Entity<CompanyJobApplied_StudentDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Add non-unique index for student email to match InternshipApplied pattern
                entity.HasIndex(e => e.StudentEmailAppliedForCompanyJob)
                      .IsUnique(false);
            });

			//COMPANY INTERNSHIPS APPLIED AS STUDENT
			modelBuilder.Entity<InternshipApplied>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationships
                entity.HasOne(e => e.StudentDetails)
                      .WithOne(s => s.Application)
                      .HasForeignKey<InternshipApplied_StudentDetails>(s => s.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.CompanyDetails)
                      .WithOne(c => c.Application)
                      .HasForeignKey<InternshipApplied_CompanyDetails>(c => c.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique constraint to prevent duplicate applications
                entity.HasIndex(e => new { e.StudentEmailAppliedForInternship, e.RNGForInternshipApplied })
                      .IsUnique();
            });

            modelBuilder.Entity<InternshipApplied_CompanyDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Non-unique index for company email
                entity.HasIndex(e => e.CompanyEmailWhereStudentAppliedForInternship)
                      .IsUnique(false);
            });

            modelBuilder.Entity<InternshipApplied_StudentDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Add any additional indexes for student details if needed
                entity.HasIndex(e => e.StudentEmailAppliedForInternship)
                      .IsUnique(false);
            });

			// COMPANY THESIS APPLIED AS STUDENT
			modelBuilder.Entity<CompanyThesisApplied>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationships
                entity.HasOne(e => e.StudentDetails)
                      .WithOne(s => s.Application)
                      .HasForeignKey<CompanyThesisApplied_StudentDetails>(s => s.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.CompanyDetails)
                      .WithOne(c => c.Application)
                      .HasForeignKey<CompanyThesisApplied_CompanyDetails>(c => c.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique constraint to prevent duplicate applications
                entity.HasIndex(e => new { e.StudentEmailAppliedForThesis, e.RNGForCompanyThesisApplied })
                      .IsUnique();
            });

            modelBuilder.Entity<CompanyThesisApplied_CompanyDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Non-unique index for company email
                entity.HasIndex(e => e.CompanyEmailWhereStudentAppliedForThesis)
                      .IsUnique(false);
            });

            modelBuilder.Entity<CompanyThesisApplied_StudentDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Add any additional indexes for student details if needed
                entity.HasIndex(e => e.StudentEmailAppliedForThesis)
                      .IsUnique(false);
            });

            // INTEREST IN COMPANY EVENT AS STUDENT
            modelBuilder.Entity<InterestInCompanyEvent>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationships
                entity.HasOne(e => e.StudentDetails)
                      .WithOne(s => s.Application)
                      .HasForeignKey<InterestInCompanyEvent_StudentDetails>(s => s.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.CompanyDetails)
                      .WithOne(c => c.Application)
                      .HasForeignKey<InterestInCompanyEvent_CompanyDetails>(c => c.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique constraint to prevent duplicate interests
                entity.HasIndex(e => new { e.StudentEmailShowInterestForEvent, e.RNGForCompanyEventInterest })
                      .IsUnique();
            });

            modelBuilder.Entity<InterestInCompanyEvent_CompanyDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CompanyEmailWhereStudentShowInterestForCompanyEvent)
                      .IsUnique(false);
            });

            modelBuilder.Entity<InterestInCompanyEvent_StudentDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.StudentEmailShowInterestForCompanyEvent)
                      .IsUnique(false);
            });

			// INTEREST IN PROFESSOR EVENT AS COMPANY
			modelBuilder.Entity<InterestInProfessorEventAsCompany>(entity =>
			{
				entity.HasKey(e => e.Id);

				// Configure relationships
				entity.HasOne(e => e.CompanyDetails)
					  .WithOne(c => c.EventInterest)
					  .HasForeignKey<InterestInProfessorEventAsCompany_CompanyDetails>(c => c.Id)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(e => e.ProfessorDetails)
					  .WithOne(p => p.EventInterest)
					  .HasForeignKey<InterestInProfessorEventAsCompany_ProfessorDetails>(p => p.Id)
					  .OnDelete(DeleteBehavior.Cascade);

				// Composite unique constraint to prevent duplicate interests
				entity.HasIndex(e => new { e.CompanyEmailShowInterestForProfessorEvent, e.RNGForProfessorEventInterestAsCompany })
					  .IsUnique();
			});

			modelBuilder.Entity<InterestInProfessorEventAsCompany_CompanyDetails>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.HasIndex(e => e.CompanyEmailShowInterestForProfessorEvent)
					  .IsUnique(false);
			});

			modelBuilder.Entity<InterestInProfessorEventAsCompany_ProfessorDetails>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.HasIndex(e => e.ProfessorEmailWhereCompanyShowInterestForProfessorEvent)
					  .IsUnique(false);
			});

			// INTEREST IN COMPANY EVENT AS PROFESSOR
			modelBuilder.Entity<InterestInCompanyEventAsProfessor>(entity =>
			{
				entity.HasKey(e => e.Id);

				// Configure relationships
				entity.HasOne(e => e.CompanyDetails)
					  .WithOne(c => c.EventInterest)
					  .HasForeignKey<InterestInCompanyEventAsProfessor_CompanyDetails>(c => c.Id)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(e => e.ProfessorDetails)
					  .WithOne(p => p.EventInterest)
					  .HasForeignKey<InterestInCompanyEventAsProfessor_ProfessorDetails>(p => p.Id)
					  .OnDelete(DeleteBehavior.Cascade);

				// Composite unique constraint to prevent duplicate interests
				entity.HasIndex(e => new { e.ProfessorEmailShowInterestForCompanyEvent, e.RNGForCompanyEventInterestAsProfessor })
					  .IsUnique();
			});

			modelBuilder.Entity<InterestInCompanyEventAsProfessor_CompanyDetails>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.HasIndex(e => e.CompanyEmailWhereProfessorShowInterestForCompanyEvent)
					  .IsUnique(false);
			});

			modelBuilder.Entity<InterestInCompanyEventAsProfessor_ProfessorDetails>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.HasIndex(e => e.ProfessorEmailShowInterestForCompanyEvent)
					  .IsUnique(false);
			});

            // PROFESSOR INTERNSHIPS APPLIED AS STUDENT
            modelBuilder.Entity<ProfessorInternshipApplied>(entity =>
            {
                entity.HasKey(e => e.Id);
                 
                // Configure relationships
                entity.HasOne(e => e.StudentDetails)
                      .WithOne(s => s.Application)
                      .HasForeignKey<ProfessorInternshipsApplied_StudentDetails>(s => s.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ProfessorDetails)
                      .WithOne(p => p.Application)
                      .HasForeignKey<ProfessorInternshipsApplied_ProfessorDetails>(p => p.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique constraint to prevent duplicate applications
                entity.HasIndex(e => new { e.StudentEmailAppliedForProfessorInternship, e.RNGForProfessorInternshipApplied })
                      .IsUnique();
            });

            modelBuilder.Entity<ProfessorInternshipsApplied_ProfessorDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Non-unique index for professor email
                entity.HasIndex(e => e.ProfessorEmailWhereStudentAppliedForProfessorInternship)
                      .IsUnique(false);
            });

            modelBuilder.Entity<ProfessorInternshipsApplied_StudentDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Non-unique index for student email
                entity.HasIndex(e => e.StudentEmailAppliedForProfessorInternship)
                      .IsUnique(false);
            });

            // PROFESSOR THESIS APPLIED AS STUDENT
            modelBuilder.Entity<ProfessorThesisApplied>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationships
                entity.HasOne(e => e.StudentDetails)
                      .WithOne(s => s.Application)
                      .HasForeignKey<ProfessorThesisApplied_StudentDetails>(s => s.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ProfessorDetails)
                      .WithOne(p => p.Application)
                      .HasForeignKey<ProfessorThesisApplied_ProfessorDetails>(p => p.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique constraint to prevent duplicate applications
                entity.HasIndex(e => new { e.StudentEmailAppliedForProfessorThesis, e.RNGForProfessorThesisApplied })
                      .IsUnique();
            });

            modelBuilder.Entity<ProfessorThesisApplied_ProfessorDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Non-unique index for professor email
                entity.HasIndex(e => e.ProfessorEmailWhereStudentAppliedForProfessorThesis)
                      .IsUnique(false);
            });

            modelBuilder.Entity<ProfessorThesisApplied_StudentDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Non-unique index for student email
                entity.HasIndex(e => e.StudentEmailAppliedForProfessorThesis)
                      .IsUnique(false);
            });

            // INTEREST IN PROFESSOR EVENT AS STUDENT
            modelBuilder.Entity<InterestInProfessorEvent>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationships
                entity.HasOne(e => e.StudentDetails)
                      .WithOne(s => s.Application)
                      .HasForeignKey<InterestInProfessorEvent_StudentDetails>(s => s.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ProfessorDetails)
                      .WithOne(p => p.Application)
                      .HasForeignKey<InterestInProfessorEvent_ProfessorDetails>(p => p.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique constraint to prevent duplicate interests
                entity.HasIndex(e => new { e.StudentEmailShowInterestForEvent, e.RNGForProfessorEventInterest })
                      .IsUnique();
            });

            modelBuilder.Entity<InterestInProfessorEvent_ProfessorDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ProfessorEmailWhereStudentShowInterestForProfessorEvent)
                      .IsUnique(false);
            });

            modelBuilder.Entity<InterestInProfessorEvent_StudentDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.StudentEmailShowInterestForProfessorEvent)
                      .IsUnique(false);
            });

            // UPLOAD COMPANY JOBS
            modelBuilder.Entity<CompanyJob>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationship with Company
                entity.HasOne(e => e.Company)
                      .WithMany() // A company can have many jobs
                      .HasForeignKey(e => e.EmailUsedToUploadJobs)
                      .HasPrincipalKey(c => c.CompanyEmail)
                      .OnDelete(DeleteBehavior.Restrict); // Or Cascade if you want to delete jobs when company is deleted

                // Add any indexes you need for CompanyJob
                entity.HasIndex(e => e.EmailUsedToUploadJobs)
                      .IsUnique(false);

                entity.HasIndex(e => e.RNGForPositionUploaded_HashedAsUniqueID)
                      .IsUnique();
            });

            // UPLOAD COMPANY ANNOUNCEMENTS
            modelBuilder.Entity<AnnouncementAsCompany>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationship with Company
                entity.HasOne(e => e.Company)
                      .WithMany() // A company can have many announcements
                      .HasForeignKey(e => e.CompanyAnnouncementCompanyEmail)
                      .HasPrincipalKey(c => c.CompanyEmail)
                      .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.CompanyAnnouncementCompanyEmail)
                      .IsUnique(false);

                entity.HasIndex(e => e.CompanyAnnouncementRNG_HashedAsUniqueID)
                      .IsUnique();
            });

            // UPLOAD COMPANY THESIS
            modelBuilder.Entity<CompanyThesis>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationship with Company
                entity.HasOne(e => e.Company)
                      .WithMany()
                      .HasForeignKey(e => e.CompanyEmailUsedToUploadThesis)
                      .HasPrincipalKey(c => c.CompanyEmail)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with Professor (if interested)
                entity.HasOne(e => e.ProfessorInterested)
                      .WithMany()
                      .HasForeignKey(e => e.ProfessorEmailInterestedInCompanyThesis)
                      .HasPrincipalKey(p => p.ProfEmail)
                      .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.CompanyEmailUsedToUploadThesis)
                      .IsUnique(false);

                entity.HasIndex(e => e.ProfessorEmailInterestedInCompanyThesis)
                      .IsUnique(false);

                entity.HasIndex(e => e.RNGForThesisUploadedAsCompany_HashedAsUniqueID)
                      .IsUnique();
            });

			// UPLOAD COMPANY EVENTS
			modelBuilder.Entity<CompanyEvent>(entity =>
			{
				entity.HasKey(e => e.Id);

				// Configure relationship with Company
				entity.HasOne(e => e.Company)
					  .WithMany() // A company can have many events
					  .HasForeignKey(e => e.CompanyEmailUsedToUploadEvent)
					  .HasPrincipalKey(c => c.CompanyEmail)
					  .OnDelete(DeleteBehavior.Restrict); // Or Cascade if appropriate

				// Add any indexes you need for CompanyEvent
				entity.HasIndex(e => e.CompanyEmailUsedToUploadEvent)
					  .IsUnique(false);

				entity.HasIndex(e => e.RNGForEventUploadedAsCompany_HashedAsUniqueID)
					  .IsUnique();
			});

            // UPLOAD PROFESSOR ANNOUNCEMENTS
            modelBuilder.Entity<AnnouncementAsProfessor>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationship with Professor
                entity.HasOne(e => e.Professor)
                      .WithMany() // A professor can have many announcements
                      .HasForeignKey(e => e.ProfessorAnnouncementProfessorEmail)
                      .HasPrincipalKey(p => p.ProfEmail)
                      .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.ProfessorAnnouncementProfessorEmail)
                      .IsUnique(false);

                entity.HasIndex(e => e.ProfessorAnnouncementRNG_HashedAsUniqueID)
                      .IsUnique();
            });

            // UPLOAD PROFESSOR EVENTS
            modelBuilder.Entity<ProfessorEvent>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationship with Professor
                entity.HasOne(e => e.Professor)
                      .WithMany() // A professor can have many events
                      .HasForeignKey(e => e.ProfessorEmailUsedToUploadEvent)
                      .HasPrincipalKey(p => p.ProfEmail)
                      .OnDelete(DeleteBehavior.Restrict); // Or Cascade if appropriate

                // Add any indexes you need for ProfessorEvent
                entity.HasIndex(e => e.ProfessorEmailUsedToUploadEvent)
                      .IsUnique(false);

                entity.HasIndex(e => e.RNGForEventUploadedAsProfessor_HashedAsUniqueID)
                      .IsUnique();
            });

            // UPLOAD COMPANY INTERNSHIP
            modelBuilder.Entity<CompanyInternship>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationship with Company
                entity.HasOne(e => e.Company)
                      .WithMany() // A company can have many internships
                      .HasForeignKey(e => e.CompanyEmailUsedToUploadInternship)
                      .HasPrincipalKey(c => c.CompanyEmail)
                      .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.CompanyEmailUsedToUploadInternship)
                      .IsUnique(false);

                entity.HasIndex(e => e.RNGForInternshipUploadedAsCompany_HashedAsUniqueID)
                      .IsUnique();
            });

			// UPLOAD PROFESSOR INTERNSHIP
			modelBuilder.Entity<ProfessorInternship>(entity =>
			{
				entity.HasKey(e => e.Id);

				// Configure relationship with Professor
				entity.HasOne(e => e.Professor)
					  .WithMany() // A professor can have many internships
					  .HasForeignKey(e => e.ProfessorEmailUsedToUploadInternship)
					  .HasPrincipalKey(p => p.ProfEmail)
					  .OnDelete(DeleteBehavior.Restrict);

				// Indexes
				entity.HasIndex(e => e.ProfessorEmailUsedToUploadInternship)
					  .IsUnique(false);

				entity.HasIndex(e => e.RNGForInternshipUploadedAsProfessor_HashedAsUniqueID)
					  .IsUnique();
			});

            // UPLOAD PROFESSOR THESIS
            modelBuilder.Entity<ProfessorThesis>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure relationship with Professor
                entity.HasOne(e => e.Professor)
                      .WithMany()
                      .HasForeignKey(e => e.ProfessorEmailUsedToUploadThesis)
                      .HasPrincipalKey(p => p.ProfEmail)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with Company (if interested)
                entity.HasOne(e => e.CompanyInterested)
                      .WithMany()
                      .HasForeignKey(e => e.CompanyEmailInterestedInProfessorThesis)
                      .HasPrincipalKey(c => c.CompanyEmail)
                      .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.ProfessorEmailUsedToUploadThesis)
                      .IsUnique(false);

                entity.HasIndex(e => e.CompanyEmailInterestedInProfessorThesis)
                      .IsUnique(false);

                entity.HasIndex(e => e.RNGForThesisUploaded_HashedAsUniqueID)
                      .IsUnique();
            });


        }


    }
}
