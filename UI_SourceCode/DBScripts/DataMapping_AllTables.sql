IF OBJECT_ID('dbo.Audit_SourceFieldConcat','U') IS NOT NULL
   DROP TABLE [dbo].[Audit_SourceFieldConcat]
GO

IF OBJECT_ID('dbo.Audit_SourceDataMapping','U') IS NOT NULL
   DROP TABLE [dbo].[Audit_SourceDataMapping]
GO

IF OBJECT_ID('dbo.Audit_SourceTemplate','U') IS NOT NULL
   DROP TABLE [dbo].[Audit_SourceTemplate]
GO

IF OBJECT_ID('dbo.Audit_StoreApplication','U') IS NOT NULL
   DROP TABLE [dbo].[Audit_StoreApplication]
GO

IF OBJECT_ID('dbo.SourceFieldConcat','U') IS NOT NULL
   DROP TABLE [dbo].[SourceFieldConcat]
GO

IF OBJECT_ID('dbo.SourceDataMapping','U') IS NOT NULL
   DROP TABLE [dbo].[SourceDataMapping]
GO

IF OBJECT_ID('dbo.StoreApplication','U') IS NOT NULL
   DROP TABLE [dbo].[StoreApplication]
GO

IF OBJECT_ID('dbo.Incident_STG','U') IS NOT NULL
   DROP TABLE [dbo].[Incident_STG]
GO

IF OBJECT_ID('dbo.SourceTemplate','U') IS NOT NULL
   DROP TABLE [dbo].[SourceTemplate]
GO

IF OBJECT_ID('dbo.JobErrorLog','U') IS NOT NULL
   DROP TABLE [dbo].[JobErrorLog]
GO

IF OBJECT_ID('dbo.JobDetail','U') IS NOT NULL
   DROP TABLE [dbo].[JobDetail]
GO

GO
IF OBJECT_ID('dbo.SourceRejectedRecord','U') IS NOT NULL
   DROP TABLE [dbo].[SourceRejectedRecord]
GO

CREATE TABLE [dbo].[SourceRejectedRecord](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[MappingId] [int] NOT NULL,
	[JobId] [int] NOT NULL,
	[Rejectid] [int] NOT NULL,
	[RejectReason] [nvarchar](1000) NULL,
	[SourceDataField] [nvarchar](100) NOT NULL,
	[SourceDataFieldVal] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_SourceRejectedRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[JobDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MappingId] [int] NULL,
	[ClientId] [int] NULL,
	[ApplicationId] [int] NULL,
	[FileName] [varchar](100) NULL,
	[UserId] [varchar](100) NULL,
	[JobStartTime] [datetime] NOT NULL CONSTRAINT [DF_JobDetail_DefaultJobStartTime]  DEFAULT (getdate()),
	[JobEndTime] [datetime] NULL,
	[RunStatus] [varchar](45) NOT NULL,
	[TotalRecordRecieved] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultTotalRecordRecieved]  DEFAULT ((0)),
	[AddRowCount] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultAddRowCount]  DEFAULT ((0)),
	[UpdateRowCount] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultUpdateRowCount]  DEFAULT ((0)),
	[DuplicateRowCount] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultDuplicateRowCount]  DEFAULT ((0)),
	[RejectedRecordsCount] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultRejectedRecordsCount]  DEFAULT ((0)),
	[IgnoredRecords] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultIgnoredRecords]  DEFAULT ((0)),
	[IsChecked] [bit] NOT NULL CONSTRAINT [DF_JobDetail_DefaultIsChecked]  DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[JobErrorLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NULL,
	[Clientid] [int] NULL,
	[MappingId] [int] NULL,
	[RejectId] [int] NULL,
	[FileName] [varchar](100) NULL,
	[FielIncident] [varchar](150) NULL,
	[FieldName] [varchar](100) NULL,
	[FieldValue] [varchar](1000) NULL,
	[ELocation] [varchar](50) NULL,
	[ErrorDesc] [varchar](500) NULL,
	[PrcsAuditId] [int] NULL,
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_JobErrorLog_CreatedOn]  DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


IF EXISTS (SELECT * FROM   sys.objects  WHERE  object_id = OBJECT_ID(N'[dbo].[GetGlobalMappingIdentifier]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))
  DROP FUNCTION [dbo].[GetGlobalMappingIdentifier]
GO

CREATE FUNCTION dbo.GetGlobalMappingIdentifier 
(
	-- Add the parameters for the function here
	@MappingId int
)
RETURNS varchar(100)
AS
BEGIN
	-- Declare the return variable here
	Declare @ClientName varchar(100)
	select @ClientName= REPLACE(cl.ClientName,' ','') + '_' +
						CONVERT(varchar(10), sa.ApplicationId)  + '_'+ 
						CONVERT(varchar(10), @MappingId)   
		    from sourcetemplate st
	join Clients cl
	on st.ClientId=cl.id
	join StoreApplication sa
	on sa.MappingId=MappingId
	where st.id=@MappingId

	RETURN @ClientName

END
GO



CREATE TABLE [dbo].[SourceTemplate](
	[Id] [int] NOT NULL PRIMARY KEY,
	[GlobalMappingIdentifier]  AS ([dbo].[GetGlobalMappingIdentifier]([Id])),
	[ClientId] [int] NULL,
	[TemplateFile] [varbinary](max) NULL,
	[FileFormat] [varchar](15) NULL,
	[IsStandard] [bit] NOT NULL DEFAULT ((0)),
	[TemplateName] [varchar](50) NULL,
	[FileDelimiter] [varchar](5) NULL,
	[BaseMappingId] [int] NULL,
	[MappingDone] [bit] NOT NULL DEFAULT ((0)),
	[AcknowledgeBy] [int] NULL ,
	[AcknowledgeOn] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL  DEFAULT ((0)),
	[UpdatedOn] [datetime] NOT NULL DEFAULT (getdate()),
	[IsDeleted] [bit] NOT NULL DEFAULT ((0)),	
	CONSTRAINT SourceTemplate_ClientId_fk FOREIGN KEY (ClientId) REFERENCES Clients (id),
	CONSTRAINT SourceTemplate_BaseMappingId_fk FOREIGN KEY (BaseMappingId) REFERENCES SourceTemplate (Id)
	)
GO



CREATE TABLE [dbo].[StoreApplication](
	[MappingId] [int] NOT NULL ,
	[ApplicationId] [int] NOT NULL ,
 CONSTRAINT StoreApplication_MappingId_fk FOREIGN KEY (MappingId) REFERENCES SourceTemplate (id),	
 CONSTRAINT StoreApplication_ApplicationId_fk FOREIGN KEY (ApplicationId) REFERENCES AppStoreApplication (id),
 PRIMARY KEY (MappingId,ApplicationId)) 



CREATE TABLE [dbo].[Audit_SourceFieldConcat](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[SourceFieldConcatId] [int] NULL,
	[SourceDataMappingId] [int] NULL,
	[MappingId] [int] NULL,
	[SourceField] [varchar](45) NULL,
	[SeparatedBy] [varchar](10) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[AddedOn] [datetime] NOT NULL CONSTRAINT [DF_Audit_SourceFieldConcat_DefaultDateadded]  DEFAULT (getdate()))
GO

GO


CREATE TABLE [dbo].[SourceDataMapping](
	[Id] [int] NOT NULL PRIMARY KEY,
	[MappingId] [int] NOT NULL ,
	[SField] [varchar](100)  NULL,
	[MappingTable] [varchar](100)  NULL,
	[MappingCol] [varchar](100) NOT NULL,
	[DefVal] [varchar](100) NULL,
	[DataType] [varchar](100) NULL,
	[MapColDesc] [varchar](400) NULL,
    [DLength] int NULL,
	[IsMandatory] [bit] NOT NULL CONSTRAINT [DF_SourceDataMapping_IsMandatory]  DEFAULT ((0)),
	[DisplayName] [varchar](100) NULL,   
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_SourceDataMapping_DefaultDateCreated]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL CONSTRAINT [DF_SourceDataMapping_UpdatedBy]  DEFAULT ((0)),
	[UpdatedOn] [datetime] NOT NULL CONSTRAINT [DF_SourceDataMapping_DefaultDateUpdated]  DEFAULT (getdate()),
	CONSTRAINT SourceDataMapping_MappingId_fk FOREIGN KEY (MappingId) REFERENCES SourceTemplate (Id) )
GO



CREATE TABLE [dbo].[SourceFieldConcat](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[SourceDataMappingId] [int] NOT NULL,
	[MappingId] [int] NOT NULL,
	[SourceField] [varchar](100) NOT NULL,
	[SeparatedBy] [varchar](10) NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_SourceFieldConcat_DefaultDateCreated]  DEFAULT (getdate()),
	CONSTRAINT SourceFieldConcat_SourceDataMappingId_fk FOREIGN KEY (SourceDataMappingId) REFERENCES SourceDataMapping (Id),
	CONSTRAINT SourceFieldConcat_MappingId_fk FOREIGN KEY (MappingId) REFERENCES SourceTemplate (Id) )
GO



CREATE TABLE [dbo].[Audit_StoreApplication](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[StoreApplicationId] [int] NULL,
	[MappingId] [int] NULL,
	[ApplicationId] [int] NULL,
	[AddedOn] [datetime] NOT NULL CONSTRAINT [DF_Audit_StoreApplication_DefaultDateadded]  DEFAULT (getdate()))
GO

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




CREATE TABLE [dbo].[Audit_SourceDataMapping](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[SourceDataMappingId] [int] NULL,
	[MappingId] [int] NULL,
	[SField] [varchar](100) NULL,
	[MappingTable] [varchar](100) NULL,
	[MappingCol] [varchar](100) NULL,
	[MapColDesc] [varchar](400) NULL,
	[DefVal] [varchar](100) NULL,
	[DataType] [varchar](100) NULL,
    [DLength] [varchar](100) NULL,
	[IsMandatory] [bit] NULL,
	[DisplayName] [varchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[AddedOn] [datetime] NOT NULL CONSTRAINT [DF_Audit_SourceDataMapping_DefaultDateadded]  DEFAULT (getdate()))
GO

-----
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF OBJECT_ID('dbo.Incident_STG','U') IS NOT NULL
   DROP TABLE [dbo].[Incident_STG]
GO

CREATE TABLE [dbo].[Incident_stg](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Clientid] [int] NULL,
	[Incidentid] [nvarchar](128) NOT NULL,
	[Recordincluded] [nvarchar](6) NULL,
	[Createddatetime] [datetime] NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Title] [nvarchar](278) NULL,
	[Priority] [nvarchar](8) NOT NULL,
	[Impactscope] [nvarchar](32) NULL,
	[Urgency] [nvarchar](20) NULL,
	[Type] [nvarchar](255) NOT NULL,
	[Outagetype] [nvarchar](38) NULL,
	[Divisionimpacted] [nvarchar](112) NULL,
	[Businessunitimpacted] [nvarchar](250) NULL,
	[Servicecategory] [nvarchar](22) NULL,
	[Servicesubcategory] [nvarchar](44) NULL,
	[Servicearea] [nvarchar](54) NULL,
	[Regionimpacted] [nvarchar](26) NULL,
	[Locationsimpacted] [nvarchar](74) NULL,
	[Customerdept] [nvarchar](100) NULL,
	[Customerlastname] [nvarchar](38) NULL,
	[Customerfirstname] [nvarchar](34) NULL,
	[Customertitle] [nvarchar](80) NULL,
	[Customersatisfaction] [nvarchar](6) NULL,
	[Impacttype] [nvarchar](32) NULL,
	[Impactdescription] [nvarchar](8) NULL,
	[Configitemid] [nvarchar](250) NULL,
	[Configtype] [nvarchar](250) NULL,
	[Configitemname] [nvarchar](1024) NOT NULL,
	[Environmentimpacted] [nvarchar](54) NULL,
	[Module] [nvarchar](120) NULL,
	[Nodename] [nvarchar](84) NULL,
	[Rootcausetype] [nvarchar](38) NULL,
	[Rootcausedescription] [nvarchar](32) NULL,
	[Resolutiontype] [nvarchar](64) NULL,
	[Resolutiondescription] [nvarchar](510) NULL,
	[Solutionid] [nvarchar](32) NULL,
	[Deploymentrelease] [nvarchar](50) NULL,
	[Relatedchangeids] [nvarchar](32) NULL,
	[Relatedids] [nvarchar](32) NULL,
	[Relatedproblemids] [nvarchar](32) NULL,
	[Helpdesk] [nvarchar](256) NULL,
	[Assignedtoperson] [nvarchar](32) NULL,
	[Assignedtosupportgroup] [nvarchar](250) NULL,
	[Createdbygroup] [nvarchar](62) NULL,
	[Createdbyperson] [nvarchar](256) NULL,
	[Createdtimezone] [nvarchar](32) NULL,
	[Createdtime] [nvarchar](8) NULL,
	[Acknowlededbygroup] [nvarchar](58) NULL,
	[Acknowlededbyperson] [nvarchar](56) NULL,
	[Acknowledgeddatetime] [datetime] NOT NULL,
	[Acknowledgedtimezone] [nvarchar](32) NULL,
	[Acknowledgedtime] [nvarchar](8) NULL,
	[Resolvedbygroup] [nvarchar](250) NULL,
	[Resolvedbyperson] [nvarchar](56) NULL,
	[Resolvedbyvendor] [nvarchar](216) NULL,
	[Resolutionefforthours] [nvarchar](32) NULL,
	[Resolutiondatetime] [datetime] NOT NULL,
	[Resolvedtimezone] [nvarchar](32) NULL,
	[Resolutiontime] [nvarchar](8) NULL,
	[Servicerestoredbygroup] [nvarchar](58) NULL,
	[Servicerestoredbyperson] [nvarchar](52) NULL,
	[Servicerestoreddatetime] [datetime] NULL,
	[Servicerestoredtimezone] [nvarchar](32) NULL,
	[Servicerestoredtime] [nvarchar](8) NULL,
	[Lastupdatedbyperson] [nvarchar](120) NULL,
	[Lastupdateddatetime] [datetime] NULL,
	[Modificationnotes] [nvarchar](255) NULL,
	[Datasource] [nvarchar](32) NULL,
	[Datasourcenotes] [nvarchar](255) NULL,
	[Uploaddate] [datetime] NULL,
--	[Uploadedby] [nvarchar](32) NULL,
	[Isdeleted] [bit] NULL,
	[Assignedbygroup] [nvarchar](250) NULL,
	[Assignedbyperson] [nvarchar](20) NULL,
	[Assigneddatetime] [datetime] NOT NULL,
	[Esitmatedresponsetimebygroup] [datetime] NULL,
	[Esitmatedresponsedatetime] [datetime] NULL,
	[Waivedindicator] [nvarchar](1) NULL,
	[Esitmatedresolutiondate] [datetime] NULL,
	[Totalsuspenddurationassignedtoack] [int] NULL,
	[Totalsuspenddurationacktoer] [int] NULL,
	[Totalsuspenddurationertoresolution] [int] NULL,
	[Resolvedindicator] [nvarchar](1) NULL,
	[JobID] [int] NULL,
	[IsNTTGroup] [nvarchar](10) NULL,
	[SupportGroupType] [nvarchar](128) NULL,
	[InProgressdatetime] [datetime] NULL,
	[Closeddatetime] [datetime] NULL,
	[ScheduledstartdateTime] [datetime] NULL,
	[ScheduledenddateTime] [datetime] NULL,
	[Scheduledchangecount] [int] NULL,
	[Company] [nvarchar](120) NULL,
	[Site] [nvarchar](120) NULL,
	[Assigneecompany] [nvarchar](120) NULL,
	[Assigneeorganization] [nvarchar](120) NULL,
	[Estimatedworkeffort] [int] NULL,
	[Actualworkeffort] [int] NULL,
	[ActualstartdateTime] [datetime] NULL,
	[Recyclecount] [int] NULL,
	[Statusreason] [nvarchar](2000) NULL,
	[ConfigItemCategory] [nvarchar](250) NULL,
	[ClosureCategory] [nvarchar](256) NULL,
	[Severity] [nvarchar](256) NULL,
	[WorkGroupLevel] [nvarchar](256) NULL,
	[AffectedItemName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Incident_stg] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

Begin ---description
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Incident ID of the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'incidentid'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ticket created date time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Createddatetime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Status of the ticket (Assigned, Pending, Resolved, closed etc)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'status'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Description'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short description or title of the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'title'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Priority of the ticket(P1/P2/P3/P4 (Or) Critical, High, Medium, Low)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'priority'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Evaluation of Impact and its scope' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'impactscope'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Evaluation of Effect' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'urgency'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ticket Severity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'severity'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ticket type (Incident, Problem, SR)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'type'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of Outage(Service availability)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'outagetype'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line of business that is impacted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'divisionimpacted'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Business unit/Business Division Impacted that is impacted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'businessunitimpacted'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Area in which Service is offered/used' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'servicearea'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SubCategory of Service' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'servicesubcategory'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Geographical region in which the service is impacted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'regionimpacted'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Location in which the service is impacted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'locationsimpacted'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Department of the customer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'customerdept'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer''s Last name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'customerlastname'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer''s First name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'customerfirstname'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title of the Customer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'customertitle'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer satisfication level' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'customersatisfaction'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of impact (Business, Process, Resource, Others, etc)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'impacttype'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of Impact' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'impactdescription'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Impacted Environment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'environmentimpacted'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'unique ID identifying the Asset/Hardware/Application(Physical/Virtual)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'configitemid'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of configuration item' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'configtype'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of Configuration item' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'configitemname'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Module in the application/Asset involved in the issue' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'module'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Node name of the module' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'nodename'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the root cause (e.g. Software, Data Issue, Password reset etc)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'rootcausetype'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the root cause' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'rootcausedescription'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of Resolution(service Restart, bug etc)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'resolutiontype'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of Resolution(service Restart, bug etc)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'resolutiondescription'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Solution offered' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'solutionid'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last release in the application' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'deploymentrelease'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Change Request ID corresponding to previous release' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'relatedchangeids'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Other related IDs such as Task ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'relatedids'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Problem ID if any created for this' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'relatedproblemids'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Helpdesk name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'helpdesk'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person assigned to work on this' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'assignedtoperson'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Support group assigned to work on this ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'assignedtosupportgroup'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group that created the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'createdbygroup'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person in the group created the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'createdbyperson'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time zone in which the ticket is created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'createdtimezone'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time when the ticket is created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'createdtime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group that acknowledge the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'acknowlededbygroup'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person in the group that acknowledge the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'acknowlededbyperson'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time, the ticket is acknowledged' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'acknowledgeddatetime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time zone in which the ticket is acknowledged' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'acknowledgedtimezone'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time when the ticket is acknowledged' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'acknowledgedtime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group that resolved the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'resolvedbygroup'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Preson in the resolved group that resolved the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'resolvedbyperson'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Vendor that reolved the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'resolvedbyvendor'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time taken for resolving the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'resolutionefforthours'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time, the ticket is resolved' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'resolutiondatetime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time zone in which the ticket is resolved' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'resolvedtimezone'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group that restored the service' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'servicerestoredbygroup'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person in the group that restored the service' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Servicerestoredbyperson'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time in which the service is restored' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Servicerestoreddatetime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time zone in which the service is restored' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Servicerestoredtimezone'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time of "Service restored datetime"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Servicerestoredtime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person that last updated the ticket"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'lastupdatedbyperson'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and Time when the ticket is last updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'lastupdateddatetime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes added at the time of modification' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'modificationnotes'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group that assigned the ticket to another group' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'assignedbygroup'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person in the "Assignedbygroup" that assigned the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'assignedbyperson'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and Time when the ticket is assigned' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'assigneddatetime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group that updated the "Estimate time of response"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'esitmatedresponsetimebygroup'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estiamted response time by group/person' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Esitmatedresponsedatetime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Flag indicating if the ticket is waived or not' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Waivedindicator'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date estimated for resolution of ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'esitmatedresolutiondate'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total suspended duration from ticket assignment to ticket acknowledgement' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Totalsuspenddurationassignedtoack'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total suspended duration from ticket acknowledgment to ticket estimated resolution' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Totalsuspenddurationacktoer'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total suspended duration from ticket estimated resolution to actual resolution' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Totalsuspenddurationertoresolution'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Flag that indicates if ticket is resolved' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Resolvedindicator'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Support group Type assigned to work on this ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'SupportGroupType'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time, the ticket is in progress status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'InProgressdatetime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time, the ticket is Closed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Closeddatetime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time, the ticket is scheduled to start to work' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'ScheduledstartdateTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time, the ticket is scheduled to End to work' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'ScheduledenddateTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Count of Schedule change' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Scheduledchangecount'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Company name -ticket belongs to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Company'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Address of site location - ticket belongs to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Site'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Company name - ticket assigned to person' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Assigneecompany'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Assignee organization name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Assigneeorganization'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total estimated effort for ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Estimatedworkeffort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total actual effort for ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Actualworkeffort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time, the ticket is actually started by Assignee' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'ActualstartdateTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of time ticket  Re-opened after marked for Resolved' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Recyclecount'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reason/notes of Status on Hold/Wait/Suspend/Pending/Reopend' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Statusreason'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Yes/No, If a record can be included in the calculation or not' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Recordincluded'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Source of Data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Datasource'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes on the DataSource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Datasourcenotes'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of Upload' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Uploaddate'
--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Job/Person that uploaded data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'Uploadedby'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'When ticket is closed, a category is assigned (reason)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'ClosureCategory'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This ar eworkgroup levels 1,2,3,4,5 (2 is the default for support services)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'WorkGroupLevel'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Incident Affected Item Name / Application Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'incident_stg', @level2type=N'COLUMN',@level2name=N'AffectedItemName'
end

GO

IF OBJECT_ID('dbo.JobDetail','U') IS NOT NULL
   DROP TABLE [dbo].[JobDetail]
GO

GO

CREATE TABLE [dbo].[JobDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MappingId] [int] NULL,
	[ClientId] [int] NULL,
	[ApplicationId] [int] NULL,
	[FileName] [varchar](100) NULL,
	[UserId] [varchar](100) NULL,
	[JobStartTime] [datetime] NOT NULL CONSTRAINT [DF_JobDetail_DefaultJobStartTime]  DEFAULT (getdate()),
	[JobEndTime] [datetime] NULL,
	[RunStatus] [varchar](45) NOT NULL,
	[TotalRecordRecieved] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultTotalRecordRecieved]  DEFAULT ((0)),
	[AddRowCount] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultAddRowCount]  DEFAULT ((0)),
	[UpdateRowCount] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultUpdateRowCount]  DEFAULT ((0)),
	[DuplicateRowCount] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultDuplicateRowCount]  DEFAULT ((0)),
	[RejectedRecordsCount] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultRejectedRecordsCount]  DEFAULT ((0)),
	[IgnoredRecords] [int] NOT NULL CONSTRAINT [DF_JobDetail_DefaultIgnoredRecords]  DEFAULT ((0)),
	[IsChecked] [bit] NOT NULL CONSTRAINT [DF_JobDetail_DefaultIsChecked]  DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF OBJECT_ID('dbo.JobErrorLog','U') IS NOT NULL
   DROP TABLE [dbo].[JobErrorLog]
GO

CREATE TABLE [dbo].[JobErrorLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NULL,
	[Clientid] [int] NULL,
	[MappingId] [int] NULL,
	[RejectId] [int] NULL,
	[FileName] [varchar](100) NULL,
	[FieldName] [varchar](100) NULL,
	[FieldValue] [varchar](1000) NULL,
	[ELocation] [varchar](50) NULL,
	[ErrorDesc] [varchar](500) NULL,
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_JobErrorLog_CreatedOn]  DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


GO


----

GO



IF OBJECT_ID('dbo.JobProcess','U') IS NOT NULL
   DROP TABLE [dbo].[JobProcess]
GO

CREATE TABLE [dbo].[JobProcess](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[JobId] [int] NOT NULL ,
	[PrcsMode] [varchar](15) NULL,
	[PrcsType] [varchar](50) NULL,
	[PrcsName] [varchar](100) NULL,
	[PrimaryMessage] [varchar](500) NULL,
	[SecondaryMessage] [varchar](500) NULL,
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_DbPrcsAuditDetails_CreatedOn]  DEFAULT (getdate()))
GO



IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MappingIDSequence]') AND type = 'SO')
	CREATE SEQUENCE dbo.MappingIDSequence  
    START WITH 1  
    INCREMENT BY 1 ;  
GO 

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SourceDataMappingIDSequence]') AND type = 'SO')
	CREATE SEQUENCE dbo.SourceDataMappingIDSequence  
    START WITH 1  
    INCREMENT BY 1 ;  
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNextSequenceValue]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE dbo.[GetNextSequenceValue]
END
go

CREATE PROCEDURE [dbo].[GetNextSequenceValue] 
	@SequenceName varchar(100)
	AS
	BEGIN
	if (@SequenceName='MappingIDSequence') 
		SELECT NEXT VALUE FOR dbo.MappingIDSequence; 
	else if (@SequenceName='SourceDataMappingIDSequence') 
		SELECT NEXT VALUE FOR dbo.SourceDataMappingIDSequence; 
	else
		select 'Invalid Sequence Name'
	END
go

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTableColumnNames]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE dbo.[GetTableColumnNames]
END
go



IF OBJECT_ID('dbo.SlaCalSetup','U') IS NOT NULL
   DROP TABLE [dbo].[SlaCalSetup]
GO

CREATE TABLE [dbo].[SlaCalSetup](
[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
[ClientId] [int] NOT NULL,
[InputFiled] [varchar](45) NULL,
[IFiledColValue] [varchar](45) NULL,
[IFiledDefaultDataType] [varchar](15) NULL, 
[IFiledDefaultDataValue] [varchar](45) NULL,
[IPriority][int] NOT NULL,
[RComment] [varchar](100) NULL, 
[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_SlaCalSetup_CreatedOn]  DEFAULT (getdate()))
GO

go 

go

  if COL_LENGTH('MetricSpecification', 'Priority') IS NULL
    begin
		 ALTER TABLE [dbo].[MetricSpecification] ADD [Priority] INT NULL
	end
go

	if COL_LENGTH('MetricSpecification', 'Clocksetup') IS NULL
    begin
		 ALTER TABLE [dbo].[MetricSpecification] ADD [Clocksetup] INT NULL
	end

GO

UPDATE [dbo].[MetricSpecification] SET [Priority] = 3, Clocksetup = 1

GO

 go

 IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SetSLADrivedValues]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE dbo.[SetSLADrivedValues]
END

go

CREATE PROCEDURE [dbo].[SetSLADrivedValues] @ClientId [int], @JobId [int]
AS
BEGIN
/******************* Declaration of the Variables ******************************************************/
  DECLARE @ResponseCalcXcols AS nvarchar(1000)
  DECLARE @ResponseCalcYcols AS nvarchar(1000)
  DECLARE @ResolutionCalcXcols AS nvarchar(1000)
  DECLARE @ResolutionCalcYcols AS nvarchar(1000)
  DECLARE @ResponseCalcXquery AS nvarchar(3500)
  DECLARE @ResponseCalcYquery AS nvarchar(3500)
  DECLARE @ResolutionCalcXquery AS nvarchar(3500)
  DECLARE @ResolutionCalcYquery AS nvarchar(3500)
  
/******Setting up Derived Values as NULL ***********************************************************/ 	 
  UPDATE Incident
     SET Resptargettime = NULL,
	     EstResptarget =  NULL,
	     Reslntargettime = NULL,
	     ResponseTime = NULL,
	     EstimateResponseTime = NULL,
	     Resolutiontime = NULL,
	     ResponseTimeMetSLA = NULL,
	     EstimatedResponseTimeMetSLA = NULL,
	     ResolutionTimeMetSLA = NULL,
	     ResponseCalcX = NULL,
	     ResponseCalcY = NULL,
	     ResolutionCalcX = NULL,
	     ResolutionCalcY = NULL
   WHERE ClientId = @ClientId
     AND JobID = @JobId;	   
	 
/*******Setting ResponseCalcX deriving Column values in a string.***********************************/    
  SELECT @ResponseCalcXcols = STUFF((SELECT ',' + IFiledColValue
									   FROM [dbo].[SlaCalSetup]
									  WHERE ClientId = @ClientId
									    AND InputFiled = 'ResponseCalcX'
									  ORDER BY IPriority
										FOR xml PATH (''), TYPE)
										.value('.', 'NVARCHAR(1000)')
									, 1, 1, '');
/*******Setting ResponseCalcY deriving Column values in a string.***********************************/  								
  SELECT @ResponseCalcYcols = STUFF((SELECT ',' + IFiledColValue
									   FROM [dbo].[SlaCalSetup]
									  WHERE ClientId = @ClientId
									    AND InputFiled = 'ResponseCalcY'
									  ORDER BY IPriority
										FOR xml PATH (''), TYPE)
										.value('.', 'NVARCHAR(1000)')
									, 1, 1, '');
/*******Setting ResolutionCalcX deriving Column values in a string.*********************************/  
  SELECT @ResolutionCalcXcols = STUFF((SELECT ',' + IFiledColValue
									   FROM [dbo].[SlaCalSetup]
									  WHERE ClientId = @ClientId
									    AND InputFiled = 'ResolutionCalcX'
									  ORDER BY IPriority
										FOR xml PATH (''), TYPE)
										.value('.', 'NVARCHAR(1000)')
									, 1, 1, '');
/*******Setting ResolutionCalcY deriving Column values in a string.*********************************/ 									
  SELECT @ResolutionCalcYcols = STUFF((SELECT ',' + IFiledColValue
									   FROM [dbo].[SlaCalSetup]
									  WHERE ClientId = @ClientId
									    AND InputFiled = 'ResolutionCalcY'
									  ORDER BY IPriority
										FOR xml PATH (''), TYPE)
										.value('.', 'NVARCHAR(1000)')
									, 1, 1, '');
/****************Deriving 1.Resptargettime Column value*********************************************/ 
    UPDATE Incident
       SET Resptargettime = CAST(m.metrictarget AS nvarchar(256))
      FROM Incident s
      JOIN metricspecification m
        ON s.Priority = m.Priority
       AND m.ClientId = @ClientId
     WHERE m.MetricId = 172
	   AND s.ClientId = @ClientId 
	   AND s.JobID = @JobId;
	   
/****************Deriving 2.EstResptarget Column value**********************************************/ 
    UPDATE Incident
       SET EstResptarget = CAST(m.metrictarget AS nvarchar(256))
      FROM Incident s
      JOIN metricspecification m
        ON s.Priority = m.Priority
       AND m.ClientId = @ClientId
     WHERE m.MetricId = 173
	   AND s.ClientId = @ClientId 
	   AND s.JobID = @JobId;
	   
/****************Deriving 3.Reslntargettime Column value********************************************/ 
    UPDATE Incident
       SET Reslntargettime = CAST(m.metrictarget AS nvarchar(256))
      FROM Incident s
      JOIN metricspecification m
        ON s.Priority = m.Priority
       AND m.ClientId = @ClientId
     WHERE m.MetricId = 181
       AND s.ClientId = @ClientId 
	   AND s.JobID = @JobId;

/******************************Setting ResponseCalcX values.****************************************/ 
    SET @ResponseCalcXquery = ' UPDATE [dbo].[Incident] SET ResponseCalcX = CONVERT(DATETIME2,a.ResponcecalcX) FROM ( SELECT id as id1,COALESCE (' + @ResponseCalcXcols + ') AS ResponcecalcX FROM
[dbo].[Incident] where ClientId = ' + CAST(@ClientId as nvarchar(20)) + ' AND  JobID =  ' + CAST(@JobId as nvarchar(20)) + ') a where a.id1 = id AND ClientId =  ' + CAST(@ClientId as nvarchar(20)) + ' AND  JobID =  ' + CAST(@JobId as nvarchar(20))
    EXECUTE (@ResponseCalcXquery)
	
/******************************Setting ResponseCalcY values.***************************************/ 
    SET @ResponseCalcYquery = ' UPDATE [dbo].[Incident] SET ResponseCalcY = CONVERT(DATETIME2,a.ResponcecalcY) FROM ( SELECT id as id1,COALESCE (' + @ResponseCalcYcols + ') AS ResponcecalcY FROM
[dbo].[Incident] where ClientId = ' + CAST(@ClientId as nvarchar(20)) + ' AND  JobID =  ' + CAST(@JobId as nvarchar(20)) + ') a where a.id1 = id AND ClientId =  ' + CAST(@ClientId as nvarchar(20)) + ' AND  JobID =  ' + CAST(@JobId as nvarchar(20))
    EXECUTE (@ResponseCalcYquery)

/******************************Setting ResolutionCalcX values.*************************************/ 
    SET @ResolutionCalcXquery = ' UPDATE [dbo].[Incident] SET ResolutionCalcX = CONVERT(DATETIME2,a.ResolutionCalcX) FROM ( SELECT id as id1, COALESCE (' + @ResolutionCalcXcols + ') AS ResolutionCalcX FROM
[dbo].[Incident] where ClientId = ' + CAST(@ClientId as nvarchar(20)) + ' AND  JobID =  ' + CAST(@JobId as nvarchar(20)) + ') a where a.id1 = id AND ClientId =  ' + CAST(@ClientId as nvarchar(20)) + ' AND  JobID =  ' + CAST(@JobId as nvarchar(20))
    EXECUTE (@ResolutionCalcXquery)
	
/******************************Setting ResolutionCalcY values.*************************************/ 
    SET @ResolutionCalcYquery = ' UPDATE [dbo].[Incident] SET ResolutionCalcY = CONVERT(DATETIME2,a.ResolutionCalcY) FROM ( SELECT id as id1, COALESCE (' + @ResolutionCalcYcols + ') AS ResolutionCalcY FROM
[dbo].[Incident] where ClientId = ' + CAST(@ClientId as nvarchar(20)) + ' AND  JobID =  ' + CAST(@JobId as nvarchar(20)) + ') a where a.id1 = id AND ClientId =  ' + CAST(@ClientId as nvarchar(20)) + ' AND  JobID =  ' + CAST(@JobId as nvarchar(20))
    EXECUTE (@ResolutionCalcYquery);
	
/****************Deriving 4.Responsetime Column Value Based on Business Clock **********************/ 	
	UPDATE Incident
       SET Responsetime = s.Responsetime
      FROM (SELECT IIF(DATEDIFF(DAY, i.ResponseCalcX, i.ResponseCalcY) != 0, (IIF((CAST(i.ResponseCalcX AS date) NOT IN (SELECT
				   calendardate
				FROM temp_servicecalendar
				WHERE holidayindicator = 1
				OR weekendindicator = 1)
				), (SELECT
				  CASE
					WHEN (CAST(i.ResponseCalcX AS time) < RESPX.Servicestarttime) THEN CAST(RESPX.workinghours AS nvarchar(max))
					WHEN (CAST(i.ResponseCalcX AS time) > RESPX.Serviceendtime) THEN '0'
					ELSE DATEDIFF(MINUTE,
					  CAST(i.ResponseCalcX AS time), RESPX.Serviceendtime)
				  END)
				, '0') + IIF((CAST(i.ResponseCalcY AS date) NOT IN (SELECT
				  calendardate
				FROM temp_servicecalendar
				WHERE holidayindicator = 1
				OR weekendindicator = 1)
				), (SELECT
				  CASE
					WHEN (CAST(i.ResponseCalcY AS time) < RESPY.Servicestarttime) THEN '0'
					WHEN (CAST(i.ResponseCalcY AS time) > RESPY.Serviceendtime) THEN CAST(RESPY.workinghours AS nvarchar(max))
					ELSE DATEDIFF(MINUTE,
					  RESPY.Servicestarttime, CAST(i.ResponseCalcY AS time))
				  END)
				, '0') + (SELECT (SELECT
					ISNULL(SUM(workinghours), 0)
				  FROM temp_serviceCalendar t1
				  WHERE t1.clientid = i.Clientid
				  AND t1.Calendardate < i.ResponseCalcY - 1
				  AND t1.Calendardate > i.ResponseCalcX))
				), (IIF((CAST(i.ResponseCalcY AS date) NOT IN (SELECT
				  calendardate
				FROM temp_servicecalendar
				WHERE holidayindicator = 1
				OR weekendindicator = 1)
				),
				(CASE
				  WHEN (CAST(i.ResponseCalcX AS time) < RESPX.Servicestarttime) THEN (CASE
					  WHEN CAST(i.ResponseCalcY AS time)
						< RESPY.Servicestarttime THEN '0'
					  WHEN CAST(i.ResponseCalcY AS time) > RESPY.Serviceendtime THEN CAST(RESPY.workinghours AS nvarchar(max))
					  ELSE DATEDIFF(MINUTE, RESPY.Servicestarttime, CAST(ResponseCalcY AS time))
					END)
				  WHEN (CAST(i.ResponseCalcX AS time)
					> RESPX.Servicestarttime AND
					CAST(i.ResponseCalcX AS time) < RESPX.Serviceendtime) THEN (CASE
					  WHEN CAST(i.ResponseCalcY AS time) > RESPY.Serviceendtime THEN DATEDIFF(MINUTE, CAST(i.ResponseCalcX AS time), RESPX.Serviceendtime)
					  ELSE DATEDIFF(MINUTE, i.ResponseCalcX, i.ResponseCalcY)
					END)
				  ELSE '0'
				END), '0'))) AS responsetime, id AS id1
			  FROM Incident AS i LEFT JOIN temp_servicecalendar AS RESPX ON RESPX.calendardate = CAST(i.ResponseCalcX AS date) AND RESPX.ClientId = @ClientId
                     			     LEFT JOIN temp_servicecalendar AS RESPY ON RESPY.calendardate = CAST(i.ResponseCalcY AS date) AND RESPX.ClientId = @ClientId
			  WHERE i.ResponseCalcX IS NOT NULL
			    AND i.ResponseCalcY IS NOT NULL
				AND i.ClientId = @ClientId
                AND i.JobID = @JobId
			  ) s , metricspecification as m
      WHERE Incident.ClientId = @ClientId
        AND Incident.JobID = @JobId
		AND Incident.ResponseCalcX IS NOT NULL
	    AND Incident.ResponseCalcY IS NOT NULL
		AND s.id1 = Incident.id
		AND m.priority =Incident.priority
		AND m.Clocksetup = 1
		AND m.ClientId = @ClientId;

/****************Deriving 4.Responsetime Column Value Based on Current Clock **********************/ 		
	UPDATE Incident
	   SET Responsetime   = DATEDIFF(DAY, ResponseCalcX, ResponseCalcY)  
      FROM metricspecification as m	   
     WHERE Incident.ClientId = @ClientId
       AND Incident.JobID = @JobId
	   AND Incident.ResponseCalcX IS NOT NULL
	   AND Incident.ResponseCalcY IS NOT NULL
	   AND m.priority =Incident.priority
	   AND m.Clocksetup = 0
	   AND m.ClientId = @ClientId;

/****************Deriving 5.Resolutiontime Column Value Based on Business Clock ********************/
    UPDATE Incident
	   SET Resolutiontime = s.Resolutiontime
	  FROM (SELECT IIF(DATEDIFF(DAY, i.ResolutionCalcX, i.ResolutionCalcY) != 0, (IIF((CAST(i.ResolutionCalcX AS date) NOT IN (SELECT
			  calendardate FROM temp_servicecalendar WHERE holidayindicator = 1	OR weekendindicator = 1)
			), (SELECT
			  CASE
				WHEN (CAST(i.ResolutionCalcX AS time) < RESOX.Servicestarttime) THEN CAST(RESOX.workinghours AS nvarchar(50))
				WHEN (CAST(i.ResolutionCalcX AS time) > RESOX.Serviceendtime) THEN '0'
				ELSE DATEDIFF(MINUTE,
				  CAST(i.ResolutionCalcX AS time), RESOX.Serviceendtime)
			  END)
			, '0') + IIF((CAST(i.ResolutionCalcY AS date) NOT IN (SELECT
			  calendardate
			FROM temp_servicecalendar
			WHERE holidayindicator = 1
			OR weekendindicator = 1)
			), (SELECT
			  CASE
				WHEN (CAST(i.ResolutionCalcY AS time) < RESOY.Servicestarttime) THEN '0'
				WHEN (CAST(i.ResolutionCalcY AS time) > RESOY.Serviceendtime) THEN CAST(RESOY.workinghours AS nvarchar(50))
				ELSE DATEDIFF(MINUTE,
				  RESOY.Servicestarttime, CAST(i.ResolutionCalcY AS time))
			  END)
			, '0') + (SELECT (SELECT
				ISNULL(SUM(workinghours), 0)
			  FROM temp_serviceCalendar t1
			  WHERE t1.clientid = i.Clientid
			  AND t1.Calendardate < i.ResolutionCalcY - 1
			  AND t1.Calendardate > i.ResolutionCalcX))
			- ISNULL(i.Totalsuspenddurationertoresolution, 0)),
			(IIF((CAST(i.ResolutionCalcX AS date) NOT IN (SELECT
			  calendardate
			FROM temp_servicecalendar
			WHERE holidayindicator = 1
			OR weekendindicator = 1)
			), (CASE
			  WHEN (CAST(i.ResolutionCalcX AS time) < RESOX.Servicestarttime) THEN (CASE
				  WHEN CAST(i.ResolutionCalcY AS time)
					< RESOY.Servicestarttime THEN '0'
				  WHEN CAST(i.ResolutionCalcY AS time) > RESOY.Serviceendtime THEN CAST(RESOY.workinghours AS nvarchar(50))
				  ELSE DATEDIFF(MINUTE, RESOY.Servicestarttime, CAST(i.ResolutionCalcY AS time))
				END)
			  WHEN (CAST(i.ResolutionCalcX AS time)
				> RESOX.Servicestarttime AND
				CAST(i.ResolutionCalcX AS time) < RESOX.Serviceendtime) THEN (CASE
				  WHEN CAST(i.ResolutionCalcY AS time) > RESOY.Serviceendtime THEN DATEDIFF(MINUTE, CAST(i.ResolutionCalcX AS time), RESOX.Serviceendtime)
				  ELSE DATEDIFF(MINUTE, i.ResolutionCalcX, i.ResolutionCalcY)
				END)
			  ELSE '0'
			END), '0'))) AS Resolutiontime, id AS id1
	   FROM Incident AS i LEFT JOIN temp_servicecalendar AS RESOX ON RESOX.calendardate = CAST(i.ResolutionCalcX AS date) AND RESOX.ClientId = @ClientId
							  LEFT JOIN temp_servicecalendar AS RESOY ON RESOY.calendardate = CAST(i.ResolutionCalcY AS date) AND RESOY.ClientId = @ClientId
	  WHERE i.ResolutionCalcX IS NOT NULL
		AND i.ResolutionCalcY IS NOT NULL
		AND i.ClientId = @ClientId
		AND i.JobID = @JobId
		) s , metricspecification as m
	WHERE Incident.ClientId = @ClientId
	  AND Incident.JobID = @JobId
	  AND Incident.ResolutionCalcX IS NOT NULL
	  AND Incident.ResolutionCalcY IS NOT NULL
	  AND s.id1 = Incident.id
	  AND m.priority =Incident.priority
	  AND m.Clocksetup = 1
	  AND m.ClientId = @ClientId;

/****************Deriving 5.Resolutiontime Column Value Based on Current Clock ************************/ 		   
   UPDATE Incident
	  SET Resolutiontime = DATEDIFF(DAY, ResolutionCalcX, ResolutionCalcY)
	 FROM metricspecification as m	   
    WHERE Incident.ClientId = @ClientId
	  AND Incident.JobID = @JobId
	  AND Incident.ResolutionCalcX IS NOT NULL
	  AND Incident.ResolutionCalcY IS NOT NULL
	  AND m.priority =Incident.priority
	  AND m.Clocksetup = 0
	  AND m.ClientId = @ClientId;
		  
/****************Deriving 6.Estimateresponsetime Column Value***************************************/ 
	UPDATE Incident
	   SET Estimateresponsetime = s.Estimateresponsetime
	  FROM (SELECT
			  CASE
				WHEN DATEDIFF(DAY, i.Acknowledgeddatetime, i.Esitmatedresponsedatetime) >= 0 THEN DATEDIFF(MINUTE,
				  i.Acknowledgeddatetime, i.Esitmatedresponsedatetime)
				ELSE DATEDIFF(MINUTE, i.Esitmatedresponsedatetime, i.Acknowledgeddatetime) - (i.Totalsuspenddurationacktoer + ISNULL
				  ((SELECT
					SUM(t1.nonworkinghours)
				  FROM temp_serviceCalendar t1
				  WHERE t1.clientid = i.clientid
				  AND t1.Calendardate BETWEEN i.Esitmatedresponsedatetime AND i.Acknowledgeddatetime
				  AND (t1.Holidayindicator = 1
				  OR t1.Weekendindicator = 1))
				  , 0))
			  END AS Estimateresponsetime, id as id1
			FROM Incident AS i
			WHERE i.ClientId = @ClientId 
			  AND i.JobID = @JobId
			  AND i.Acknowledgeddatetime IS NOT NULL 
			  AND i.Esitmatedresponsedatetime IS NOT NULL) s
	WHERE ClientId = @ClientId 
	  AND JobID = @JobId
	  AND id = s.id1 ;
			 
/****************Deriving 7.ResponseTimeMetSLA Column Value Based on condition**********************/ 	 
   UPDATE Incident
	  SET ResponseTimeMetSLA = IIF((Responsetime - CAST(Resptargettime AS int)) > 0, 'NO', 'YES')       
	WHERE ClientId = @ClientId 
	  AND JobID = @JobId 
	  AND Resptargettime IS NOT NULL
	  AND Responsetime IS NOT NULL;	  

   UPDATE Incident
	  SET ResponseTimeMetSLA = 'NA'       
	WHERE ClientId = @ClientId 
	  AND JobID = @JobId 
	  AND ( Resptargettime IS NULL
	   OR Responsetime IS NULL );	
		
/****************Deriving 8.EstimatedResponseTimeMetSLA Column Value Based on condition************/ 	 
   UPDATE Incident
	  SET EstimatedResponseTimeMetSLA = IIF((Estimateresponsetime - CAST(EstResptarget AS int)) > 0, 'NO', 'YES')       
	WHERE ClientId = @ClientId 
	  AND JobID = @JobId
	  AND Estimateresponsetime IS NOT NULL
	  AND EstResptarget IS NOT NULL;	  

   UPDATE Incident
	  SET EstimatedResponseTimeMetSLA = 'NA'       
	WHERE ClientId = @ClientId 
	  AND JobID = @JobId
	  AND ( Estimateresponsetime IS NULL
	  OR EstResptarget IS NULL );
		
/****************Deriving 9.ResolutionTimeMetSLA Column Value Based on condition********************/ 	 
   UPDATE Incident
	  SET ResolutionTimeMetSLA = IIF((CAST(Resolutiontime AS int) - CAST(Reslntargettime AS int)) > 0, 'NO', 'YES')        
	WHERE ClientId = @ClientId 
	  AND JobID = @JobId 
	  AND Resolutiontime IS NOT NULL
	  AND Reslntargettime IS NOT NULL;	 

   UPDATE Incident
	  SET ResolutionTimeMetSLA = 'NA'       
	WHERE ClientId = @ClientId 
	  AND JobID = @JobId 
	  AND ( Resolutiontime IS NULL
	  OR Reslntargettime IS NULL );			

END;


GO

CREATE PROCEDURE [dbo].[GetTableColumnNames] 
	@tableName varchar(100)
	AS
BEGIN

SELECT  ISC.COLUMN_NAME  as AppField, 
	ISC.DATA_TYPE as  DataType, 
	ISC.CHARACTER_MAXIMUM_LENGTH as Size, 
	case ISC.IS_NULLABLE when 'YES' then 1 else 0 end as isNullable,
	ext.[Description]
FROM INFORMATION_SCHEMA.COLUMNS  ISC

INNER JOIN
(
    select sep.value [Description],sc.name 
    from sys.tables st
    inner join sys.columns sc on st.object_id = sc.object_id
    left join sys.extended_properties sep on st.object_id = sep.major_id
                                         and sc.column_id = sep.minor_id
                                         and sep.name = 'MS_Description'
    where upper(st.name) = upper(@tableName)
) ext on (ext.name= ISC.COLUMN_NAME)
WHERE upper(ISC.COLUMN_NAME)<>'ID'
and  upper(ISC.COLUMN_NAME)<>'CLIENTID'
and  upper(ISC.COLUMN_NAME)<>'ISDELETED'
and upper(ISC.COLUMN_NAME)<>'JOBID'
and  upper(ISC.TABLE_NAME) = upper(@tableName)
ORDER BY ISC.IS_NULLABLE
	END

go

  if COL_LENGTH('AppstoreApplication', 'StageTable') IS NULL
    begin
		 ALTER table AppstoreApplication add  StageTable varchar(50) null
	end

go

go
if COL_LENGTH('AppstoreApplication', 'SSISPackageFolder') IS NULL
    begin
		 ALTER table AppstoreApplication add  SSISPackageFolder varchar(100) null
	end
go
  if COL_LENGTH('AppstoreApplication', 'DataUploadJob') IS NULL
    begin
		 ALTER table AppstoreApplication add  DataUploadJob varchar(50) null
	end
go
if COL_LENGTH('AppstoreApplication', 'DataUploadSharedFolder') IS NULL
    begin
		 ALTER table AppstoreApplication add  DataUploadSharedFolder varchar(100) null
	end
go
	UPDATE AppstoreApplication SET StageTable='Incident_STG',DataUploadJob='SSIS_INSLA_DefaultJob' where ApplicationName like 'SLA%' or  ApplicationName like 'Incident%'
go


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SSIS_AddUpdateDuplicate_INSTG]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE dbo.[SSIS_AddUpdateDuplicate_INSTG]
END

Go

CREATE  PROCEDURE [dbo].[SSIS_AddUpdateDuplicate_INSTG]
AS
BEGIN 

DECLARE @addcount INT  
 SET @addcount =0 
DECLARE @updatecount INT
 SET @updatecount =0  
DECLARE @duplicatecount INT
 SET @duplicatecount =0 
 
 
DECLARE @addcount_Incident INT  
 SET @addcount_Incident =0 
DECLARE @updatecount_Incident INT
 SET @updatecount_Incident =0  
DECLARE @duplicatecount_Incident INT
 SET @duplicatecount_Incident =0 

 
DECLARE @addcount_MR INT  
 SET @addcount_MR  =0 
DECLARE @updatecount_MR  INT
 SET @updatecount_MR  =0  
DECLARE @duplicatecount_MR  INT
 SET @duplicatecount_MR  =0 

 
DECLARE @addcount_Problem INT  
 SET @addcount_Problem  =0 
DECLARE @updatecount_Problem  INT
 SET @updatecount_Problem  =0  
DECLARE @duplicatecount_Problem  INT
 SET @duplicatecount_Problem  =0 
  DECLARE @IgnoredRecords INT
 SET @IgnoredRecords =0 

-- select * from Incident_STG

 SELECT @addcount_Incident = COUNT(distinct TicketId) FROM Incident_STG stg 
  WHERE TicketId  NOT IN (SELECT TicketId FROM Incident I where I.Clientid=stg.Clientid) 
  --AND Type in ('INC', 'SR') 
  AND TicketId IS NOT NULL

  --AND IsNTT = 'NTT'
 --select @addcount_Incident
  
SELECT @updatecount_Incident  =COUNT(DISTINCT A.TicketId ) FROM Incident_STG A
 INNER JOIN dbo.Incident B ON A.TicketId =B.Incidentid and A.Clientid=B.Clientid
 WHERE A.TicketId IS NOT NULL 
 --AND A.IsNTT  = 'NTT'

DECLARE @duplicates int 

CREATE TABLE #temp_IncidentStaging ( incident nvarchar(255), RecordCount int )
INSERT INTO #temp_IncidentStaging ( incident , RecordCount ) 
SELECT TicketId, count (TicketId )  from Incident_STG 
WHERE TicketId IS NOT NULL 
group by TicketId having  count (TicketId )>1


select @duplicates =count(*) from #temp_IncidentStaging 

--select sum(RecordCount) from #temp_IncidentStaging 

 SELECT @IgnoredRecords = COUNT(*) FROM Incident_STG  WHERE TicketId IS NULL 

 SET @addcount = @addcount_Incident + @addcount_MR + @addcount_Problem 
 select  @duplicatecount = (sum(RecordCount) - @duplicates) from #temp_IncidentStaging
 SET @updatecount = @updatecount_Incident + @updatecount_MR + @updatecount_Problem 

select @addcount , @updatecount , @duplicatecount , @IgnoredRecords 


drop table #temp_IncidentStaging

END 


GO

 go

 IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SSIS_InsertIncidentFromSTG]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE dbo.[SSIS_InsertIncidentFromSTG]
END

GO

CREATE PROCEDURE [dbo].[SSIS_InsertIncidentFromSTG]
AS
BEGIN

--identify and mark duplicate records
CREATE TABLE #temp_IncidentStaging ( incident nvarchar(255), RecordCount int )
INSERT INTO #temp_IncidentStaging ( incident , RecordCount ) 
SELECT TicketId, count (TicketId )  from Incident_STG 
WHERE TicketId IS NOT NULL 
group by TicketId having  count (TicketId )>1

UPDATE A set  A.isdeleted=1  from incident_stg A
where A.id not in 
(select top 1  id from Incident_stg stg 
inner join  #temp_IncidentStaging tmpstg on ( tmpstg.incident =stg.TicketId and stg.TicketId=A.TicketId))
and A.TicketId in (select incident from #temp_IncidentStaging) 

delete #temp_IncidentStaging


INSERT INTO [dbo].[Incident]
           ([Clientid]
           ,[Incidentid]
           ,[Recordincluded]
           ,[Createddatetime]
           ,[Status]
           ,[Description]
           ,[Title]
           ,[Priority]
           ,[Impactscope]
           ,[Urgency]
           ,[Type]
           ,[Outagetype]
           ,[Divisionimpacted]
           ,[Businessunitimpacted]
           ,[Servicecategory]
           ,[Servicesubcategory]
           ,[Servicearea]
           ,[Regionimpacted]
           ,[Locationsimpacted]
           ,[Customerdept]
           ,[Customerlastname]
           ,[Customerfirstname]
           ,[Customertitle]
           ,[Customersatisfaction]
           ,[Impacttype]
           ,[Impactdescription]
           ,[Configitemid]
           ,[Configtype]
           ,[Configitemname]
           ,[Environmentimpacted]
           ,[Module]
           ,[Nodename]
           ,[Rootcausetype]
           ,[Rootcausedescription]
           ,[Resolutiontype]
           ,[Resolutiondescription]
           ,[Solutionid]
           ,[Deploymentrelease]
           ,[Relatedchangeids]
           ,[Relatedids]
           ,[Relatedproblemids]
           ,[Helpdesk]
           ,[Assignedtoperson]
           ,[Assignedtosupportgroup]
           ,[Createdbygroup]
           ,[Createdbyperson]
           ,[Createdtimezone]
           ,[Createdtime]
           ,[Createdweekday]
           ,[Createdweeknum]
           ,[Createdmonth]
           ,[Createdyear]
           ,[Acknowlededbygroup]
           ,[Acknowlededbyperson]
           ,[Acknowledgeddatetime]
           ,[Acknowledgedtimezone]
           ,[Acknowledgedtime]
           ,[Acknowledgedweekday]
           ,[Acknowledgedweeknum]
           ,[Acknowledgedmonth]
           ,[Acknowledgedyear]
           ,[Resolvedbygroup]
           ,[Resolvedbyperson]
           ,[Resolvedbyvendor]
           ,[Resolutionefforthours]
           ,[Resolutiondatetime]
           ,[Resolvedtimezone]
          -- ,[Resolutiontime]
           ,[Resolutionweekday]
           ,[Resolutionweeknum]
           ,[Servicerestoredbygroup]
           ,[Servicerestoredbyperson]
           ,[Servicerestoreddatetime]
           ,[Servicerestoredtimezone]
           ,[Servicerestoredtime]
           ,[Servicerestoredweekday]
           ,[Servicerestoredweeknum]
           ,[Servicerestoredmonth]
           ,[Servicerestoredyear]
           ,[Lastupdatedbyperson]
           ,[Lastupdateddatetime]
           ,[Modificationnotes]
           ,[Datasource]
           ,[Datasourcenotes]
           ,[Uploaddate]
         --  ,[Uploadedby]
           ,[Isdeleted]
           ,[Assignedbygroup]
           ,[Assignedbyperson]
           ,[Assigneddatetime]
           ,[Esitmatedresponsetimebygroup]
           ,[Esitmatedresponsedatetime]
           ,[Resolutionyear]
           ,[Resolutionmonth]
           ,[Waivedindicator]
           ,[Esitmatedresolutiondate]
           ,[Totalsuspenddurationassignedtoack]
           ,[Totalsuspenddurationacktoer]
           ,[Totalsuspenddurationertoresolution]
           ,[Resolvedindicator]
           ,[JobID]
           ,[IsNTTGroup]
           ,[SupportGroupType]
    ,[InProgressdatetime] 
	,[Closeddatetime] 
	,[ScheduledstartdateTime] 
	,[ScheduledenddateTime] 
	,[Scheduledchangecount]
	,[Company] 
	,[Site] 
	,[Assigneecompany] 
	,[Assigneeorganization] 
	,[Estimatedworkeffort] 
	,[Actualworkeffort]
	,[ActualstartdateTime]
	,[Recyclecount] 
	,[Statusreason] 
	,[ConfigItemCategory] 
	,[ClosureCategory] 
	,[Severity] 
	,[WorkGroupLevel]
	,[AffectedItemName]
)
SELECT 
	[Clientid]
      ,[TicketId]
      ,[Recordincluded]
      ,[Createddatetime]
      ,[Status]
      ,[Description]
      ,[Title]
      --,[Priority]
	  , CASE [Priority] 
				WHEN 'Critical' then 1 
				when 'High' then 2 
				WHEN 'Medium' THEN 3
				WHEN 'Low' THEN 4 
				WHEN '1' THEN 1
				WHEN '2' THEN 2
				WHEN '3' THEN 3
				WHEN '4' THEN 4
				WHEN 'p1' THEN 1
				WHEN 'p2' THEN 2
				WHEN 'p3' THEN 3
				WHEN 'p4' THEN 4
				when null then 0
				ELSE 0 
				END 
      ,[Impactscope]
      ,[Urgency]
      ,[Type]
      ,[Outagetype]
      ,[Divisionimpacted]
      ,[Businessunitimpacted]
      ,[Servicecategory]
      ,[Servicesubcategory]
      ,[Servicearea]
      ,[Regionimpacted]
      ,[Locationsimpacted]
      ,[Customerdept]
      ,[Customerlastname]
      ,[Customerfirstname]
      ,[Customertitle]
      ,[Customersatisfaction]
      ,[Impacttype]
      ,[Impactdescription]
      ,[Configitemid]
      ,[Configtype]
      ,[Configitemname]
      ,[Environmentimpacted]
      ,[Module]
      ,[Nodename]
      ,[Rootcausetype]
      ,[Rootcausedescription]
      ,[Resolutiontype]
      ,[Resolutiondescription]
      ,[Solutionid]
      ,[Deploymentrelease]
      ,[Relatedchangeids]
      ,[Relatedids]
      ,[Relatedproblemids]
      ,[Helpdesk]
      ,[Assignedtoperson]
      ,[Assignedtosupportgroup]
      ,[Createdbygroup]
      ,[Createdbyperson]
      ,[Createdtimezone]
      --,[Createdtime]
      --,[Createdweekday]
      --,[Createdweeknum]
      --,[Createdmonth]
      --,[Createdyear]
	    ,left(cast([Createddatetime] as time ),8)
	  	,SUBSTRING(DATENAME(DW , [Createddatetime] ), 1,3)
		,datepart(WEEK, [Createddatetime] )
		, SUBSTRING(datename (MM, [Createddatetime]),1,3) 
		, DATEPART (yy, [Createddatetime] )
      ,[Acknowlededbygroup]
      ,[Acknowlededbyperson]
      ,[Acknowledgeddatetime]
      ,[Acknowledgedtimezone]
      --,[Acknowledgedtime]
      --,[Acknowledgedweekday]
      --,[Acknowledgedweeknum]
      --,[Acknowledgedmonth]
      --,[Acknowledgedyear]
		,left(cast([Acknowledgeddatetime] as time ),8)
		,SUBSTRING(DATENAME(DW , [Acknowledgeddatetime] ), 1,3)
		,datepart(WEEK, [Acknowledgeddatetime] )
		,SUBSTRING(datename (MM, [Acknowledgeddatetime]),1,3)
		,DATEPART (yy, [Acknowledgeddatetime] )
      ,[Resolvedbygroup]
      ,[Resolvedbyperson]
      ,[Resolvedbyvendor]
      ,[Resolutionefforthours]
      ,[Resolutiondatetime]
      ,[Resolvedtimezone]
      --,[Resolutiontime]
      --,[Resolutionweekday]
      --,[Resolutionweeknum]
	  --,left(cast([Resolutiondatetime] as time ),8)
	  , SUBSTRING(DATENAME(DW , [Resolutiondatetime] ),1,3)
	  ,datepart(WEEK, [Resolutiondatetime] )
      ,[Servicerestoredbygroup]
      ,[Servicerestoredbyperson]
      ,[Servicerestoreddatetime]
      ,[Servicerestoredtimezone]
      --,[Servicerestoredtime]
      --,[Servicerestoredweekday]
      --,[Servicerestoredweeknum]
      --,[Servicerestoredmonth]
      --,[Servicerestoredyear]
	  	,left(cast([Servicerestoreddatetime] as time ),8)
		,SUBSTRING(DATENAME(DW , [Servicerestoreddatetime] ), 1,3)
		,datepart(WEEK, [Servicerestoreddatetime] )
		,SUBSTRING(datename (MM, [Servicerestoreddatetime]),1,3)
		,DATEPART (yy, [Servicerestoreddatetime] )
      ,[Lastupdatedbyperson]
      ,[Lastupdateddatetime]
      ,[Modificationnotes]
      ,[Datasource]
      ,[Datasourcenotes]
      ,[Uploaddate]
     -- ,[Uploadedby]
      ,0
      ,[Assignedbygroup]
      ,[Assignedbyperson]
      ,[Assigneddatetime]
      ,[Esitmatedresponsetimebygroup]
      ,[Esitmatedresponsedatetime]
      --,[Resolutionyear]
      --,[Resolutionmonth]
	  ,DATEPART (yy, [Resolutiondatetime] )
	  ,SUBSTRING(datename (MM, [Resolutiondatetime]),1,3)
	 
      --,[Waivedindicator]
	  ,case 
			  when ([IsNTTGroup] = 'N') or ([Assigneddatetime] IS null) OR ([Assigneddatetime]='') or ([InProgressdatetime] IS NULL) OR ([InProgressdatetime] ='') OR ([Status] IS NULL) OR ([Status] = '') then 'Y'
			  else 'N'
			  END
      --,[Esitmatedresolutiondate]
	  ,Case ISNULL([Esitmatedresolutiondate], '00:00:00')
					WHEN '' THEN '00:00:00'
				ELSE ISNULL([Esitmatedresolutiondate], '00:00:00')
				END  
      ,[Totalsuspenddurationassignedtoack]
      ,[Totalsuspenddurationacktoer]
      ,[Totalsuspenddurationertoresolution]
      --,[Resolvedindicator]
	  , case 
						when [Status] =  'Closed'   then  'Y'
						when [Status] = 'Pending' and [Statusreason] ='Resolved' then 'Y'
						else 'N'
						END 
      ,[JobID]
      ,[IsNTTGroup]
      ,[SupportGroupType]
,[InProgressdatetime] 
	,[Closeddatetime] 
	,[ScheduledstartdateTime] 
	,[ScheduledenddateTime] 
	--,[Scheduledchangecount]
	, Case ISNULL([Scheduledchangecount], 0 )
					WHEN '' THEN 0
				ELSE ISNULL([Scheduledchangecount], 0 )
				END 
	,[Company] 
	,[Site] 
	,[Assigneecompany] 
	,[Assigneeorganization] 
	,[Estimatedworkeffort] 
	,[Actualworkeffort]
	,[ActualstartdateTime]
	,[Recyclecount] 
	,[Statusreason] 
	,[ConfigItemCategory] 
	,[ClosureCategory] 
	,[Severity] 
	,[WorkGroupLevel] 
	,[AffectedItemName]
  FROM [dbo].[Incident_STG] stg WHERE  
  stg.TicketId  not in (SELECT i.Incidentid from Incident i where i.Clientid=stg.Clientid )
  AND (stg.Isdeleted = 0 or stg.Isdeleted is null)
  AND TicketId<>''
  AND TicketId is not null
 
END


 go



  IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SSIS_UpdateIncidentFromSTG]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE dbo.[SSIS_UpdateIncidentFromSTG]
END

 go

CREATE  PROCEDURE [dbo].[SSIS_UpdateIncidentFromSTG]

AS
BEGIN

----CREATE TABLE #temp_IncidentStaging ( incident nvarchar(255), RecordCount int )
----INSERT INTO #temp_IncidentStaging ( incident , RecordCount ) 
----SELECT IncidentID, ROW_NUMBER() OVER(PARTITION BY IncidentID ORDER  BY  IncidentID) FROM Incident_STG where IncidentID is not null  and (Isdeleted=0 or Isdeleted is null)

Update  A
    SET    A.[Incidentid] =B.TicketId 
           ,A.[Recordincluded] = B.Recordincluded 
           ,A.[Status] =B.Status
           ,A.[Description] =B.Description 
           ,A.[Title] =B.Title 
          -- ,A.[Priority]= isnull(B.Priority,0) 
		  ,A.[Priority] =  CASE B.[Priority] 
				WHEN 'Critical' then 1 
				when 'High' then 2 
				WHEN 'Medium' THEN 3
				WHEN 'Low' THEN 4 
				WHEN '1' THEN 1
				WHEN '2' THEN 2
				WHEN '3' THEN 3
				WHEN '4' THEN 4
				when null then 0
				WHEN '' then 0
				ELSE 0 
				END 
           ,A.[Type] =B.Type 
           ,A.Configitemname=B.Configitemname 
           ,A.[Helpdesk] =	B.[Helpdesk]
           ,A.[Assignedtoperson] =B.Assignedtoperson 
           ,A.[Assignedtosupportgroup] = CAST(B.Assignedtosupportgroup AS NVARCHAR(120))
           ,A.[Acknowledgeddatetime] = ISNULL( A.Acknowledgeddatetime  ,B.Acknowledgeddatetime )
           ,A.[Acknowledgedweeknum] = datepart(WEEK, ISNULL( A.Acknowledgeddatetime  ,B.Acknowledgeddatetime ) )
           ,A.[Acknowledgedmonth] = SUBSTRING(DATENAME(MM , ISNULL( A.Acknowledgeddatetime  ,B.Acknowledgeddatetime ) ), 1,3) 
           ,A.[Acknowledgedyear] = DATENAME (YY, ISNULL( A.Acknowledgeddatetime  ,B.Acknowledgeddatetime ))
		   ,A.[Acknowledgedweekday] = SUBSTRING(DATENAME(DW , ISNULL( A.Acknowledgeddatetime  ,B.Acknowledgeddatetime ) ), 1,3)
		   ,A.[Acknowledgedtime] = left(cast(ISNULL( A.Acknowledgeddatetime  ,B.Acknowledgeddatetime ) as time ),8)
		   ,A.Acknowlededbygroup= B.Assignedtosupportgroup
           ,A.[Resolvedbygroup] = B.Resolvedbygroup									---isnull(B.Resolvedbygroup  ,B.Assignedtosupportgroup)
           ,A.[Resolvedbyvendor] = B.Resolvedbyvendor								---- isnull(B.Resolvedbyvendor  ,B.Assignedtosupportgroup)
           ,A.[Resolutiondatetime] = B.Resolutiondatetime 
           ,A.[Resolutionweekday] = SUBSTRING(DATENAME(DW , b.Resolutiondatetime ), 1,3)
		   ,A.Resolutiondescription= B.Resolutiondescription
           ,A.[Resolutionweeknum] =datepart(WEEK, B.Resolutiondatetime )
		   --,A.[Resolutiontime] = left(cast(B.Resolutiondatetime  as time ),8)
           --,A.[Isdeleted] =B.Isdeleted 
           ,A.[Assigneddatetime] = B.Assigneddatetime 
		   ---,A.Assignedbygroup= B.Assignedtosupportgroup
           ,A.[Esitmatedresponsetimebygroup] =isnull (B.Esitmatedresponsetimebygroup , '00:00:00')
           ,A.[Esitmatedresponsedatetime] = isnull(B.Esitmatedresponsedatetime , '00:00:00')
           ,A.[Resolutionyear] =datename(YY, b.Resolutiondatetime  ) 
           ,A.[Resolutionmonth] =SUBSTRING(DATENAME(MM ,B.Resolutiondatetime ), 1,3) 
           ,A.[Waivedindicator] =   case 
				when (B.IsNTTGroup = 'N') or (B.Assigneddatetime IS null) OR (B.Assigneddatetime ='') or (B.InProgressdatetime  IS NULL) OR (B.InProgressdatetime  ='') OR (B.Status  IS NULL) OR (B.Status  = '') then 'Y'
			  else 'N'
			  END   ---B.Waivedindicator 
           ,A.[Esitmatedresolutiondate] =ISNULL(B.Esitmatedresolutiondate , '00:00:00')
           ,A.[Totalsuspenddurationassignedtoack] =isnull(B.Totalsuspenddurationassignedtoack ,0)
           ,A.[Totalsuspenddurationacktoer] =isnull(B.Totalsuspenddurationacktoer ,0)
           ,A.[Totalsuspenddurationertoresolution] =isnull(B.Totalsuspenddurationertoresolution ,0)
           ,A.[Resolvedindicator] = case 
						when  B.Resolvedindicator is not null and B.Resolvedindicator<>'' then B.Resolvedindicator
						when B.Status =  'Closed'   then  'Y'
						when B.Status = 'Pending' and B.Statusreason ='Resolved' then 'Y'
						else 'N'
						END 
		   --,A.Servicecategory= B.Servicecategory
		   --,A.Rootcausetype = B.Rootcausetype 
		   ,A.Lastupdatedbyperson= B.Lastupdatedbyperson
		   ,A.Lastupdateddatetime= B.Lastupdateddatetime  ----ISNULL(B.Lastupdateddatetime , B.Resolutiondatetime)
		   ,a.Rootcausedescription = B.Rootcausedescription
		   ,A.InProgressdatetime =B.InProgressdatetime  
		   ,A.Closeddatetime =B.Closeddatetime 
		   ,A.ScheduledstartdateTime =B.ScheduledstartdateTime 
		   ,A.ScheduledenddateTime =B.ScheduledenddateTime 
		   ,A.Scheduledchangecount =B.Scheduledchangecount 
		   ,A.Company =B.Company 
		   ,A.Site =B.Site 
		   ,A.Assigneecompany =B.Assigneecompany 
		   ,A.Assigneeorganization =B.Assigneeorganization 
		   ,A.Estimatedworkeffort =B.Estimatedworkeffort 
		   ,A.Actualworkeffort =B.Actualworkeffort 
		   ,ActualstartdateTime =B.ActualstartdateTime 
		   ,A.Recyclecount =B.Recyclecount 
		   ,A.SupportGroupType = case 
			  when B.IsNTTGroup = 'Y' THEN 'NTT'
			  ELSE ' '
			  END 
	 
	,A.[Statusreason]=B.[Statusreason]
	,A.JobID=B.[JobID]
	,A.[Severity] =B.[Severity]
	,A.[WorkGroupLevel]=B.[WorkGroupLevel]
	,A.[AffectedItemName]=B.[AffectedItemName]

FROM [dbo].[Incident] A
INNER JOIN ( SELECT Incident_stg.* , ROW_NUMBER() over (partition by TicketId ORDER by TicketId) AS reccount
 FROM Incident_STG  where TicketId is not null  and (Isdeleted=0 or Isdeleted is null)
 ) B 
 ON (CAST( A.IncidentId AS NVARCHAR(40)) =B.TicketId  and A.Clientid=B.Clientid and (B.Isdeleted=0 or B.Isdeleted is null))
--join #temp_IncidentStaging c on c.incident =a.IncidentId 
--where b.reccount =(select  max(RecordCount ) from #temp_IncidentStaging where incident =b.IncidentID )


--DROP TABLE #temp_IncidentStaging 

END




Go

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetRejectedRecords]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE dbo.[GetRejectedRecords]
END
Go
---Getting rejected record in rowwise
CREATE Procedure GetRejectedRecords(@JobId as int)
AS
Begin

DECLARE @columns VARCHAR(1000)     
DECLARE @querydesc VARCHAR(6000)      

SELECT @columns = COALESCE(@columns + ',[' + [SourceDataField]+ ']' ,      
                          + '[' + [SourceDataField] +']')        
FROM(      
  Select distinct SourceDataField    
  From SourceRejectedRecord where jobid=@JobId
) x      

--print @columns;
     
SET @querydesc = 'SELECT *  
FROM (
    SELECT SourceDataField,SourceDataFieldVal,Clientid,JobId,Rejectid,RejectReason
    FROM SourceRejectedRecord where JobId=' + CAST(@JobId AS varchar(10))  +'
) as s PIVOT
(
   max( [SourceDataFieldVal])
 FOR [SourceDataField]  in  (' + @columns + ')) AS pvt'

	--print @querydesc;
exec (@querydesc)


End;

GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SSIS_CreateSQLJobsForClients]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE dbo.[SSIS_CreateSQLJobsForClients]
END

GO

CREATE PROCEDURE [dbo].[SSIS_CreateSQLJobsForClients](@SSISPackagePath nvarchar(400), @ClientName varchar(50), @Appid varchar(10), @ScheduleType varchar(20), @scheduleon varchar(20), @ScheduleTime int)
AS
BEGIN

BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0

IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
declare @jobname varchar(50)
set @jobname ='SSIS__Client_'+ltrim(str(DB_ID()))+'_'+@ClientName+'_'+@Appid+'_'+@ScheduleType+'_Job'

declare @freq int
declare @freqInterval int

select @freq= case @ScheduleType 
				when 'DAILY' then 4
				when 'WEEKLY' then 8
				when 'MONTHLY' then 16
				end
select @freqInterval = case @scheduleon 
						when 'SUN' then 1 
						when 'MON' then 2
						when 'TUE' then 4
						when 'WED' then 8
						when 'THU' then 16
						when 'FRI' then 32
						when 'SAT' then 64
						when '' then 1
						else cast(@scheduleon as int)
						end
declare @date int
select @date= cast(cast(year(getdate()) as nvarchar) +
right('00' + cast(month(getdate()) as nvarchar),2) + 
right('00' + cast(day(getdate()) as nvarchar),2) as int)

declare @desc varchar(100)
select @desc= 'SSIS job for '+ @ClientName +' to scheduled ' + @ScheduleType + 'on' + @scheduleon

EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=@jobname, 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=@desc,
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [SSIS_Package]    Script Date: 11/10/2017 8:11:19 AM ******/
declare @execCommand nvarchar(300)
set @execCommand=N'/FILE "\"'+@SSISPackagePath+'INSLA_dataload.dtsx\"" /CONFIGFILE "\"' +@SSISPackagePath+'INSLA_dataload.dtsConfig\"" /X86  /CHECKPOINTING OFF /SET "\"\Package.Variables[User::var_FilePattern].Properties[Value]\"";' + @ClientName+'_'+ @Appid+'*.xlsx' + '/REPORTING E'
--set @execCommand='"\"InsuranceClient_3_43*.xlsx\""'
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SSIS_Package', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=@execCommand,
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
declare @schedulename varchar(50)
set @schedulename=@ScheduleType+'-'+@scheduleon
declare @newguid varchar(100)
set @newguid=cast(newid() as varchar(100))
declare @RefFactor int
select @RefFactor = case @ScheduleType 
					when 'DAILY' then 0
					else 1
					end
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=@schedulename, 
		@enabled=1, 
		@freq_type=@freq, 
		@freq_interval=@freqInterval, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=@RefFactor, 
		@active_start_date=@date, 
		@active_end_date=99991231, 
		@active_start_time=@ScheduleTime, 
		@active_end_time=235959, 
		@schedule_uid=  @newguid
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

Select '1'
END;



GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SSIS_GetSQLJobs]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE dbo.[SSIS_GetSQLJobs]
END

GO


CREATE PROCEDURE [dbo].[SSIS_GetSQLJobs](@clientname varchar(50), @AppId varchar(10))
AS
BEGIN
--use [msdb]
declare @hasJobName varchar(100)
--declare @clientname varchar(50)
--declare @AppId int
--set @AppId=null
--set @clientname='jhuhu'
if @AppId is null OR @AppId=0
begin
	set @AppId=''
end
set @hasJobName=ltrim(str(DB_ID()))+'_'+@clientname + '_' + cast(@AppId as varchar(10))
--select @hasJobName


Declare @weekDay Table
(
mask int
, maskValue varchar(32)
);

Insert Into @weekDay
Select 1, 'Sunday' UNION ALL
Select 2, 'Monday' UNION ALL
Select 4, 'Tuesday' UNION ALL
Select 8, 'Wednesday' UNION ALL
Select 16, 'Thursday' UNION ALL
Select 32, 'Friday' UNION ALL
Select 64, 'Saturday';


With SCHED as (
Select sched.name As 'scheduleName'
, sched.schedule_id
, jobsched.job_id as job_id
, Case
When sched.freq_type = 1
Then 'Once'
When sched.freq_type = 4 And sched.freq_interval = 1
Then 'Daily'
When sched.freq_type = 4
Then 'Every ' + Cast(sched.freq_interval As varchar(5)) + ' days'
When sched.freq_type = 8
Then Replace( Replace( Replace((
Select maskValue
From @weekDay As x
Where sched.freq_interval & x.mask <> 0
Order By mask For XML Raw)
, '"/><row maskValue="', ', '), '<row maskValue="', ''), '"/>', '')
+ Case When sched.freq_recurrence_factor <> 0
And sched.freq_recurrence_factor = 1
Then '; weekly'
When sched.freq_recurrence_factor <> 0
Then '; every '
+ Cast(sched.freq_recurrence_factor As varchar(10)) + ' weeks'
End
When sched.freq_type = 16
Then 'On day '
+ Cast(sched.freq_interval As varchar(10)) + ' of every '
+ Cast(sched.freq_recurrence_factor As varchar(10)) + ' months'
When sched.freq_type = 32
Then Case
When sched.freq_relative_interval = 1
Then 'First'
When sched.freq_relative_interval = 2
Then 'Second'
When sched.freq_relative_interval = 4
Then 'Third'
When sched.freq_relative_interval = 8
Then 'Fourth'
When sched.freq_relative_interval = 16
Then 'Last'
End +
Case
When sched.freq_interval = 1
Then ' Sunday'
When sched.freq_interval = 2
Then ' Monday'
When sched.freq_interval = 3
Then ' Tuesday'
When sched.freq_interval = 4
Then ' Wednesday'
When sched.freq_interval = 5
Then ' Thursday'
When sched.freq_interval = 6
Then ' Friday'
When sched.freq_interval = 7
Then ' Saturday'
When sched.freq_interval = 8
Then ' Day'
When sched.freq_interval = 9
Then ' Weekday'
When sched.freq_interval = 10
Then ' Weekend'
End
+
Case
When sched.freq_recurrence_factor <> 0
And sched.freq_recurrence_factor = 1
Then '; monthly'
When sched.freq_recurrence_factor <> 0
Then '; every '
+ Cast(sched.freq_recurrence_factor As varchar(10)) + ' months'
End
When sched.freq_type = 64
Then 'StartUp'
When sched.freq_type = 128
Then 'Idle'
End As 'frequency'
, IsNull('Every ' + Cast(sched.freq_subday_interval As varchar(10)) +
Case
When sched.freq_subday_type = 2
Then ' seconds'
When sched.freq_subday_type = 4
Then ' minutes'
When sched.freq_subday_type = 8
Then ' hours'
End, 'Once') As 'subFrequency'

,[Start_time] =
CASE LEN(sched.active_start_time)
WHEN 1 THEN CAST('00:00:0' + RIGHT(sched.active_start_time, 2) AS CHAR(8))
WHEN 2 THEN CAST('00:00:' + RIGHT(sched.active_start_time, 2) AS CHAR(8))
WHEN 3 THEN CAST('00:0'
+ LEFT(RIGHT(sched.active_start_time, 3), 1)
+ ':' + RIGHT(sched.active_start_time, 2) AS CHAR(8))
WHEN 4 THEN CAST('00:'
+ LEFT(RIGHT(sched.active_start_time, 4), 2)
+ ':' + RIGHT(sched.active_start_time, 2) AS CHAR(8))
WHEN 5 THEN CAST('0'
+ LEFT(RIGHT(sched.active_start_time, 5), 1)
+ ':' + LEFT(RIGHT(sched.active_start_time, 4), 2)
+ ':' + RIGHT(sched.active_start_time, 2) AS CHAR(8))
WHEN 6 THEN CAST(LEFT(RIGHT(sched.active_start_time, 6), 2)
+ ':' + LEFT(RIGHT(sched.active_start_time, 4), 2)
+ ':' + RIGHT(sched.active_start_time, 2) AS CHAR(8))
END
,[End_time] =
CASE LEN(sched.active_end_time)
WHEN 1 THEN CAST('00:00:0' + RIGHT(sched.active_end_time, 2) AS CHAR(8))
WHEN 2 THEN CAST('00:00:' + RIGHT(sched.active_end_time, 2) AS CHAR(8))
WHEN 3 THEN CAST('00:0'
+ LEFT(RIGHT(sched.active_end_time, 3), 1)
+ ':' + RIGHT(sched.active_end_time, 2) AS CHAR(8))
WHEN 4 THEN CAST('00:'
+ LEFT(RIGHT(sched.active_end_time, 4), 2)
+ ':' + RIGHT(sched.active_end_time, 2) AS CHAR(8))
WHEN 5 THEN CAST('0'
+ LEFT(RIGHT(sched.active_end_time, 5), 1)
+ ':' + LEFT(RIGHT(sched.active_end_time, 4), 2)
+ ':' + RIGHT(sched.active_end_time, 2) AS CHAR(8))
WHEN 6 THEN CAST(LEFT(RIGHT(sched.active_end_time, 6), 2)
+ ':' + LEFT(RIGHT(sched.active_end_time, 4), 2)
+ ':' + RIGHT(sched.active_end_time, 2) AS CHAR(8))
END
, Replicate('0', 6 - Len(jobsched.next_run_time))
+ Cast(jobsched.next_run_time As varchar(6)) As 'nextRunTime'
, Cast(jobsched.next_run_date As char(8)) As 'nextRunDate'
From msdb.dbo.sysschedules As sched
Join msdb.dbo.sysjobschedules As jobsched
On sched.schedule_id = jobsched.schedule_id),
JOB as (
SELECT
[job_id] = job.job_id
,[Job_Name] = job.Name
,[Job_Description] = job.[description]
,[Job_Enabled] =
CASE job.Enabled
WHEN 1 THEN 'Yes'
WHEN 0 THEN 'No'
END
,[Sched_ID] = sched.schedule_id
,[Sched_Enabled] =
CASE sched.enabled
WHEN 1 THEN 'Yes'
WHEN 0 THEN 'No'
END
,[Sched_Frequency] =
CASE sched.freq_type
WHEN 1 THEN 'Once'
WHEN 4 THEN 'Daily'
WHEN 8 THEN 'Weekly'
WHEN 16 THEN 'Monthly'
WHEN 32 THEN 'Monthly relative'
WHEN 64 THEN 'When SQLServer Agent starts'
END
,[Next_Run_Date] =
CASE next_run_date
WHEN 0 THEN NULL
ELSE SUBSTRING(CONVERT(VARCHAR(15), next_run_date), 1, 4) + '/' +
SUBSTRING(CONVERT(VARCHAR(15), next_run_date), 5, 2) + '/' +
SUBSTRING(CONVERT(VARCHAR(15), next_run_date), 7, 2)
END
,[Next_Run_Time] =
CASE LEN(next_run_time)
WHEN 1 THEN CAST('00:00:0' + RIGHT(next_run_time, 2) AS CHAR(8))
WHEN 2 THEN CAST('00:00:' + RIGHT(next_run_time, 2) AS CHAR(8))
WHEN 3 THEN CAST('00:0'
+ LEFT(RIGHT(next_run_time, 3), 1)
+ ':' + RIGHT(next_run_time, 2) AS CHAR(8))
WHEN 4 THEN CAST('00:'
+ LEFT(RIGHT(next_run_time, 4), 2)
+ ':' + RIGHT(next_run_time, 2) AS CHAR(8))
WHEN 5 THEN CAST('0'
+ LEFT(RIGHT(next_run_time, 5), 1)
+ ':' + LEFT(RIGHT(next_run_time, 4), 2)
+ ':' + RIGHT(next_run_time, 2) AS CHAR(8))
WHEN 6 THEN CAST(LEFT(RIGHT(next_run_time, 6), 2)
+ ':' + LEFT(RIGHT(next_run_time, 4), 2)
+ ':' + RIGHT(next_run_time, 2) AS CHAR(8))
END
,[Max_Duration] =
CASE LEN(max_run_duration)
WHEN 1 THEN CAST('00:00:0'
+ CAST(max_run_duration AS CHAR) AS CHAR(8))
WHEN 2 THEN CAST('00:00:'
+ CAST(max_run_duration AS CHAR) AS CHAR(8))
WHEN 3 THEN CAST('00:0'
+ LEFT(RIGHT(max_run_duration, 3), 1)
+ ':' + RIGHT(max_run_duration, 2) AS CHAR(8))
WHEN 4 THEN CAST('00:'
+ LEFT(RIGHT(max_run_duration, 4), 2)
+ ':' + RIGHT(max_run_duration, 2) AS CHAR(8))
WHEN 5 THEN CAST('0'
+ LEFT(RIGHT(max_run_duration, 5), 1)
+ ':' + LEFT(RIGHT(max_run_duration, 4), 2)
+ ':' + RIGHT(max_run_duration, 2) AS CHAR(8))
WHEN 6 THEN CAST(LEFT(RIGHT(max_run_duration, 6), 2)
+ ':' + LEFT(RIGHT(max_run_duration, 4), 2)
+ ':' + RIGHT(max_run_duration, 2) AS CHAR(8))
END
,[Min_Duration] =
CASE LEN(min_run_duration)
WHEN 1 THEN CAST('00:00:0'
+ CAST(min_run_duration AS CHAR) AS CHAR(8))
WHEN 2 THEN CAST('00:00:'
+ CAST(min_run_duration AS CHAR) AS CHAR(8))
WHEN 3 THEN CAST('00:0'
+ LEFT(RIGHT(min_run_duration, 3), 1)
+ ':' + RIGHT(min_run_duration, 2) AS CHAR(8))
WHEN 4 THEN CAST('00:'
+ LEFT(RIGHT(min_run_duration, 4), 2)
+ ':' + RIGHT(min_run_duration, 2) AS CHAR(8))
WHEN 5 THEN CAST('0'
+ LEFT(RIGHT(min_run_duration, 5), 1)
+ ':' + LEFT(RIGHT(min_run_duration, 4), 2)
+ ':' + RIGHT(min_run_duration, 2) AS CHAR(8))
WHEN 6 THEN CAST(LEFT(RIGHT(min_run_duration, 6), 2)
+ ':' + LEFT(RIGHT(min_run_duration, 4), 2)
+ ':' + RIGHT(min_run_duration, 2) AS CHAR(8))
END
,[Avg_Duration] =
CASE LEN(avg_run_duration)
WHEN 1 THEN CAST('00:00:0'
+ CAST(avg_run_duration AS CHAR) AS CHAR(8))
WHEN 2 THEN CAST('00:00:'
+ CAST(avg_run_duration AS CHAR) AS CHAR(8))
WHEN 3 THEN CAST('00:0'
+ LEFT(RIGHT(avg_run_duration, 3), 1)
+ ':' + RIGHT(avg_run_duration, 2) AS CHAR(8))
WHEN 4 THEN CAST('00:'
+ LEFT(RIGHT(avg_run_duration, 4), 2)
+ ':' + RIGHT(avg_run_duration, 2) AS CHAR(8))
WHEN 5 THEN CAST('0'
+ LEFT(RIGHT(avg_run_duration, 5), 1)
+ ':' + LEFT(RIGHT(avg_run_duration, 4), 2)
+ ':' + RIGHT(avg_run_duration, 2) AS CHAR(8))
WHEN 6 THEN CAST(LEFT(RIGHT(avg_run_duration, 6), 2)
+ ':' + LEFT(RIGHT(avg_run_duration, 4), 2)
+ ':' + RIGHT(avg_run_duration, 2) AS CHAR(8))
END
,[Subday_Frequency] =
CASE (sched.freq_subday_interval)
WHEN 0 THEN 'Once'
ELSE CAST('Every '
+ RIGHT(sched.freq_subday_interval, 2)
+ ' '
+ CASE (sched.freq_subday_type)
WHEN 1 THEN 'Once'
WHEN 4 THEN 'Minutes'
WHEN 8 THEN 'Hours'
END AS CHAR(16))
END
,[Sched_End Date] = sched.active_end_date
,[Sched_End Time] = sched.active_end_time
,[Fail_Notify_Name] =
CASE
WHEN oper.enabled = 0 THEN 'Disabled: '
ELSE ''
END + oper.name
,[Fail_Notify_Email] = oper.email_address
,server

FROM msdb.dbo.sysjobs job
LEFT JOIN (SELECT

job_schd.job_id
,sys_schd.enabled
,sys_schd.schedule_id
,sys_schd.freq_type
,sys_schd.freq_subday_type
,sys_schd.freq_subday_interval
,next_run_date =
CASE
WHEN job_schd.next_run_date = 0 THEN sys_schd.active_start_date
ELSE job_schd.next_run_date
END
,next_run_time =
CASE
WHEN job_schd.next_run_date = 0 THEN sys_schd.active_start_time
ELSE job_schd.next_run_time
END
,active_end_date = NULLIF(sys_schd.active_end_date, '99991231')
,active_end_time = NULLIF(sys_schd.active_end_time, '235959')

FROM msdb.dbo.sysjobschedules job_schd
LEFT JOIN msdb.dbo.sysschedules sys_schd
ON job_schd.schedule_id = sys_schd.schedule_id) sched
ON job.job_id = sched.job_id
LEFT OUTER JOIN (SELECT
job_id, server
,MAX(job_his.run_duration) AS max_run_duration
,MIN(job_his.run_duration) AS min_run_duration
,AVG(job_his.run_duration) AS avg_run_duration
FROM msdb.dbo.sysjobhistory job_his
GROUP BY job_id, server) Q1
ON job.job_id = Q1.job_id
LEFT JOIN msdb.dbo.sysoperators oper
ON job.notify_email_operator_id = oper.id)

SELECT isnull(b.server,convert(varchar(max),SERVERPROPERTY('ServerName'))), b.job_name, b.Job_Description, b.job_enabled, isnull(b.sched_enabled,'No') as sched_enabled,
isnull(a.scheduleName, 'None') as scheduleName, isnull(a.frequency,'Not scheduled') as frequency,
isnull(a.subFrequency, 'None') as subFrequency, isnull(a.start_time,'-') as start_time, isnull(a.end_Time,'-') as end_time,
isnull(b.Next_Run_Date, '-') + ' '+ isnull(b.Next_Run_Time, '-') as Next_Run_Date,
isnull(b.Max_Duration, '-') as Max_Duration, isnull(b.Min_Duration, '-') as Min_Duration--,
--isnull(b.Avg_Duration, '-') as Avg_Duration, isnull(b.Fail_Notify_Name, 'None') as Fail_Notify_Name,
--isnull(b.Fail_Notify_Email, 'None') as Fail_Notify_Email
FROM SCHED a RIGHT OUTER JOIN JOB b
ON a.job_id = b.job_id
where b.Job_Name like '%'+@hasJobName+'%'
ORDER BY job_name 

END;

----SSIS package default Package

GO
USE [msdb]
GO


/****** Object:  Job [SSIS_INSLA_DefaultJob]    Script Date: 11/10/17 3:16:26 PM ******/
EXEC msdb.dbo.sp_delete_job @job_name=N'SSIS_INSLA_DefaultJob', @delete_unused_schedule=1
GO

/****** Object:  Job [SSIS_INSLA_DefaultJob]    Script Date: 11/7/2017 10:36:42 AM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 11/7/2017 10:36:42 AM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'SSIS_INSLA_DefaultJob', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Default SSIS job for all clients to process incoming data.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [SSIS_Package]    Script Date: 11/7/2017 10:36:42 AM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SSIS_Package', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=N'/FILE "\"C:\NextGenPackage\INSLA_dataload.dtsx\"" /CONFIGFILE "\"C:\NextGenPackage\INSLA_dataload.dtsConfig\"" /X86  /CHECKPOINTING OFF /REPORTING E', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO




GO
--------------------------------------------
-- ENABLING CDC-----------------------------
--EXEC sp_changedbowner 'sa'
--go

--EXEC sys.sp_cdc_enable_db 
--GO

--SELECT [name], is_tracked_by_cdc FROM sys.tables

--EXEC sys.sp_cdc_enable_table 
--@source_schema = N'dbo', 
--@source_name   = N'sourcedatamapping', 
--@role_name     = NULL 
--GO
-------------------------------------------------


CREATE TABLE [dbo].[SlaFieldCol](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IFiledColValue] [varchar](45) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[SlaClockType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](45) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[SlaClockType]
           ([Type])
     VALUES
           ('Clock Hours')
GO
INSERT INTO [dbo].[SlaClockType]
           ([Type])
     VALUES
           ('Business Hours')
GO 

GO
Begin
INSERT INTO [dbo].[SlaFieldCol] ([IFiledColValue]) VALUES ('Created Datetime')
INSERT INTO [dbo].[SlaFieldCol] ([IFiledColValue]) VALUES ('Assigned Datetime')
INSERT INTO [dbo].[SlaFieldCol] ([IFiledColValue]) VALUES ('Acknowledged Datetime')
INSERT INTO [dbo].[SlaFieldCol] ([IFiledColValue]) VALUES ('Resolution Datetime')
End
GO 


/****** Object:  StoredProcedure [dbo].[rptTicketBreakdownByAssignedGroup]    Script Date: 12/11/2017 9:52:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[rptTicketBreakdownByAssignedGroup]
	-- Add the parameters for the stored procedure here
	(@StartDate DateTime,@EndDate DateTime,@WhereClause varchar(max)='',@AgeingNumber int,
	@clientid int)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @qry VARCHAR(MAX)
    -- Insert statements for procedure here
	
begin
if(@AgeingNumber is null)
			SET @qry = '   SELECT        COUNT(ID) AS CountbyPriorityWeekday, [Status], 
                        Createdweeknum, Assignedtosupportgroup,supportgrouptype, CASE [Priority] WHEN ''1'' THEN ''P1'' WHEN ''2'' THEN ''P2'' WHEN ''3'' THEN ''P3'' WHEN ''4'' THEN ''P4'' END AS Priority,Type
FROM            Incident WHERE          Createddatetime >=   '''+ convert(nvarchar(25), @StartDate, 121) + ''''+' and Createddatetime <= '''+ convert(nvarchar(25),cast(convert(char(8), @EndDate, 112) + ' 23:59:59.99' as datetime), 121) + ''' and  '+ @WhereClause+' 
		
			 GROUP BY Createdweeknum,Type,supportgrouptype, Assignedtosupportgroup,[Status],[Priority],Assignedtoperson
ORDER BY Assignedtosupportgroup'

else if(DATEDIFF(day,@StartDate,@EndDate)>@AgeingNumber)

SET @qry = ' SELECT        COUNT(ID) AS CountbyPriorityWeekday, [Status], 
                        Createdweeknum, Assignedtosupportgroup,supportgrouptype, CASE [Priority] WHEN ''1'' THEN ''P1'' WHEN ''2'' THEN ''P2'' WHEN ''3'' THEN ''P3'' WHEN ''4'' THEN ''P4'' END AS Priority
FROM            Incident WHERE          Createddatetime >=   '''+ convert(nvarchar(25),DATEADD(day,@AgeingNumber, @StartDate), 121) + ''''+' and  Createddatetime <='''+ convert(nvarchar(25),cast(convert(char(8), @EndDate, 112) + ' 23:59:59.99' as datetime), 121) + ''' and  '+ @WhereClause+' 
		and Status NOT IN(''Closed'',''Resolved'',''Cancelled'') and clientid in (@clientid)
			 GROUP BY Createdweeknum,supportgrouptype, Assignedtosupportgroup,[Status],[Priority],Assignedtoperson
ORDER BY Assignedtosupportgroup'
			EXEC(@qry)


--			 SELECT        COUNT(ID) AS CountbyPriorityWeekday, [Status], 
--                         Createdweeknum, Assignedtosupportgroup,supportgrouptype, CASE [Priority] WHEN '1' THEN 'P1' WHEN '2' THEN 'P2' WHEN '3' THEN 'P3' WHEN '4' THEN 'P4' END AS Priority
--FROM            Incident
--WHERE          (Createddatetime between  @StartDate and  @EndDate) and waivedindicator = 'N' 
--and assignedtosupportgroup in (@assignedgroup)
--and [Priority] in (@Priority)
--GROUP BY Createdweeknum,supportgrouptype, Assignedtosupportgroup,[Status],[Priority],Assignedtoperson
--ORDER BY Assignedtosupportgroup
--	  end
--	  else
--	  SELECT        COUNT(ID) AS CountbyPriorityWeekday, [Status], 
--                         Createdweeknum, Assignedtosupportgroup,supportgrouptype, CASE [Priority] WHEN '1' THEN 'P1' WHEN '2' THEN 'P2' WHEN '3' THEN 'P3' WHEN '4' THEN 'P4' END AS Priority
--FROM            Incident
--WHERE          (Createddatetime between  DATEADD(day,@AgeingNumber,@StartDate) and  @EndDate) and waivedindicator = 'N' 
----and assignedtosupportgroup in (@assignedgroup)
----and [Priority] in (@Priority)
-- and Status NOT IN('Closed','Resolved')
--GROUP BY Createdweeknum,supportgrouptype, Assignedtosupportgroup,[Status],[Priority],Assignedtoperson
--ORDER BY Assignedtosupportgroup
End
End




GO


