﻿using BankingAppliction.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingAppliction.Data  
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
