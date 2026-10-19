IF OBJECT_ID('dbo.SourceDataMapping','U') IS NOT NULL
   DROP TABLE [dbo].[SourceDataMapping]
GO

CREATE TABLE [dbo].[SourceDataMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[MappingId] [int] NULL ,
	[SField] [varchar](45) NULL,
	[StgTable] [varchar](30) NOT NULL,
	[StgCol] [varchar](20) NOT NULL,
	[DefVal] [varchar](45) NULL,
	[DataType] [varchar](45) NOT NULL,
    [DLength] [varchar](45) NULL,
	[IsMandatory] [bit] NOT NULL CONSTRAINT [DF_SourceDataMapping_IsMandatory]  DEFAULT ((0)),
	[DisplayName] [varchar](45) NULL,
    [IsConcat] [bit] NOT NULL CONSTRAINT [DF_SourceDataMapping_ConcatFlag]  DEFAULT ((0)),
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_SourceDataMapping_DefaultDateCreated]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL CONSTRAINT [DF_SourceDataMapping_UpdatedBy]  DEFAULT ((0)),
	[UpdatedOn] [datetime] NOT NULL CONSTRAINT [DF_SourceDataMapping_DefaultDateUpdated]  DEFAULT (getdate()),
	CONSTRAINT SourceDataMapping_MappingId_fk FOREIGN KEY (MappingId) REFERENCES SourceTemplate (Id) )
GO
