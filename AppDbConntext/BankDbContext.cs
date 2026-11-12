using Assignment_04_EFC_.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_04_EFC_.AppDbConntext
{
    public class BankDbContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server =.; Database = NationalBankDb; Trusted_Connection = True; TrustServerCertificate = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerAccount>()
                .HasKey(ca => new { ca.CustomerId, ca.AccountNumber });

            modelBuilder.Entity<Account>()
                .Property(a => a.CurrentBalance).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.NationalId).IsUnique();

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.TransactionNumber).IsUnique();

            modelBuilder.Entity<Customer>()
                .Property(c => c.CustomerType).HasConversion<string>();

            modelBuilder.Entity<Account>()
                .Property(a => a.AccountType).HasConversion<string>();

            modelBuilder.Entity<Account>()
                .Property(a => a.AccountStatus).HasConversion<string>();

            modelBuilder.Entity<CustomerAccount>()
                .Property(ca => ca.OwnershipType).HasConversion<string>();

            modelBuilder.Entity<CustomerAccount>()
                .HasOne(ca => ca.Customer)
                .WithMany(c => c.CustomerAccounts)
                .HasForeignKey(ca => ca.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerAccount>()
                .HasOne(ca => ca.Account)
                .WithMany(a => a.CustomerAccounts)
                .HasForeignKey(ca => ca.AccountNumber)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Manager)
                .WithOne(m => m.Branch)
                .HasForeignKey<Branch>(b => b.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Branch)
                .WithMany(b => b.Accounts)
                .HasForeignKey(a => a.BranchCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountNumber)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
