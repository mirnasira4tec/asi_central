
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ATT_EmployeeAttendee]') )
DROP TABLE [dbo].[ATT_EmployeeAttendee]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ATT_Attendee]'))
DROP TABLE [dbo].[ATT_Attendee]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ATT_Show]'))
DROP TABLE [dbo].[ATT_Show]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ATT_ShowType]'))
DROP TABLE [dbo].[ATT_ShowType]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ATT_Employee]'))
DROP TABLE [dbo].[ATT_Employee]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ATT_CompanyAddress]'))
DROP TABLE [dbo].[ATT_CompanyAddress]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ATT_Address]'))
DROP TABLE [dbo].[ATT_Address]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ATT_Company]'))
DROP TABLE [dbo].[ATT_Company]

--
-- Table structure for table `ATT_Company`
--
CREATE TABLE [dbo].[ATT_Company](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[WebUrl] [nvarchar](50) NULL,
	[ASINumber] [nvarchar](10) NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [varchar](128) NULL CONSTRAINT [ATT_Company_UpdateSource] DEFAULT ((suser_sname()+' Proc=')+isnull(object_name(@@procid),'')),
	[MemberType] [nvarchar](20) NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

--
-- Table structure for table `ATT_Address`
--
CREATE TABLE [dbo].[ATT_Address](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[Phone] [nvarchar](50) NULL,
	[PhoneAreaCode] [nchar](10) NULL,
	[FaxAreaCode] [nvarchar](50) NULL,
	[Fax] [nvarchar](50) NULL,
	[Street1] [nvarchar](50) NOT NULL,
	[Street2] [nvarchar](50) NULL,
	[Zip] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [varchar](128) NULL CONSTRAINT [ATT_Address_UpdateSource] DEFAULT ((suser_sname()+' Proc=')+isnull(object_name(@@procid),''))
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

--
-- Table structure for table `ATT_CompanyAddress`
--
CREATE TABLE [dbo].[ATT_CompanyAddress](
	[CompanyAddressId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[AddressId] [int] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [varchar](128) NULL CONSTRAINT [ATT_CompanyAddress_UpdateSource] DEFAULT ((suser_sname()+' Proc=')+isnull(object_name(@@procid),''))
 CONSTRAINT [PK_CompanyAddress] PRIMARY KEY CLUSTERED 
(
	[CompanyAddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ATT_CompanyAddress]  WITH CHECK ADD  CONSTRAINT [FK_CompanyAddress_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[ATT_Address] ([AddressId])
GO

ALTER TABLE [dbo].[ATT_CompanyAddress] CHECK CONSTRAINT [FK_CompanyAddress_Address]
GO

ALTER TABLE [dbo].[ATT_CompanyAddress]  WITH CHECK ADD  CONSTRAINT [FK_CompanyAddress_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[ATT_Company] ([CompanyId])
GO

ALTER TABLE [dbo].[ATT_CompanyAddress] CHECK CONSTRAINT [FK_CompanyAddress_Company]
GO

--
-- Table structure for table `ATT_Employee`
--
CREATE TABLE [dbo].[ATT_Employee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[LastName] [nchar](10) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[AddressId] [int] NULL,
	[Phone] [nvarchar](50) NULL,
	[PhoneAreaCode] [nchar](10) NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [varchar](128) NULL CONSTRAINT [ATT_Employee_UpdateSource] DEFAULT ((suser_sname()+' Proc=')+isnull(object_name(@@procid),''))
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ATT_Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[ATT_Address] ([AddressId])
GO

ALTER TABLE [dbo].[ATT_Employee] CHECK CONSTRAINT [FK_Employee_Address]
GO

ALTER TABLE [dbo].[ATT_Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[ATT_Company] ([CompanyId])
GO

ALTER TABLE [dbo].[ATT_Employee] CHECK CONSTRAINT [FK_Employee_Company]
GO

--
-- Table structure for table `ATT_ShowType`
--
CREATE TABLE [dbo].[ATT_ShowType](
	[ShowTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [varchar](128) NULL CONSTRAINT [ATT_ShowType_UpdateSource] DEFAULT ((suser_sname()+' Proc=')+isnull(object_name(@@procid),''))
 CONSTRAINT [PK_ShowType] PRIMARY KEY CLUSTERED 
(
	[ShowTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

Insert into [dbo].[ATT_ShowType] (Type,CreateDateUTC,UpdateDateUTC) values ('Engage East' ,GETDATE(),GETDATE()), ('Engage West' ,GETDATE(),GETDATE())

GO

--
-- Table structure for table `ATT_Show`
--
CREATE TABLE [dbo].[ATT_Show](
	[ShowId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[TypeId] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [varchar](128) NULL CONSTRAINT [ATT_Show_UpdateSource] DEFAULT ((suser_sname()+' Proc=')+isnull(object_name(@@procid),'')),
	[Address] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Show] PRIMARY KEY CLUSTERED 
(
	[ShowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ATT_Show]  WITH CHECK ADD  CONSTRAINT [FK_Show_ShowType] FOREIGN KEY([TypeId])
REFERENCES [dbo].[ATT_ShowType] ([ShowTypeId])
GO

ALTER TABLE [dbo].[ATT_Show] CHECK CONSTRAINT [FK_Show_ShowType]
GO

--
-- Table structure for table `ATT_Attendee`
--
CREATE TABLE [dbo].[ATT_Attendee](
	[AttendeeId] [int] IDENTITY(1,1) NOT NULL,
	[ShowId] [int] NULL,
	[AddressId] [int] NULL,
	[CompanyId] [int] NOT NULL,
	[IsAttending] [bit] NOT NULL DEFAULT '0',
	[IsSponsor] [bit] NOT NULL DEFAULT '0',
	[IsExhibitDay] [bit] NOT NULL DEFAULT '0',
	[BoothNumber] [nvarchar](50) NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [varchar](128) NULL CONSTRAINT [ATT_Attendee_UpdateSource] DEFAULT ((suser_sname()+' Proc=')+isnull(object_name(@@procid),''))
 CONSTRAINT [PK_Attendees] PRIMARY KEY CLUSTERED 
(
	[AttendeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO



ALTER TABLE [dbo].[ATT_Attendee]  WITH CHECK ADD  CONSTRAINT [FK_Attendee_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[ATT_Company] ([CompanyId])
GO

ALTER TABLE [dbo].[ATT_Attendee] CHECK CONSTRAINT [FK_Attendee_Company]
GO

ALTER TABLE [dbo].[ATT_Attendee]  WITH CHECK ADD  CONSTRAINT [FK_Attendee_Show] FOREIGN KEY([ShowId])
REFERENCES [dbo].[ATT_Show] ([ShowId])
GO

ALTER TABLE [dbo].[ATT_Attendee] CHECK CONSTRAINT [FK_Attendee_Show]
GO

--
-- Table structure for table `ATT_EmployeeAttendee`
--

CREATE TABLE [dbo].[ATT_EmployeeAttendee](
	[EmployeeAttendeeId] [int] IDENTITY(1,1) NOT NULL,
	[AttendeeId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [varchar](128) NULL CONSTRAINT [ATT_EmployeeAttendee_UpdateSource] DEFAULT ((suser_sname()+' Proc=')+isnull(object_name(@@procid),''))
 CONSTRAINT [PK_EmployeeAttendee] PRIMARY KEY CLUSTERED 
(
	[EmployeeAttendeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ATT_EmployeeAttendee]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeAttendee_Attendee] FOREIGN KEY([AttendeeId])
REFERENCES [dbo].[ATT_Attendee] ([AttendeeId])
GO

ALTER TABLE [dbo].[ATT_EmployeeAttendee] CHECK CONSTRAINT [FK_EmployeeAttendee_Attendee]
GO

ALTER TABLE [dbo].[ATT_EmployeeAttendee]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeAttendee_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[ATT_Employee] ([EmployeeId])
GO

ALTER TABLE [dbo].[ATT_EmployeeAttendee] CHECK CONSTRAINT [FK_EmployeeAttendee_Employee]
GO