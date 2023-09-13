using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SADPMRCarAPI.Model;
using System.Reflection.Emit;

namespace SADPMRCarAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUserIdentity>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedCars(builder);
            SeedDepartment(builder);
            JobtitleSeededData(builder);
            Status(builder);

        }

        public DbSet<Status> statuses { get; set; }
        public DbSet<JobTitle> jobTitles { get; set; }  
        public DbSet<CarModel> Cars { get; set; }
        public DbSet<CarServiceModel> CarServices { get; set; }
        public DbSet<TripModel> Trips { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Uploadfile> Uploadfiles { get; set; }
        public DbSet<CarAccessories> CarAccessories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                
                optionsBuilder.UseSqlServer("Server = JHBICTD001\\SINETHEMBASQL; Database = mySAPDMR_db; Trusted_Connection = true;");
            }
        }



        private void SeedCars(ModelBuilder builder)
        {
            builder.Entity<CarModel>().HasData(
                new CarModel
                {
                    CarId = 1,
                    MakeOfTheCar = "Toyota Hilux",
                    RegistrationOfTheCar = "SAD 756 GP",
                    DepartmentId = 2,   
                   
                },
                new CarModel
                {
                    CarId = 2,
                    MakeOfTheCar = "Ford Ranger",
                    RegistrationOfTheCar = "PMR 005 GP",
                    DepartmentId = 1
                  
                },
                 new CarModel
                 {
                     CarId = 3,
                     MakeOfTheCar = "VW Amarok",
                     RegistrationOfTheCar = "SDT 586 GP",
                     DepartmentId = 3   

                 }

            );
        }

        private void SeedDepartment(ModelBuilder builder)
        {
            builder.Entity<Department>().HasData(
                new Department 
                {
                    DepartmentId = 1,
                    DepartmentName = "Administration"
                },
                new Department
                {
                    DepartmentId = 2,
                    DepartmentName = "Diamond Trade"
                },
                 new Department
                 {
                     DepartmentId = 3,
                     DepartmentName = "Regulatory Compliance"
                 },
                new Department
                {
                    DepartmentId = 4,
                    DepartmentName = "Finance"
                },
                 new Department
                 {
                     DepartmentId = 5,
                     DepartmentName = "Licensing"
                 }
                );
        }

        private static void JobtitleSeededData(ModelBuilder builder) 
        {
            builder.Entity<JobTitle>().HasData(
                
                new JobTitle
                {
                    JobTitleId = 1, 
                    Title = "Staff",
                },
                new JobTitle
                {
                    JobTitleId = 2, 
                    Title = "Manager",

                }
                ,
                new JobTitle    
                {
                    JobTitleId = 3,
                    Title = "General Manager Administration",  
                },
                new JobTitle 
                {
                    JobTitleId = 4, 
                    Title = "General Manager Diamond Trade"


                },
                new JobTitle 
                { 
                    JobTitleId= 5,
                    Title = "General Manager Licensing"
                },
                new JobTitle
                { 
                    JobTitleId = 6,
                    Title = "General Manager Regulatory Compliance"
                },   
                new JobTitle
                { 
                    JobTitleId = 7,
                    Title = "General Manager Finance"
                },
                new JobTitle
                {
                    JobTitleId = 8, 
                    Title = "CEO",    
                },
                new JobTitle 
                { 
                   JobTitleId = 9,
                   Title = "Admin For Administration"
                },
                new JobTitle 
                {
                   JobTitleId = 10,
                   Title = "Admin For Diamond Trade"
                },
                new JobTitle
                {
                    JobTitleId = 11,
                    Title = "Admin For Licensing"
                },
                new JobTitle 
                {
                    JobTitleId = 12,
                    Title = "Admin For Regulatory Compliance "
                },
                new JobTitle 
                {
                    JobTitleId = 13,
                    Title = "Admin For  Finance"
                }
                );

        }
        private void Status(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Status>().HasData(
                new Status
                { StatusId = 1,
                  StatusName = "Reject" 
                },
                new Status
                { StatusId = 2 ,
                  StatusName = "Accepted"
                },
                new Status
                {
                    StatusId = 3 ,  
                    StatusName = "Submitted"
                },
                new Status
                {
                    StatusId= 4 ,   
                    StatusName = "Pending"
                }
                );
        } 
    }
}

