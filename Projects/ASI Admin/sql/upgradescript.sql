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

CREATE TABLE [dbo].[ATT_ProfileRequestOptionalDetails](
	[ProfileRequestOptionalDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ProfileRequestId] [int] NOT NULL,
	[ProfileOptionalDataLabelId] [int] NOT NULL,
	[UpdateValue] [nvarchar](1000) NULL,
	[OrigValue] [nvarchar](1000) NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ATT_ProfileRequestOptionalDetails] PRIMARY KEY CLUSTERED 
(
	[ProfileRequestOptionalDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ATT_ProfileRequestOptionalDetails] ADD  CONSTRAINT [DF_ATT_ProfileRequestOptionalDetails_UpdateSource]  DEFAULT ('Not Specified') FOR [UpdateSource]
GO

ALTER TABLE [dbo].[ATT_ProfileRequestOptionalDetails]  WITH CHECK ADD  CONSTRAINT [FK_ATT_ProfileRequestOptionalDetails_ATT_ProfileOptionalDataLabel] FOREIGN KEY([ProfileOptionalDataLabelId])
REFERENCES [dbo].[ATT_ProfileOptionalDataLabel] ([ProfileOptionalDataLabelId])
GO

ALTER TABLE [dbo].[ATT_ProfileRequestOptionalDetails] CHECK CONSTRAINT [FK_ATT_ProfileRequestOptionalDetails_ATT_ProfileOptionalDataLabel]
GO

ALTER TABLE [dbo].[ATT_ProfileRequestOptionalDetails]  WITH CHECK ADD  CONSTRAINT [FK_ATT_ProfileRequestOptionalDetails_ATT_ProfileRequests] FOREIGN KEY([ProfileRequestId])
REFERENCES [dbo].[ATT_ProfileRequests] ([ProfileRequestId])
GO

ALTER TABLE [dbo].[ATT_ProfileRequestOptionalDetails] CHECK CONSTRAINT [FK_ATT_ProfileRequestOptionalDetails_ATT_ProfileRequests]
GO

/****** Object:  Table [dbo].[ATT_ProfileRequiredData] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATT_ProfileRequiredData](
	[ProfileRequiredDataId] [int] IDENTITY(1,1) NOT NULL,
	[ProfileRequestId] [int] NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[CompanyName] [nvarchar](100) NOT NULL,
	[ASINumber] [nvarchar](10) NOT NULL,
	[AttendeeName] [nvarchar](100) NOT NULL,
	[AttendeeTitle] [nvarchar](100) NOT NULL,
	[AttendeeCommEmail] [nvarchar](100) NOT NULL,
	[AttendeeCellPhone] [nvarchar](15) NOT NULL,
	[AttendeeWorkPhone] [nvarchar](15) NOT NULL,
	[CorporateAddress] [nvarchar](100) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[Zip] [nvarchar](10) NOT NULL,
	[CompanyWebsite] [nvarchar](50) NOT NULL,
	[ProductSummary] [nvarchar](1000) NOT NULL,
	[TrustFromDistributor] [nvarchar](1000) NOT NULL,
	[SpecialServices] [nvarchar](1000) NOT NULL,
	[LoyaltyPrograms] [nvarchar](1000) NOT NULL,
	[Samples] [nvarchar](10) NOT NULL,
	[ProductSafety] [nvarchar](1000) NOT NULL,
	[FactAboutCompany] [nvarchar](1000) NOT NULL,
	[IsUpdate] [BIT] NOT NULL DEFAULT 0
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ATT_ProfileRequiredData] PRIMARY KEY CLUSTERED 
(
	[ProfileRequiredDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ATT_ProfileRequiredData] ADD  CONSTRAINT [DF_ATT_ProfileRequiredData_UpdateSource]  DEFAULT ('Not Specified') FOR [UpdateSource]
GO

ALTER TABLE [dbo].[ATT_ProfileRequiredData]  WITH CHECK ADD  CONSTRAINT [FK_ATT_ProfileRequiredData_ATT_ProfileRequests] FOREIGN KEY([ProfileRequestId])
REFERENCES [dbo].[ATT_ProfileRequests] ([ProfileRequestId])
GO

ALTER TABLE [dbo].[ATT_ProfileRequiredData] CHECK CONSTRAINT [FK_ATT_ProfileRequiredData_ATT_ProfileRequests]
GO








