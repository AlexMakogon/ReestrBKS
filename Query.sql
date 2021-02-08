USE ReestrBks;  
GO

Exec sp_msforeachtable 'ALTER INDEX ALL ON ? REBUILD WITH (FILLFACTOR = 80, SORT_IN_TEMPDB = ON, STATISTICS_NORECOMPUTE = ON);'
GO

SELECT a.index_id, name, avg_fragmentation_in_percent  
FROM sys.dm_db_index_physical_stats (DB_ID(N'AdventureWorks2012'), OBJECT_ID(N'HumanResources.Employee'), NULL, NULL, NULL) AS a  
    JOIN sys.indexes AS b ON a.object_id = b.object_id AND a.index_id = b.index_id;   
GO

DBCC SHRINKDATABASE ([ReestrBks], 10);
GO