USE [Umbraco_Show]
GO

/****** Object:  Table [dbo].[ATT_ProfileOptionalDataLabel] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATT_ProfileOptionalDataLabel](
	[ProfileOptionalDataLabelId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](1000)  NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL,
	[IsObsolete] [bit] NULL,
 CONSTRAINT [PK_ATT_ProfileOptionalDataLabel] PRIMARY KEY CLUSTERED 
(
	[ProfileOptionalDataLabelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ATT_ProfileOptionalDataLabel] ADD  CONSTRAINT [DF_ATT_ProfileOptionalDataLabel_UpdateSource]  DEFAULT ('Not Specified') FOR [UpdateSource]
GO

ALTER TABLE [dbo].[ATT_ProfileOptionalDataLabel] ADD  CONSTRAINT [DF_ATT_ProfileOptionalDataLabel_IsObsolete]  DEFAULT ((0)) FOR [IsObsolete]
GO


/****** Object:  Table [dbo].[ATT_ProfileRequests]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATT_ProfileRequests](
	[ProfileRequestId] [int] IDENTITY(1,1) NOT NULL,
	[AttendeeId] [int] NULL,
	[EmployeeAttendeeId] [int] NULL,
	[RequestedBy] [nvarchar](250) NOT NULL,
	[ApprovedBy] [nvarchar](250) NULL,
	[Status] [int] NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ATT_ProfileRequests] PRIMARY KEY CLUSTERED 
(
	[ProfileRequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ATT_ProfileRequests] ADD  CONSTRAINT [DF_ATT_ProfileRequests_UpdateSource]  DEFAULT ('Not Specified') FOR [UpdateSource]
GO

ALTER TABLE [dbo].[ATT_ProfileRequests]  WITH CHECK ADD  CONSTRAINT [FK_ATT_ProfileRequests_ATT_Attendee] FOREIGN KEY([AttendeeId])
REFERENCES [dbo].[ATT_Attendee] ([AttendeeId])
GO

ALTER TABLE [dbo].[ATT_ProfileRequests] CHECK CONSTRAINT [FK_ATT_ProfileRequests_ATT_Attendee]
GO

ALTER TABLE [dbo].[ATT_ProfileRequests]  WITH CHECK ADD  CONSTRAINT [FK_ATT_ProfileRequests_ATT_EmployeeAttendee] FOREIGN KEY([EmployeeAttendeeId])
REFERENCES [dbo].[ATT_EmployeeAttendee] ([EmployeeAttendeeId])
GO

ALTER TABLE [dbo].[ATT_ProfileRequests] CHECK CONSTRAINT [FK_ATT_ProfileRequests_ATT_EmployeeAttendee]
GO


/****** Object:  Table [dbo].[ATT_ProfileRequestOptionalDetails]******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATT_ProfileOptionalDetails](
	[ProfileRequestOptionalDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ProfileRequestId] [int] NOT NULL,
	[ProfileOptionalDataLabelId] [int] NOT NULL,
	[UpdateValue] [nvarchar](1000) NULL,
	[OrigValue] [nvarchar](1000) NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ATT_ProfileOptionalDetails] PRIMARY KEY CLUSTERED 
(
	[ProfileOptionalDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ATT_ProfileOptionalDetails] ADD  CONSTRAINT [DF_ATT_ProfileOptionalDetails_UpdateSource]  DEFAULT ('Not Specified') FOR [UpdateSource]
GO

ALTER TABLE [dbo].[ATT_ProfileOptionalDetails]  WITH CHECK ADD  CONSTRAINT [FK_ATT_ProfileOptionalDetails_ATT_ProfileOptionalDataLabel] FOREIGN KEY([ProfileOptionalDataLabelId])
REFERENCES [dbo].[ATT_ProfileOptionalDataLabel] ([ProfileOptionalDataLabelId])
GO

ALTER TABLE [dbo].[ATT_ProfileOptionalDetails] CHECK CONSTRAINT [FK_ATT_ProfileOptionalDetails_ATT_ProfileOptionalDataLabel]
GO

ALTER TABLE [dbo].[ATT_ProfileOptionalDetails]  WITH CHECK ADD  CONSTRAINT [FK_ATT_ProfileOptionalDetails_ATT_ProfileRequests] FOREIGN KEY([ProfileRequestId])
REFERENCES [dbo].[ATT_ProfileRequests] ([ProfileRequestId])
GO

ALTER TABLE [dbo].[ATT_ProfileOptionalDetails] CHECK CONSTRAINT [FK_ATT_ProfileOptionalDetails_ATT_ProfileRequests]
GO

/****** Object:  Table [dbo].[ATT_ProfileSupplierData] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATT_ProfileSupplierData](
	[ProfileSupplierDataId] [int] IDENTITY(1,1) NOT NULL,
	[ProfileRequestId] [int] NOT NULL,
	[Email] [nvarchar](100) NULL,
	[CompanyName] [nvarchar](100) NULL,
	[ASINumber] [nvarchar](10) NULL,
	[AttendeeName] [nvarchar](100) NULL,
	[AttendeeImage] [nvarchar] (200) NULL,
	[AttendeeTitle] [nvarchar](100) NULL,
	[AttendeeCommEmail] [nvarchar](100) NULL,
	[AttendeeCellPhone] [nvarchar](15) NULL,
	[AttendeeWorkPhone] [nvarchar](15) NULL,
	[CorporateAddress] [nvarchar](100) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Zip] [nvarchar](10) NULL,
	[CompanyWebsite] [nvarchar](50) NULL,
	[ProductSummary] [nvarchar](1000) NULL,
	[TrustFromDistributor] [nvarchar](1000) NULL,
	[SpecialServices] [nvarchar](1000) NULL,
	[LoyaltyPrograms] [nvarchar](1000) NULL,
	[Samples] [nvarchar](10) NULL,
	[ProductSafety] [nvarchar](1000) NULL,
	[FactAboutCompany] [nvarchar](1000) NULL,
	[IsUpdate] [BIT] NOT NULL DEFAULT 0,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ATT_ProfileSupplierData] PRIMARY KEY CLUSTERED 
(
	[ProfileSupplierDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ATT_ProfileSupplierData] ADD  CONSTRAINT [DF_ATT_ProfileSupplierData_UpdateSource]  DEFAULT ('Not Specified') FOR [UpdateSource]
GO

ALTER TABLE [dbo].[ATT_ProfileSupplierData]  WITH CHECK ADD  CONSTRAINT [FK_ATT_ProfileSupplierData_ATT_ProfileRequests] FOREIGN KEY([ProfileRequestId])
REFERENCES [dbo].[ATT_ProfileRequests] ([ProfileRequestId])
GO

ALTER TABLE [dbo].[ATT_ProfileSupplierData] CHECK CONSTRAINT [FK_ATT_ProfileSupplierData_ATT_ProfileRequests]
GO


SET IDENTITY_INSERT [dbo].[ATT_ProfileOptionalDataLabel] ON
INSERT INTO [dbo].[ATT_ProfileOptionalDataLabel]([ProfileOptionalDataLabelId],[Name],[Description],[CreateDateUTC],[UpdateDateUTC],[UpdateSource],[IsObsolete],[isSupplier],[isDistributor])
VALUES(1,'AttendeeName','Secondary Attendee Name',GETUTCDATE(), GETUTCDATE(),'Admin',null, 1,0),
(2,'AttendeeTitle','Secondary Attendee Title',GETUTCDATE(), GETUTCDATE(),'Admin',null, 1,0),
(3,'AttendeeCommEmail','Secondary Attendee Email',GETUTCDATE(), GETUTCDATE(),'Admin', null, 1,0),
(4,'AttendeeCellPhone','Secondary Attendee Cell Phone',GETUTCDATE(), GETUTCDATE(),'Admin', null, 1,0),
(5,'AttendeeWorkPhone','Secondary Attendee Work Phone',GETUTCDATE(), GETUTCDATE(),'Admin', null, 1,0),
(6,'AttendeeImage','Secondary Attendee Image',GETUTCDATE(), GETUTCDATE(),'Admin', null, 1,0),
(7,'OfferToDistributor','Product specials you are offering to fASIlitate distributors',GETUTCDATE(), GETUTCDATE(),'Admin', null, 1,0),
(8,'GoalForParticipating','What is your #1 goal for participating in fASIlitate?',GETUTCDATE(), GETUTCDATE(),'Admin', null, 1,0),
(9,'FiveDistributorCompanies','Please list five distributor companies you would like to see be invited to future fASIlitate events',GETUTCDATE(), GETUTCDATE(),'Admin', null, 1,0),
(10,'FOBlocations','What are your current FOB locations?',GETUTCDATE(), GETUTCDATE(),'Admin', null, 1,0) 
SET IDENTITY_INSERT [dbo].[ATT_ProfileOptionalDataLabel] OFF








