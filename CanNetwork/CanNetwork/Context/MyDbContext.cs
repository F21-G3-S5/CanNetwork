using CanNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CanNetwork.Context
{
    public class MyDbContext: DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ApplyedJob> ApplyedJobs { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LikedCompany> LikedCompanies { get; set; }
        public DbSet<LikedJobs> LikedJobs { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<RegisterUser> RegisterUsers { get; set; }
        public DbSet<ReportedJob> ReportedJobs { get; set; }
        public DbSet<SeekerRegistration> SeekerRegistrations { get; set; }
        public DbSet<University> Universities { get; set; }
        public MyDbContext()
        {
        }
    }
}