﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------
namespace _13
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class KharrasovGlazkiSaveEntities : DbContext
    {
        private static KharrasovGlazkiSaveEntities _context;

        public static KharrasovGlazkiSaveEntities GetContext()
        {
            if (_context == null)
                _context = new KharrasovGlazkiSaveEntities();
            return _context;
        }
        public KharrasovGlazkiSaveEntities()
            : base("name=KharrasovGlazkiSaveEntities")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        public virtual DbSet<Agent> Agent { get; set; }
        public virtual DbSet<AgentPriorityHistory> AgentPriorityHistory { get; set; }
        public virtual DbSet<AgentType> AgentType { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialCountHistory> MaterialCountHistory { get; set; }
        public virtual DbSet<MaterialType> MaterialType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCostHistory> ProductCostHistory { get; set; }
        public virtual DbSet<ProductMaterial> ProductMaterial { get; set; }
        public virtual DbSet<ProductSale> ProductSale { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<Shop> Shop { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
    }
}
