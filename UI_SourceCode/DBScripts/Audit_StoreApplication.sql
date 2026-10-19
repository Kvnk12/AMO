IF OBJECT_ID('dbo.Audit_StoreApplication','U') IS NOT NULL
   DROP TABLE [dbo].[Audit_StoreApplication]
GO

CREATE TABLE [dbo].[Audit_StoreApplication](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[StoreApplicationId] [int] NULL,
	[MappingId] [int] NULL,
	[ApplicationName] [varchar](45) NULL,
	[StgDataTableName] [varchar](30) NULL,
	[AddedOn] [datetime] NOT NULL CONSTRAINT [DF_Audit_StoreApplication_DefaultDateadded]  DEFAULT (getdate()))
GO
