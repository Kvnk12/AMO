

GO

  if COL_LENGTH('UserApplications', 'ClientId') IS NULL
    begin
		 ALTER TABLE [dbo].[UserApplications] ADD [ClientId] INT NULL
	end
go


 if COL_LENGTH('UserProfile', 'ReportingManager') IS NULL
    begin
		 ALTER TABLE [dbo].[UserProfile] ADD [ReportingManager] varchar(50) NULL
	end
go
GO

  if COL_LENGTH('UserApplications', 'RoleId') IS NULL
    begin
		 ALTER TABLE [dbo].[UserApplications] ADD [RoleId] INT NULL
	end
go

ALTER TABLE [dbo].[UserApplications]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[UserRoles] ([Id])

GO

ALTER TABLE [dbo].[UserApplications]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([id])

GO
GO
 if COL_LENGTH('UserRoles', 'Weightage') IS NULL
    begin
		 ALTER TABLE [dbo].[UserRoles] ADD [Weightage] int NOT NULL DEFAULT 0
	end
GO

  CREATE procedure GETUserProfilesForUser @userid int
  AS
  BEGIN

  declare @roleWeight int
    select @roleWeight= max (r.weightage) from UserRoles r 
  inner join UserApplications ua on ua.RoleId=r.Id and ua.UserId=@userid
  --order by r.

  Select distinct up.* from UserProfile up
  INNER JOIN  UserApplications  ua1 on ua1.UserId=up.Id
  INNER join UserApplications ua2 on ua1.ApplicationId=ua2.ApplicationId and ua1.ClientId=ua2.ClientId and ua2.UserId=@userid 
  INNER  join UserRoles ur on ua1.RoleId=ur.Id and ur.Weightage<=@roleWeight
  Union
  Select distinct up.* from UserProfile up 
  where up.id not in (select UserId from UserApplications)

  END