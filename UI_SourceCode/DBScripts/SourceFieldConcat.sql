IF OBJECT_ID('dbo.SourceFieldConcat','U') IS NOT NULL
   DROP TABLE [dbo].[SourceFieldConcat]
GO

CREATE TABLE [dbo].[SourceFieldConcat](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[SourceDataMappingId] [int] NULL,
	[MappingId] [int] NULL,
	[SourceField] [varchar](45) NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_SourceFieldConcat_DefaultDateCreated]  DEFAULT (getdate()),
	CONSTRAINT SourceFieldConcat_SourceDataMappingId_fk FOREIGN KEY (SourceDataMappingId) REFERENCES SourceDataMapping (Id),
	CONSTRAINT SourceFieldConcat_MappingId_fk FOREIGN KEY (MappingId) REFERENCES SourceTemplate (Id) )
GO
