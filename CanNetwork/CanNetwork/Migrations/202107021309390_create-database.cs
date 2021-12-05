namespace CanNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplyedJobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplyDate = c.DateTime(nullable: false),
                        JobId = c.Int(nullable: false),
                        SeekerRegistrationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .ForeignKey("dbo.SeekerRegistrations", t => t.SeekerRegistrationId, cascadeDelete: true)
                .Index(t => t.JobId)
                .Index(t => t.SeekerRegistrationId);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobTitle = c.String(),
                        JobDescription = c.String(),
                        CareerLevel = c.String(),
                        Vacancy = c.Int(nullable: false),
                        NeededExperience = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        Salary = c.Int(nullable: false),
                        PublishedDate = c.DateTime(nullable: false),
                        JobState = c.Boolean(nullable: false),
                        CityId = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        JobTypeId = c.Int(nullable: false),
                        JobCategoryId = c.Int(nullable: false),
                        RegisterUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.JobCategories", t => t.JobCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.JobTypes", t => t.JobTypeId, cascadeDelete: true)
                .ForeignKey("dbo.RegisterUsers", t => t.RegisterUserId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.CountryId)
                .Index(t => t.JobTypeId)
                .Index(t => t.JobCategoryId)
                .Index(t => t.RegisterUserId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobCategoryName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobTypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RegisterUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProviderImage = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Password = c.String(),
                        CompanyName = c.String(),
                        CompanyField = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        JobTitleId = c.Int(nullable: false),
                        SecurityQuestion = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobTitles", t => t.JobTitleId, cascadeDelete: true)
                .Index(t => t.JobTitleId);
            
            CreateTable(
                "dbo.JobTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobTitleName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SeekerRegistrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeekerImage = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Gender = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        SearchOnJobTitle = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CityId = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        NationalityId = c.Int(nullable: false),
                        GraduationDate = c.DateTime(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        UniversityId = c.Int(nullable: false),
                        EducationLevelId = c.Int(nullable: false),
                        SecurityQuestion = c.String(),
                        SeekerCV = c.String(),
                        CompanyName = c.String(),
                        Salary = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        JobTitleId = c.Int(nullable: false),
                        JobTypeId = c.Int(nullable: false),
                        JobCategoryId = c.Int(nullable: false),
                        CurrentCompanyName = c.String(),
                        CurrentSalary = c.Int(nullable: false),
                        CurrentDuration = c.Int(nullable: false),
                        CurrentJobTitle = c.String(),
                        CurrentJobType = c.String(),
                        CurrentJobCategory = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.EducationLevels", t => t.EducationLevelId, cascadeDelete: true)
                .ForeignKey("dbo.JobCategories", t => t.JobCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.JobTitles", t => t.JobTitleId, cascadeDelete: true)
                .ForeignKey("dbo.JobTypes", t => t.JobTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.Nationalities", t => t.NationalityId, cascadeDelete: true)
                .ForeignKey("dbo.Universities", t => t.UniversityId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.CountryId)
                .Index(t => t.NationalityId)
                .Index(t => t.LanguageId)
                .Index(t => t.UniversityId)
                .Index(t => t.EducationLevelId)
                .Index(t => t.JobTitleId)
                .Index(t => t.JobTypeId)
                .Index(t => t.JobCategoryId);
            
            CreateTable(
                "dbo.EducationLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EducationLevelName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Nationalities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NationalityName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Universities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniversityName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostImage = c.String(),
                        PostTitle = c.String(),
                        PostContent = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Subject = c.String(nullable: false),
                        Message = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LikedCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LikedCompanyDate = c.DateTime(nullable: false),
                        SeekerRegistrationId = c.Int(nullable: false),
                        RegisterUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RegisterUsers", t => t.RegisterUserId, cascadeDelete: true)
                .ForeignKey("dbo.SeekerRegistrations", t => t.SeekerRegistrationId, cascadeDelete: true)
                .Index(t => t.SeekerRegistrationId)
                .Index(t => t.RegisterUserId);
            
            CreateTable(
                "dbo.LikedJobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LikedDate = c.DateTime(nullable: false),
                        JobId = c.Int(nullable: false),
                        SeekerRegistrationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .ForeignKey("dbo.SeekerRegistrations", t => t.SeekerRegistrationId, cascadeDelete: true)
                .Index(t => t.JobId)
                .Index(t => t.SeekerRegistrationId);
            
            CreateTable(
                "dbo.ReportedJobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportedDate = c.DateTime(nullable: false),
                        JobId = c.Int(nullable: false),
                        SeekerRegistrationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .ForeignKey("dbo.SeekerRegistrations", t => t.SeekerRegistrationId, cascadeDelete: true)
                .Index(t => t.JobId)
                .Index(t => t.SeekerRegistrationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportedJobs", "SeekerRegistrationId", "dbo.SeekerRegistrations");
            DropForeignKey("dbo.ReportedJobs", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.LikedJobs", "SeekerRegistrationId", "dbo.SeekerRegistrations");
            DropForeignKey("dbo.LikedJobs", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.LikedCompanies", "SeekerRegistrationId", "dbo.SeekerRegistrations");
            DropForeignKey("dbo.LikedCompanies", "RegisterUserId", "dbo.RegisterUsers");
            DropForeignKey("dbo.ApplyedJobs", "SeekerRegistrationId", "dbo.SeekerRegistrations");
            DropForeignKey("dbo.SeekerRegistrations", "UniversityId", "dbo.Universities");
            DropForeignKey("dbo.SeekerRegistrations", "NationalityId", "dbo.Nationalities");
            DropForeignKey("dbo.SeekerRegistrations", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.SeekerRegistrations", "JobTypeId", "dbo.JobTypes");
            DropForeignKey("dbo.SeekerRegistrations", "JobTitleId", "dbo.JobTitles");
            DropForeignKey("dbo.SeekerRegistrations", "JobCategoryId", "dbo.JobCategories");
            DropForeignKey("dbo.SeekerRegistrations", "EducationLevelId", "dbo.EducationLevels");
            DropForeignKey("dbo.SeekerRegistrations", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.SeekerRegistrations", "CityId", "dbo.Cities");
            DropForeignKey("dbo.ApplyedJobs", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.RegisterUsers", "JobTitleId", "dbo.JobTitles");
            DropForeignKey("dbo.Jobs", "RegisterUserId", "dbo.RegisterUsers");
            DropForeignKey("dbo.Jobs", "JobTypeId", "dbo.JobTypes");
            DropForeignKey("dbo.Jobs", "JobCategoryId", "dbo.JobCategories");
            DropForeignKey("dbo.Jobs", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Jobs", "CityId", "dbo.Cities");
            DropIndex("dbo.ReportedJobs", new[] { "SeekerRegistrationId" });
            DropIndex("dbo.ReportedJobs", new[] { "JobId" });
            DropIndex("dbo.LikedJobs", new[] { "SeekerRegistrationId" });
            DropIndex("dbo.LikedJobs", new[] { "JobId" });
            DropIndex("dbo.LikedCompanies", new[] { "RegisterUserId" });
            DropIndex("dbo.LikedCompanies", new[] { "SeekerRegistrationId" });
            DropIndex("dbo.SeekerRegistrations", new[] { "JobCategoryId" });
            DropIndex("dbo.SeekerRegistrations", new[] { "JobTypeId" });
            DropIndex("dbo.SeekerRegistrations", new[] { "JobTitleId" });
            DropIndex("dbo.SeekerRegistrations", new[] { "EducationLevelId" });
            DropIndex("dbo.SeekerRegistrations", new[] { "UniversityId" });
            DropIndex("dbo.SeekerRegistrations", new[] { "LanguageId" });
            DropIndex("dbo.SeekerRegistrations", new[] { "NationalityId" });
            DropIndex("dbo.SeekerRegistrations", new[] { "CountryId" });
            DropIndex("dbo.SeekerRegistrations", new[] { "CityId" });
            DropIndex("dbo.RegisterUsers", new[] { "JobTitleId" });
            DropIndex("dbo.Jobs", new[] { "RegisterUserId" });
            DropIndex("dbo.Jobs", new[] { "JobCategoryId" });
            DropIndex("dbo.Jobs", new[] { "JobTypeId" });
            DropIndex("dbo.Jobs", new[] { "CountryId" });
            DropIndex("dbo.Jobs", new[] { "CityId" });
            DropIndex("dbo.ApplyedJobs", new[] { "SeekerRegistrationId" });
            DropIndex("dbo.ApplyedJobs", new[] { "JobId" });
            DropTable("dbo.ReportedJobs");
            DropTable("dbo.LikedJobs");
            DropTable("dbo.LikedCompanies");
            DropTable("dbo.Contacts");
            DropTable("dbo.Blogs");
            DropTable("dbo.Universities");
            DropTable("dbo.Nationalities");
            DropTable("dbo.Languages");
            DropTable("dbo.EducationLevels");
            DropTable("dbo.SeekerRegistrations");
            DropTable("dbo.JobTitles");
            DropTable("dbo.RegisterUsers");
            DropTable("dbo.JobTypes");
            DropTable("dbo.JobCategories");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
            DropTable("dbo.Jobs");
            DropTable("dbo.ApplyedJobs");
            DropTable("dbo.Admins");
        }
    }
}
