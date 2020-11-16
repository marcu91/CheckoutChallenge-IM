using Gateway.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gateway.Data
{
    /// <summary>
    /// Database Context for Entity Framework
    /// </summary>
    public class GatewayDBContext : DbContext
    {
        public GatewayDBContext(DbContextOptions<GatewayDBContext> options) : base(options)
        {

        }

        #region DBSets 

        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<CardDetails> CardDetails { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


        }
    }
}