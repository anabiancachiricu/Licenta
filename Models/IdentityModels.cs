using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MedOffice.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public IEnumerable<SelectListItem> AllRoles { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] UserPhoto { get; set; }


        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DBConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext,MedOffice.Migrations.Configuration>("DBConnectionString"));
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        public DbSet<Location> Locations { get; set;}
        public DbSet<Investigation> Investigations { get; set; }
        public DbSet<Department> Departments { get; set; }
        
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<LocationDepartment> LocationDepartments { get;  set; }
    }
}