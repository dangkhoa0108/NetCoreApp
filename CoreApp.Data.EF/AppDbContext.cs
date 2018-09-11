using System;
using System.IO;
using System.Linq;
using CoreApp.Data.EF.Config;
using CoreApp.Data.EF.Extensions;
using CoreApp.Data.Entities;
using CoreApp.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CoreApp.Data.EF
{
    public class AppDbContext:IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        #region DbSet
        public DbSet<Language> Languages { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AnnouncementUser> AnnouncementUsers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Contact> Contacts { set; get; }
        public DbSet<Feedback> Feedbacks { set; get; }
        public DbSet<Footer> Footers { set; get; }
        public DbSet<Page> Pages { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }
        public DbSet<ProductQuantity> ProductQuantities { set; get; }
        public DbSet<ProductTag> ProductTags { set; get; }
        public DbSet<Size> Sizes { set; get; }
        public DbSet<Slide> Slides { set; get; }
        public DbSet<Tag> Tags { set; get; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<WholePrice> WholePrices { get; set; }
        public DbSet<AdvertisementPage> AdvertisementPages { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementPosition> AdvertisementPositions { get; set; }


        #endregion

        /// <summary>
        /// Override function OnModelCreating 
        /// </summary>
        /// <param name="builder">builder</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Identity Config
            //Rename default table name in Identity

            builder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims").HasKey(x => x.Id);
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            builder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles").HasKey(x => new {x.RoleId, x.UserId});
            builder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens").HasKey(x => new {x.UserId});

            #endregion

            builder.AddConfiguration(new TagConfiguration());
            builder.AddConfiguration(new BlogTagConfiguration());
            builder.AddConfiguration(new ContactDetailConfiguration());
            builder.AddConfiguration(new FooterConfiguration());
            builder.AddConfiguration(new PageConfiguration());
            builder.AddConfiguration(new ProductTagConfiguration());
            builder.AddConfiguration(new SystemConfigConfiguration());
            builder.AddConfiguration(new AdvertisementPositionConfiguration());
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            var modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (EntityEntry entityEntry in modified)
            {
                if (!(entityEntry.Entity is IDateTracking changeOrAddedItem)) continue;
                if (entityEntry.State == EntityState.Added)
                {
                    changeOrAddedItem.DateCreated=DateTime.Now;
                }
                changeOrAddedItem.DateModified=DateTime.Now;
            }
            return base.SaveChanges();
        }
    }
    public class DesignTimeDbContextFactory :IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new AppDbContext(builder.Options);
        }
    }
}
