using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BarKick.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
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
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Cocktail> Cocktails { get; set; }
        public DbSet<Bartender> Bartenders { get; set; }
        public DbSet<VenueBartender> VenueBartenders { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define the composite primary key for VenueBartender
            modelBuilder.Entity<VenueBartender>()
                .HasKey(vb => new { vb.BartenderId, vb.VenueID });

            // Define the relationships
            modelBuilder.Entity<VenueBartender>()
                .HasRequired(vb => vb.Bartender)
                .WithMany(b => b.Venues)
                .HasForeignKey(vb => vb.BartenderId);

            modelBuilder.Entity<VenueBartender>()
                .HasRequired(vb => vb.Venue)
                .WithMany(v => v.VenueBartenders)
                .HasForeignKey(vb => vb.VenueID);

        }
    }
}
