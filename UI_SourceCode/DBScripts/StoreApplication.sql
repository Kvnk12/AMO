IF OBJECT_ID('dbo.StoreApplication','U') IS NOT NULL
   DROP TABLE [dbo].[StoreApplication]
GO

CREATE TABLE [dbo].[StoreApplication](
	[Id] [int] IDENTITY(1,1) NOT NULL  PRIMARY KEY,
	[MappingId] [int] NULL,
	[ApplicationName] [varchar](45) NOT NULL,
	[StgDataTableName] [varchar](30) NOT NULL,
 CONSTRAINT StoreApplication_MappingId_fk FOREIGN KEY (MappingId) REFERENCES SourceTemplate (id))	

GO

