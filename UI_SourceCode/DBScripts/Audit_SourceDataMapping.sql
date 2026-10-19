IF OBJECT_ID('dbo.Audit_SourceDataMapping','U') IS NOT NULL
   DROP TABLE [dbo].[Audit_SourceDataMapping]
GO

CREATE TABLE [dbo].[Audit_SourceDataMapping](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[SourceDataMappingId] [int] NULL,
	[MappingId] [int] NULL,
	[SField] [varchar](45) NULL,
	[StgTable] [varchar](30) NULL,
	[StgCol] [varchar](20) NULL,
	[DefVal] [varchar](45) NULL,
	[DataType] [varchar](45) NULL,
    [DLength] [varchar](45) NULL,
	[IsMandatory] [bit] NULL,
	[DisplayName] [varchar](45) NULL,
    [IsConcat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[AddedOn] [datetime] NOT NULL CONSTRAINT [DF_Audit_SourceDataMapping_DefaultDateadded]  DEFAULT (getdate()))
GO

