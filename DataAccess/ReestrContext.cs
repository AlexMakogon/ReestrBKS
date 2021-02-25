using ReestrBKS.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using ReestrBKS.DataAccess.Interfaces;
using System.Collections.Generic;
using EFCore.BulkExtensions;

namespace ReestrBKS.DataAccess
{
    public class ReestrContext : DbContext
    {
        public DbSet<HotWaterLine> HotWaterLines { get; set; }
        public DbSet<CommonHouseLine> CommonHouseLines { get; set; }
        public DbSet<ColdWaterLine> ColdWaterLines { get; set; }
        public DbSet<AmountType> AmountTypes { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectType> SubjectTypes { get; set; }

        public ReestrContext(DbContextOptions<ReestrContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(6000);
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AmountType amountType1 = new AmountType() { Id = 1, Name = "Лицевой счет" };
            AmountType amountType2 = new AmountType() { Id = 2, Name = "Вх. сальдо" };
            AmountType amountType3 = new AmountType() { Id = 3, Name = "Начислено (расчет)" };
            AmountType amountType4 = new AmountType() { Id = 4, Name = "Начислено (итого)" };
            AmountType amountType5 = new AmountType() { Id = 5, Name = "Разовые (перерасчеты" };
            AmountType amountType6 = new AmountType() { Id = 6, Name = "Оплачено" };
            AmountType amountType7 = new AmountType() { Id = 7, Name = "Исх. сальдо" };

            modelBuilder.Entity<AmountType>().HasData(new AmountType[] { amountType1, amountType2,
            amountType3, amountType4, amountType5, amountType6, amountType7 });

         //   modelBuilder.Entity<HotWaterLine>().ToTable("HotWaterLines");
         //   modelBuilder.Entity<CommonHouseLine>().ToTable("CommonHouseLines");
        }

        public void CleanDataBase()
        {
            try
            {
                this.Database.ExecuteSqlRaw(@"
                Exec sp_msforeachtable 'ALTER INDEX ALL ON ? REBUILD WITH (FILLFACTOR = 80, SORT_IN_TEMPDB = ON, STATISTICS_NORECOMPUTE = ON);'
                DBCC SHRINKDATABASE ([ReestrBks], 10);", new object[] { });
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }
    }
}
