IF OBJECT_ID('dbo.Audit_SourceTemplate','U') IS NOT NULL
   DROP TABLE [dbo].[Audit_SourceTemplate]
GO

CREATE TABLE [dbo].[Audit_SourceTemplate](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[MappingId] [int] NULL,
	[ClientId] [int]  NULL ,
	[TemplateFile] [varbinary](max) NULL,
	[FileFormat] [varchar](15) NULL,
	[IsStandard] [bit] NULL ,
	[TemplateName] [varchar](50) NULL,
	[FileDelimiter] [varchar](5) NULL,
	[BaseMappingId] [int] NULL,
	[AcknowledgeBy] [int] NULL,
	[AcknowledgeOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL ,
	[UpdatedOn] [datetime] NULL,
	[AddedOn] [datetime] NOT NULL CONSTRAINT [DF_Audit_SourceTemplate_DefaultDateadded]  DEFAULT (getdate()))
GO
