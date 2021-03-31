using Microsoft.EntityFrameworkCore;
using Netaba.Data.Enteties;
using Netaba.Data.Enums;
using System.Security.Cryptography;
using System.Text;

using System;

namespace Netaba.Data.Contexts
{
    public class HashGenerator
    {
        static public byte[] GetHash(string password) =>
            MD5.HashData(Encoding.UTF8.GetBytes(password));
    }

    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Console.WriteLine("OnModelCreating: Users.");

            User Admin = new() {
                                 Id = 1,
                                 Name = nameof(Role.SuperAdmin), 
                                 PassHash = HashGenerator.GetHash("123"), 
                                 Role = Role.SuperAdmin
                               };

            modelBuilder.Entity<User>().HasData(new User[] { Admin });
        }
    }
}
