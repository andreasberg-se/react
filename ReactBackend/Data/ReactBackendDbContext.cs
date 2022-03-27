using System;
using Pomelo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ReactBackend.Models;

namespace ReactBackend.Data
{

    public class ReactBackendDbContext : IdentityDbContext<ReactBackendUser>
    {
        public ReactBackendDbContext(DbContextOptions<ReactBackendDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Person> People { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<PersonLanguage> PersonLanguages { get; set; }

        public override DbSet<ReactBackendUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // primary keys
            modelBuilder.Entity<Person>().HasKey(p => p.PersonId);
            modelBuilder.Entity<City>().HasKey(c => c.CityId);
            modelBuilder.Entity<Country>().HasKey(co => co.CountryId);
            modelBuilder.Entity<Language>().HasKey(l => l.LanguageId);
            modelBuilder.Entity<PersonLanguage>().HasKey(pl => new { pl.PersonId, pl.LanguageId });

            // relationships (one to many)
            modelBuilder.Entity<City>()
                .HasOne(ci => ci.Country)
                .WithMany(co => co.Cities)
                .HasForeignKey(ci => ci.CountryId);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.City)
                .WithMany(ci => ci.People)
                .HasForeignKey(p => p.CityId);

            // relationships (many to many)
            modelBuilder.Entity<PersonLanguage>()
                .HasOne(pl => pl.Person)
                .WithMany(p => p.PersonLanguages)
                .HasForeignKey(pl => pl.PersonId);

            modelBuilder.Entity<PersonLanguage>()
                .HasOne(pl => pl.Language)
                .WithMany(l => l.PersonLanguages)
                .HasForeignKey(pl => pl.LanguageId);

            // seeding
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 1, Name = "Sverige" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 2, Name = "Norge" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 3, Name = "Danmark" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 4, Name = "Finland" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 5, Name = "Island" });

            modelBuilder.Entity<City>().HasData(new City { CityId = 1, CountryId = 1, Name = "Mellerud" });
            modelBuilder.Entity<City>().HasData(new City { CityId = 2, CountryId = 1, Name = "Göteborg" });
            modelBuilder.Entity<City>().HasData(new City { CityId = 3, CountryId = 1, Name = "Stockholm" });
            modelBuilder.Entity<City>().HasData(new City { CityId = 4, CountryId = 2, Name = "Oslo" });
            modelBuilder.Entity<City>().HasData(new City { CityId = 5, CountryId = 3, Name = "Köpenhamn" });
            modelBuilder.Entity<City>().HasData(new City { CityId = 6, CountryId = 4, Name = "Helsingfors" });
            modelBuilder.Entity<City>().HasData(new City { CityId = 7, CountryId = 5, Name = "Reykjavik" });

            modelBuilder.Entity<Person>().HasData(new Person { PersonId = 1, FirstName = "Andreas", LastName = "Berg", CityId = 1, Phone = "0000-000 000" });
            modelBuilder.Entity<Person>().HasData(new Person { PersonId = 2, FirstName = "Anders", LastName = "Andersson", CityId = 2, Phone = "1234-567 890" });
            modelBuilder.Entity<Person>().HasData(new Person { PersonId = 3, FirstName = "Maria", LastName = "Svensson", CityId = 3, Phone = "0987-654 321" });

            modelBuilder.Entity<Language>().HasData(new Language { LanguageId = 1, LanguageName = "Svenska" });
            modelBuilder.Entity<Language>().HasData(new Language { LanguageId = 2, LanguageName = "Norska" });
            modelBuilder.Entity<Language>().HasData(new Language { LanguageId = 3, LanguageName = "Danska" });
            modelBuilder.Entity<Language>().HasData(new Language { LanguageId = 4, LanguageName = "Finska" });
            modelBuilder.Entity<Language>().HasData(new Language { LanguageId = 5, LanguageName = "Isländska" });
            modelBuilder.Entity<Language>().HasData(new Language { LanguageId = 6, LanguageName = "Engelska" });
            modelBuilder.Entity<Language>().HasData(new Language { LanguageId = 7, LanguageName = "Tyska" });
            modelBuilder.Entity<Language>().HasData(new Language { LanguageId = 8, LanguageName = "Spanska" });
            modelBuilder.Entity<Language>().HasData(new Language { LanguageId = 9, LanguageName = "Italienska" });
            modelBuilder.Entity<Language>().HasData(new Language { LanguageId = 10, LanguageName = "Ryska" });

            modelBuilder.Entity<PersonLanguage>().HasData(new PersonLanguage { PersonId = 1, LanguageId = 1 });
            modelBuilder.Entity<PersonLanguage>().HasData(new PersonLanguage { PersonId = 1, LanguageId = 6 });
            modelBuilder.Entity<PersonLanguage>().HasData(new PersonLanguage { PersonId = 2, LanguageId = 1 });
            modelBuilder.Entity<PersonLanguage>().HasData(new PersonLanguage { PersonId = 2, LanguageId = 6 });
            modelBuilder.Entity<PersonLanguage>().HasData(new PersonLanguage { PersonId = 3, LanguageId = 1 });
            modelBuilder.Entity<PersonLanguage>().HasData(new PersonLanguage { PersonId = 3, LanguageId = 9 });

            // seeding (user roles and standard account)
            string adminRoleId = Guid.NewGuid().ToString();
            string moderatorRoleId = Guid.NewGuid().ToString();
            string userRoleId = Guid.NewGuid().ToString();

            string accountId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{ Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{ Id = moderatorRoleId, Name = "Moderator", NormalizedName = "MODERATOR" });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{ Id = userRoleId, Name = "User", NormalizedName = "USER" });

            PasswordHasher<ReactBackendUser> passwordHasher = new PasswordHasher<ReactBackendUser>();

            modelBuilder.Entity<ReactBackendUser>().HasData(new ReactBackendUser
            {
                Id = accountId,
                Email = "mail@domain.com",
                NormalizedEmail = "MAIL@DOMAIN.COM",
                UserName = "mail@domain.com",
                NormalizedUserName = "MAIL@DOMAIN.COM",
                PasswordHash = passwordHasher.HashPassword(null, "pass1234"),
                FirstName = "Andreas",
                LastName = "Berg",
                Birthdate = new DateTime(2000, 10, 31)
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { UserId = accountId, RoleId = userRoleId });
        }
    }

}