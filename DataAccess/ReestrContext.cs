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

            if (this.Database.IsSqlite())
            {
                this.Database.ExecuteSqlRaw(@"
                    CREATE TABLE IF NOT EXISTS ""ColdWaterLines"" (
	                ""Id""	INTEGER NOT NULL,
	                ""Year""	INTEGER NOT NULL,
	                ""Month""	INTEGER NOT NULL,
	                ""AccountNumber""	TEXT,
	                ""PersonId""	INTEGER,
	                ""SubjectId""	INTEGER NOT NULL,
	                ""AmountTypeId""	INTEGER,
	                ""IncBalance""	REAL NOT NULL,
	                ""IncBalanceDebit""	REAL NOT NULL,
	                ""IncBalanceCredit""	REAL NOT NULL,
	                ""ColdWater""	REAL NOT NULL,
	                ""WaterDisposal""	REAL NOT NULL,
	                ""ColdWaterCommon""	REAL NOT NULL,
	                ""ColdWaterIncrease""	REAL NOT NULL,
	                ""ColdWaterHotIncrease""	REAL NOT NULL,
	                ""ColdWaterHot""	REAL NOT NULL,
	                ""ColdWaterHotCommon""	REAL NOT NULL,
	                ""WaterDisposalCommon""	REAL NOT NULL,
	                ""ColdWaterHotIncCoeff""	REAL NOT NULL,
	                ""ColdWaterIncCoeff""	REAL NOT NULL,
	                ""HotWater""	REAL NOT NULL,
	                ""SummerWatering""	REAL NOT NULL,
	                ""Heating""	REAL NOT NULL,
	                ""Total""	REAL NOT NULL,
	                ""Penalty""	REAL NOT NULL,
	                ""OutBalance""	REAL NOT NULL,
	                ""OutBalanceDebit""	REAL NOT NULL,
	                ""OutBalanceCredit""	REAL NOT NULL,
	                CONSTRAINT ""PK_ColdWaterLines"" PRIMARY KEY(""Id"" AUTOINCREMENT),
	                CONSTRAINT ""FK_ColdWaterLines_Subjects_SubjectId"" FOREIGN KEY(""SubjectId"") REFERENCES ""Subjects""(""Id"") ON DELETE CASCADE,
	                CONSTRAINT ""FK_ColdWaterLines_AmountTypes_AmountTypeId"" FOREIGN KEY(""AmountTypeId"") REFERENCES ""AmountTypes""(""Id"") ON DELETE RESTRICT,
	                CONSTRAINT ""FK_ColdWaterLines_People_PersonId"" FOREIGN KEY(""PersonId"") REFERENCES ""People""(""Id"") ON DELETE RESTRICT);"
                );
            }

            if (this.Database.IsSqlServer())
            {
                this.Database.ExecuteSqlRaw(@"
                    IF not exists (select 1 from information_schema.tables where table_name = 'ColdWaterLines')
                    BEGIN
                      CREATE TABLE dbo.ColdWaterLines (
                        Id int IDENTITY,
                        Year int NOT NULL,
                        Month int NOT NULL,
                        AccountNumber nvarchar(max) NULL,
                        PersonId int NULL,
                        SubjectId int NOT NULL,
                        AmountTypeId int NULL,
                        IncBalance float NOT NULL,
                        IncBalanceDebit float NOT NULL,
                        IncBalanceCredit float NOT NULL,
                        ColdWater float NOT NULL,
                        WaterDisposal float NOT NULL,
                        ColdWaterCommon float NOT NULL,
                        ColdWaterIncrease float NOT NULL,
                        ColdWaterHotIncrease float NOT NULL,
                        ColdWaterHot float NOT NULL,
                        ColdWaterHotCommon float NOT NULL,
                        WaterDisposalCommon float NOT NULL,
                        ColdWaterHotIncCoeff float NOT NULL,
                        ColdWaterIncCoeff float NOT NULL,
                        HotWater float NOT NULL,
                        SummerWatering float NOT NULL,
                        Heating float NOT NULL,
                        Total float NOT NULL,
                        Penalty float NOT NULL,
                        OutBalance float NOT NULL,
                        OutBalanceDebit float NOT NULL,
                        OutBalanceCredit float NOT NULL,
                        CONSTRAINT PK_ColdWaterLines PRIMARY KEY CLUSTERED (Id)
                      )
                      ON [PRIMARY]
                      TEXTIMAGE_ON [PRIMARY]
  
                      CREATE INDEX IX_ColdWaterLines_AmountTypeId
                        ON ReestrBks.dbo.ColdWaterLines (AmountTypeId)
                        ON [PRIMARY]
  
                      CREATE INDEX IX_ColdWaterLines_PersonId
                        ON ReestrBks.dbo.ColdWaterLines (PersonId)
                        ON [PRIMARY]
  
                      CREATE INDEX IX_ColdWaterLines_SubjectId
                        ON ReestrBks.dbo.ColdWaterLines (SubjectId)
                        ON [PRIMARY]
  
                      ALTER TABLE ReestrBks.dbo.ColdWaterLines
                        ADD CONSTRAINT FK_ColdWaterLines_AmountTypes_AmountTypeId FOREIGN KEY (AmountTypeId) REFERENCES dbo.AmountTypes (Id)
  
                      ALTER TABLE ReestrBks.dbo.ColdWaterLines
                        ADD CONSTRAINT FK_ColdWaterLines_People_PersonId FOREIGN KEY (PersonId) REFERENCES dbo.People (Id)
  
                      ALTER TABLE ReestrBks.dbo.ColdWaterLines
                        ADD CONSTRAINT FK_ColdWaterLines_Subjects_SubjectId FOREIGN KEY (SubjectId) REFERENCES dbo.Subjects (Id) ON DELETE CASCADE
                    END"
                );
            }
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
        }

        public void CleanDataBase()
        {
            try
            {
                if (this.Database.IsSqlServer())
                {
                    this.Database.ExecuteSqlRaw(@"
                    Exec sp_msforeachtable 'ALTER INDEX ALL ON ? REBUILD WITH (FILLFACTOR = 80, SORT_IN_TEMPDB = ON, STATISTICS_NORECOMPUTE = ON);'
                    DBCC SHRINKDATABASE ([ReestrBks], 10);", new object[] { });
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }
    }
}
