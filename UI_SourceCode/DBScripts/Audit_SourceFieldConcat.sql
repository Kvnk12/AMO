IF OBJECT_ID('dbo.Audit_SourceFieldConcat','U') IS NOT NULL
   DROP TABLE [dbo].[Audit_SourceFieldConcat]
GO

CREATE TABLE [dbo].[Audit_SourceFieldConcat](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[SourceFieldConcatId] [int] NULL,
	[SourceDataMappingId] [int] NULL,
	[MappingId] [int] NULL,
	[SourceField] [varchar](45) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[AddedOn] [datetime] NOT NULL CONSTRAINT [DF_Audit_SourceFieldConcat_DefaultDateadded]  DEFAULT (getdate()))
GO
