IF OBJECT_ID('dbo.SourceTemplate','U') IS NOT NULL
   DROP TABLE [dbo].[SourceTemplate]
GO

CREATE TABLE [dbo].[SourceTemplate](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ClientId] [int] NULL,
	[TemplateFile] [varbinary](max) NOT NULL,
	[FileFormat] [varchar](15) NOT NULL,
	[IsStandard] [bit] NOT NULL DEFAULT ((0)),
	[TemplateName] [varchar](50) NOT NULL,
	[FileDelimiter] [varchar](5) NULL,
	[BaseMappingId] [int] NULL,
	[AcknowledgeBy] [int] NOT NULL DEFAULT ((0)),
	[AcknowledgeOn] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL  DEFAULT ((0)),
	[UpdatedOn] [datetime] NOT NULL DEFAULT (getdate()),
	[IsDeleted] [bit] NOT NULL  DEFAULT ((0)),
	CONSTRAINT SourceTemplate_ClientId_fk FOREIGN KEY (ClientId) REFERENCES Clients (id),
	CONSTRAINT SourceTemplate_BaseMappingId_fk FOREIGN KEY (BaseMappingId) REFERENCES SourceTemplate (Id)
	)
GO
