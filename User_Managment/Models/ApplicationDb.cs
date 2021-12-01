using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Managment.Models
{
    public class ApplicationDb:DbContext
    {
        public ApplicationDb(DbContextOptions options):base(options)
        {
           
        }
        public DbSet<User> users { get; set; }
        public DbSet<Role>  roles { get; set; }
        public DbSet<RoleUser>  roleUsers { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var user= modelBuilder.Entity<User>();
            var role = modelBuilder.Entity<Role>();
            var roleUser = modelBuilder.Entity<RoleUser>();


           user.HasKey(i => i.Id);                
           user.Property(p => p.Name).HasMaxLength(40);
           user.Property(p => p.Email).HasMaxLength(40).IsRequired();
           user.Property(p => p.Password).IsRequired();
           user.HasMany(t => t.roles)
               .WithMany(r => r.users);
            role.HasKey(i => i.Id);
            role.Property(p => p.Name).HasMaxLength(40).IsRequired();


            roleUser.HasKey(i =>new { i.rolesId , i.usersId});
            
               
                
        }

    }
}
