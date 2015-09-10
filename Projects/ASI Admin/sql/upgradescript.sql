
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


--
-- Insert Data into `ATT_Show`
--
INSERT [dbo].[ATT_Show] ( [Name], [TypeId], [StartDate], [EndDate], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [Address]) VALUES ( N'Engage West 2016', 2, CAST(0x0000A5C900000000 AS DateTime), CAST(0x0000A5CB00000000 AS DateTime), CAST(0x0000A50F006785E6 AS DateTime), CAST(0x0000A50F006785E8 AS DateTime), N'ShowController - AddShow', N'Anaheim Marriott, Anaheim, CA')
INSERT [dbo].[ATT_Show] ( [Name], [TypeId], [StartDate], [EndDate], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [Address]) VALUES ( N'Engage East 2016', 1, CAST(0x0000A60100000000 AS DateTime), CAST(0x0000A60300000000 AS DateTime), CAST(0x0000A50F0067D931 AS DateTime), CAST(0x0000A50F0067D931 AS DateTime), N'ShowController - AddShow', N'Marriott Marquis, New York, NY')

--
-- Insert Data into `ATT_Address`
--
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'2730 Monterey St Ste 108', NULL, N'90503-7230', N'CA', N'USA', N'Torrance', CAST(0x0000A50F007634ED AS DateTime), CAST(0x0000A50F007634ED AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'869 S Keim St', NULL, N'19465', N'PA', N'USA', N'Pottstown', CAST(0x0000A50F007696B2 AS DateTime), CAST(0x0000A50F007696B2 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'4115 Profit Ct', NULL, N'47150-7207', N'IN', N'USA', N'New Albany', CAST(0x0000A50F00770693 AS DateTime), CAST(0x0000A50F00770693 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'7145 Boone Ave N Ste 100', NULL, N'55428-1556', N'MN', N'USA', N'Brooklyn Park', CAST(0x0000A50F00775DB9 AS DateTime), CAST(0x0000A50F00775DB9 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'500 S Kraemer Blvd Ste 185', NULL, N'92821-1715', N'CA', N'USA', N'Brea', CAST(0x0000A50F0077CB41 AS DateTime), CAST(0x0000A50F0077CB41 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'10415A Westpark Dr', NULL, N'77042-5314', N'TX', N'USA', N'Houston', CAST(0x0000A50F00781B60 AS DateTime), CAST(0x0000A50F00781B60 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'27525 Puerta Real Ste 100-460', NULL, N'92691-6379', N'CA', N'USA', N'Mission Viejo', CAST(0x0000A50F0078A3C2 AS DateTime), CAST(0x0000A50F0078A3C2 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'9033 Murphy Rd', NULL, N'60517-1100', N'IL', N'USA', N'Woodridge', CAST(0x0000A50F00791D18 AS DateTime), CAST(0x0000A50F00791D18 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'1130 N Main St', NULL, N'92867-3421', N'CA', N'USA', N'Orange', CAST(0x0000A50F0079713E AS DateTime), CAST(0x0000A50F0079713E AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'6325 McCoy Rd', NULL, N'32822-5167', N'FL', N'USA', N'Orlando', CAST(0x0000A50F0079E5E5 AS DateTime), CAST(0x0000A50F0079E5E5 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'PO Box 3669', NULL, N'56002-3669', N'MN', N'USA', N'Mankato', CAST(0x0000A50F007A9702 AS DateTime), CAST(0x0000A50F007A9702 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'PO Box 1759', NULL, N'93515-1759', N'CA', N'USA', N'Bishop', CAST(0x0000A50F007AE6BC AS DateTime), CAST(0x0000A50F007AE6BD AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'55 E Hintz Rd', NULL, N'60090', N'IL', N'USA', N'Wheeling', CAST(0x0000A50F007B3E70 AS DateTime), CAST(0x0000A50F007B3E70 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'2110 E Winston Rd', NULL, N'92806-5534', N'CA', N'USA', N'Anaheim', CAST(0x0000A50F007BA2B0 AS DateTime), CAST(0x0000A50F007BA2B0 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'1730 W Bruton Rd', NULL, N'75180-1119', N'TX', N'USA', N'Balch Springs', CAST(0x0000A50F007C06BF AS DateTime), CAST(0x0000A50F007C06BF AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'PO Box 101029', NULL, N'76185-1029', N'TX', N'USA', N'Fort Worth', CAST(0x0000A50F007C5FB9 AS DateTime), CAST(0x0000A50F007C5FB9 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'140 Calle Iglesia', NULL, N'92672-7502', N'CA', N'USA', N'San Clemente', CAST(0x0000A50F007CDC3B AS DateTime), CAST(0x0000A50F007CDC3B AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'420 Benigno Blvd Unit F', NULL, N'08031-2563', N'NJ', N'USA', N'Bellmawr', CAST(0x0000A50F007D3AFE AS DateTime), CAST(0x0000A50F007D3AFE AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'6 Neshaminy Interplex Dr Fl 6', NULL, N'19053-6942', N'PA', N'USA', N'Trevose', CAST(0x0000A50F007DC425 AS DateTime), CAST(0x0000A50F007DC425 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'10498 Colonel Ct Ste 104', NULL, N'20110-6794', N'VA', N'USA', N'Manassas', CAST(0x0000A50F007E3430 AS DateTime), CAST(0x0000A50F007E3430 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'2951 Grant Ave', NULL, N'19114-1012', N'PA', N'USA', N'Philadelphia', CAST(0x0000A50F007E8FE8 AS DateTime), CAST(0x0000A50F007E8FE8 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'1635 Commons Pkwy', NULL, N'14502-9191', N'NY', N'USA', N'Macedon', CAST(0x0000A50F007F3582 AS DateTime), CAST(0x0000A50F007F3583 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'345 Plato Blvd E', NULL, N'55107-1211', N'MN', N'USA', N'Saint Paul', CAST(0x0000A50F008BEACF AS DateTime), CAST(0x0000A50F008BEAD0 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'1729 S Davis Rd', NULL, N'30241-6172', N'GA', N'USA', N'LaGrange', CAST(0x0000A50F008C4D4F AS DateTime), CAST(0x0000A50F008C4D4F AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'64 Healey Rd Unit 4', NULL, N'L7E-5A5', N'ON', N'CAN', N'Bolton', CAST(0x0000A50F008CDC1F AS DateTime), CAST(0x0000A50F008CDC1F AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'4771 Sweetwater Blvd Ste 241', NULL, N'77479-3121', N'TX', N'USA', N'Sugar Land', CAST(0x0000A50F008D660C AS DateTime), CAST(0x0000A50F008D660C AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'22833 Se Black Nugget Rd Ste 130', NULL, N'98029-3621', N'WA', N'USA', N'Issaquah', CAST(0x0000A50F008E5B45 AS DateTime), CAST(0x0000A50F008E5B45 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'180 Central Ave', NULL, N'11735-6928', N'NY', N'USA', N'Farmingdale', CAST(0x0000A50F008FC25F AS DateTime), CAST(0x0000A50F008FC25F AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'1440 Innovative Dr Ste 300', NULL, N'92154-6631', N'CA', N'USA', N'San Diego', CAST(0x0000A50F00909118 AS DateTime), CAST(0x0000A50F00909118 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'1500 NE 131st St', NULL, N'33161-4426', N'FL', N'USA', N'North Miami', CAST(0x0000A50F0090F952 AS DateTime), CAST(0x0000A50F0090F952 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'28458 Selke Rd', NULL, N'55925-4187', N'MN', N'USA', N'Dakota', CAST(0x0000A50F0091BF92 AS DateTime), CAST(0x0000A50F0091BF92 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'PO Box 4029', NULL, N'33014-0029', N'FL', N'USA', N'Hialeah', CAST(0x0000A50F00922212 AS DateTime), CAST(0x0000A50F00922212 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'PO Box 7009', NULL, N'7107', N'NJ', N'USA', N'Newark', CAST(0x0000A50F0092AFD8 AS DateTime), CAST(0x0000A50F0092AFD8 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'311 Mechanic St', NULL, N'07005-1830', N'NJ', N'USA', N'Boonton', CAST(0x0000A50F00937709 AS DateTime), CAST(0x0000A50F00937709 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'2001 Marcus Ave Ste N114', NULL, N'11042', N'NY', N'USA', N'Lake Success', CAST(0x0000A50F0093FC92 AS DateTime), CAST(0x0000A50F0093FC92 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'1205 Providence Hwy Rte 1', NULL, N'02067-1671', N'MA', N'USA', N'Sharon', CAST(0x0000A50F009551E6 AS DateTime), CAST(0x0000A50F009551E7 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'PO Box 67', NULL, N'07604-0067', N'NJ', N'USA', N'Hasbrouck Hts', CAST(0x0000A50F009664ED AS DateTime), CAST(0x0000A50F009664ED AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'24795 County Road 75', NULL, N'56301', N'MN', N'USA', N'Saint Cloud', CAST(0x0000A50F0096F16A AS DateTime), CAST(0x0000A50F0096F16A AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'1425 Holland Rd Ste B', NULL, N'43537', N'OH', N'USA', N'Maumee', CAST(0x0000A50F00979E1D AS DateTime), CAST(0x0000A50F00979E1D AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'2025 Lookout Dr', NULL, N'56003-1719', N'MN', N'USA', N'North Mankato', CAST(0x0000A50F00984512 AS DateTime), CAST(0x0000A50F00984512 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'PO Box 888', NULL, N'55987-0888', N'MN', N'USA', N'Winona', CAST(0x0000A50F0098CB78 AS DateTime), CAST(0x0000A50F0098CB78 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'2660 W Mason St', NULL, N'54303-4963', N'WI', N'USA', N'Green Bay', CAST(0x0000A50F00996A25 AS DateTime), CAST(0x0000A50F00996A25 AS DateTime), N'ShowCompanyController - AddCompany')
INSERT [dbo].[ATT_Address] ( [Phone], [PhoneAreaCode], [FaxAreaCode], [Fax], [Street1], [Street2], [Zip], [State], [Country], [City], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (NULL, NULL, NULL, NULL, N'Pressley Rd', NULL, N'28217-0974', N'NC', N'USA', N'Charlotte', CAST(0x0000A50F0099DE89 AS DateTime), CAST(0x0000A50F0099DE89 AS DateTime), N'ShowCompanyController - AddCompany')

--
-- Insert Data into `ATT_Company`
--
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'CleaRush Prints Inc', NULL, N'45395', CAST(0x0000A50F007634D6 AS DateTime), CAST(0x0000A50F007634ED AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Corry Enterprises', NULL, N'46509', CAST(0x0000A50F0076969C AS DateTime), CAST(0x0000A50F007696B2 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Discount Labels', NULL, N'49890', CAST(0x0000A50F0077067B AS DateTime), CAST(0x0000A50F00770693 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Halls & Company', NULL, N'59080', CAST(0x0000A50F00775DA3 AS DateTime), CAST(0x0000A50F00775DB9 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Idol Memory', NULL, N'62222', CAST(0x0000A50F0077CB2A AS DateTime), CAST(0x0000A50F0077CB41 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'KTI Networks Inc', NULL, N'63776', CAST(0x0000A50F00781B4B AS DateTime), CAST(0x0000A50F00781B60 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'MSL Line Inc', NULL, N'68314', CAST(0x0000A50F0078A3AC AS DateTime), CAST(0x0000A50F0078A3C2 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Orbus Exhibit & Display Group', NULL, N'75209', CAST(0x0000A50F00791D03 AS DateTime), CAST(0x0000A50F00791D18 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Presentation Folder Inc', NULL, N'79396', CAST(0x0000A50F00797127 AS DateTime), CAST(0x0000A50F0079713E AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Price Chopper Inc', NULL, N'79500', CAST(0x0000A50F0079E5CF AS DateTime), CAST(0x0000A50F0079E5E5 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Navitor', NULL, N'81500', CAST(0x0000A50F007A96EC AS DateTime), CAST(0x0000A50F007A9702 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Vertical Imprints', NULL, N'93635', CAST(0x0000A50F007AE691 AS DateTime), CAST(0x0000A50F007AE6BC AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Qpromo', NULL, N'47962', CAST(0x0000A50F007B3E57 AS DateTime), CAST(0x0000A50F007B3E70 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Universal Frames', NULL, N'92940', CAST(0x0000A50F007BA29B AS DateTime), CAST(0x0000A50F007BA2B0 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Best Promotions USA LLC', NULL, N'40344', CAST(0x0000A50F007C06A9 AS DateTime), CAST(0x0000A50F007C06BF AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Flexible Innovations Ltd', NULL, N'54596', CAST(0x0000A50F007C5FA3 AS DateTime), CAST(0x0000A50F007C5FB9 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'AP Specialties', NULL, N'30208', CAST(0x0000A50F007CDC25 AS DateTime), CAST(0x0000A50F007CDC3B AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Admints & Zagabor', NULL, N'31516', CAST(0x0000A50F007D3AE7 AS DateTime), CAST(0x0000A50F007D3AFE AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Alphabroder', NULL, N'34063', CAST(0x0000A50F007DC3F6 AS DateTime), CAST(0x0000A50F007DC425 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'BamBams', NULL, N'38228', CAST(0x0000A50F007E341B AS DateTime), CAST(0x0000A50F007E3430 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Bodek & Rhodes', NULL, N'40788', CAST(0x0000A50F007E8FAB AS DateTime), CAST(0x0000A50F007E8FE8 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Devara', NULL, N'49470', CAST(0x0000A50F007F356C AS DateTime), CAST(0x0000A50F007F3582 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Hotline Products', NULL, N'61960', CAST(0x0000A50F008BEAB8 AS DateTime), CAST(0x0000A50F008BEACF AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Logo Mats', NULL, N'67849', CAST(0x0000A50F008C4D37 AS DateTime), CAST(0x0000A50F008C4D4F AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Payne MFG Co LTD', NULL, N'76576', CAST(0x0000A50F008CDBEA AS DateTime), CAST(0x0000A50F008CDC1F AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'PMGOA', NULL, N'79982', CAST(0x0000A50F008D65AE AS DateTime), CAST(0x0000A50F008D660C AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'SanMar', NULL, N'84863', CAST(0x0000A50F008E5B2F AS DateTime), CAST(0x0000A50F008E5B45 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Tekweld', NULL, N'90807', CAST(0x0000A50F008FC1F2 AS DateTime), CAST(0x0000A50F008FC25F AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Terry Town', NULL, N'90913', CAST(0x0000A50F00908FD7 AS DateTime), CAST(0x0000A50F00909118 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'World Emblem', NULL, N'98264', CAST(0x0000A50F0090F93B AS DateTime), CAST(0x0000A50F0090F952 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Express-A-Button', NULL, N'53408', CAST(0x0000A50F0091BF7D AS DateTime), CAST(0x0000A50F0091BF92 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Cotton Love', NULL, N'46756', CAST(0x0000A50F009221FD AS DateTime), CAST(0x0000A50F00922212 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Unionwear', NULL, N'73775', CAST(0x0000A50F0092AFC3 AS DateTime), CAST(0x0000A50F0092AFD8 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Patchworks Embroidery', NULL, N'76428', CAST(0x0000A50F00937664 AS DateTime), CAST(0x0000A50F00937709 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Stone Enterprises', NULL, N'89850', CAST(0x0000A50F0093FBDD AS DateTime), CAST(0x0000A50F0093FC92 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Charles River Apprel', NULL, N'44620', CAST(0x0000A50F009551D2 AS DateTime), CAST(0x0000A50F009551E6 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Concap Sportswear LLC', NULL, N'46187', CAST(0x0000A50F009664D7 AS DateTime), CAST(0x0000A50F009664ED AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Fields Manufacturing', NULL, N'54100', CAST(0x0000A50F0096F155 AS DateTime), CAST(0x0000A50F0096F16A AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Haas-Jordan Umbrellas', NULL, N'58860', CAST(0x0000A50F00979E07 AS DateTime), CAST(0x0000A50F00979E1D AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Label Works, a Navitor Company', NULL, N'66040', CAST(0x0000A50F009844FB AS DateTime), CAST(0x0000A50F00984512 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'LarLu', NULL, N'66390', CAST(0x0000A50F0098CB63 AS DateTime), CAST(0x0000A50F0098CB78 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Medalcraft Mint Inc', NULL, N'70130', CAST(0x0000A50F00996A0F AS DateTime), CAST(0x0000A50F00996A25 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')
INSERT [dbo].[ATT_Company] ( [Name], [WebUrl], [ASINumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [MemberType]) VALUES (N'Century Place Apparel', NULL, N'85988', CAST(0x0000A50F0099DE74 AS DateTime), CAST(0x0000A50F0099DE89 AS DateTime), N'ShowCompanyController - AddCompany', N'Supplier')

--
-- Insert Data into `ATT_CompanyAddress`
--
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, 1, CAST(0x0000A50F007634EF AS DateTime), CAST(0x0000A50F007634EF AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, 2, CAST(0x0000A50F007696B3 AS DateTime), CAST(0x0000A50F007696B3 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (3, 3, CAST(0x0000A50F00770693 AS DateTime), CAST(0x0000A50F00770693 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (4, 4, CAST(0x0000A50F00775DBA AS DateTime), CAST(0x0000A50F00775DBA AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (5, 5, CAST(0x0000A50F0077CB42 AS DateTime), CAST(0x0000A50F0077CB42 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (6, 6, CAST(0x0000A50F00781B61 AS DateTime), CAST(0x0000A50F00781B61 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (7, 7, CAST(0x0000A50F0078A3C3 AS DateTime), CAST(0x0000A50F0078A3C3 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (8, 8, CAST(0x0000A50F00791D19 AS DateTime), CAST(0x0000A50F00791D19 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (9, 9, CAST(0x0000A50F0079713F AS DateTime), CAST(0x0000A50F0079713F AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (10, 10, CAST(0x0000A50F0079E5E5 AS DateTime), CAST(0x0000A50F0079E5E6 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (11, 11, CAST(0x0000A50F007A9703 AS DateTime), CAST(0x0000A50F007A9703 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (12, 12, CAST(0x0000A50F007AE6BD AS DateTime), CAST(0x0000A50F007AE6BD AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (13, 13, CAST(0x0000A50F007B3E70 AS DateTime), CAST(0x0000A50F007B3E70 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (14, 14, CAST(0x0000A50F007BA2B1 AS DateTime), CAST(0x0000A50F007BA2B1 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (15, 15, CAST(0x0000A50F007C06C0 AS DateTime), CAST(0x0000A50F007C06C0 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (16, 16, CAST(0x0000A50F007C5FBA AS DateTime), CAST(0x0000A50F007C5FBA AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (17, 17, CAST(0x0000A50F007CDC3C AS DateTime), CAST(0x0000A50F007CDC3C AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (18, 18, CAST(0x0000A50F007D3AFF AS DateTime), CAST(0x0000A50F007D3AFF AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (19, 19, CAST(0x0000A50F007DC427 AS DateTime), CAST(0x0000A50F007DC427 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (20, 20, CAST(0x0000A50F007E3431 AS DateTime), CAST(0x0000A50F007E3431 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (21, 21, CAST(0x0000A50F007E8FE9 AS DateTime), CAST(0x0000A50F007E8FE9 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (22, 22, CAST(0x0000A50F007F3583 AS DateTime), CAST(0x0000A50F007F3583 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (23, 23, CAST(0x0000A50F008BEAD0 AS DateTime), CAST(0x0000A50F008BEAD0 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (24, 24, CAST(0x0000A50F008C4D50 AS DateTime), CAST(0x0000A50F008C4D50 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (25, 25, CAST(0x0000A50F008CDC20 AS DateTime), CAST(0x0000A50F008CDC20 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (26, 26, CAST(0x0000A50F008D660D AS DateTime), CAST(0x0000A50F008D660D AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (27, 27, CAST(0x0000A50F008E5B45 AS DateTime), CAST(0x0000A50F008E5B45 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (28, 28, CAST(0x0000A50F008FC260 AS DateTime), CAST(0x0000A50F008FC260 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (29, 29, CAST(0x0000A50F00909119 AS DateTime), CAST(0x0000A50F00909119 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (30, 30, CAST(0x0000A50F0090F953 AS DateTime), CAST(0x0000A50F0090F954 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (31, 31, CAST(0x0000A50F0091BF93 AS DateTime), CAST(0x0000A50F0091BF93 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (32, 32, CAST(0x0000A50F00922213 AS DateTime), CAST(0x0000A50F00922213 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (33, 33, CAST(0x0000A50F0092AFD9 AS DateTime), CAST(0x0000A50F0092AFD9 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (34, 34, CAST(0x0000A50F0093770A AS DateTime), CAST(0x0000A50F0093770A AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (35, 35, CAST(0x0000A50F0093FC92 AS DateTime), CAST(0x0000A50F0093FC92 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (36, 36, CAST(0x0000A50F009551E7 AS DateTime), CAST(0x0000A50F009551E7 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (37, 37, CAST(0x0000A50F009664EE AS DateTime), CAST(0x0000A50F009664EE AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (38, 38, CAST(0x0000A50F0096F16B AS DateTime), CAST(0x0000A50F0096F16B AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (39, 39, CAST(0x0000A50F00979E1D AS DateTime), CAST(0x0000A50F00979E1D AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (40, 40, CAST(0x0000A50F00984513 AS DateTime), CAST(0x0000A50F00984513 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (41, 41, CAST(0x0000A50F0098CB79 AS DateTime), CAST(0x0000A50F0098CB79 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (42, 42, CAST(0x0000A50F00996A26 AS DateTime), CAST(0x0000A50F00996A26 AS DateTime), N'ShowCompanyController - AddAddress')
INSERT [dbo].[ATT_CompanyAddress] ( [CompanyId], [AddressId], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (43, 43, CAST(0x0000A50F0099DE89 AS DateTime), CAST(0x0000A50F0099DE89 AS DateTime), N'ShowCompanyController - AddAddress')

--
-- Insert Data into `ATT_Attendee`
--
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 1, 0, 0, 1, NULL, CAST(0x0000A50F009BD16B AS DateTime), CAST(0x0000A50F009BD16C AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 2, 0, 0, 1, NULL, CAST(0x0000A50F009BE76F AS DateTime), CAST(0x0000A50F009BE770 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 3, 0, 0, 1, NULL, CAST(0x0000A50F009BF8B0 AS DateTime), CAST(0x0000A50F009BF8B0 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 4, 0, 0, 1, NULL, CAST(0x0000A50F009C1262 AS DateTime), CAST(0x0000A50F009C1262 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 5, 0, 0, 1, NULL, CAST(0x0000A50F009C233A AS DateTime), CAST(0x0000A50F009C233A AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 6, 0, 0, 1, NULL, CAST(0x0000A50F009C2EF3 AS DateTime), CAST(0x0000A50F009C2EF3 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 7, 0, 0, 1, NULL, CAST(0x0000A50F009C499E AS DateTime), CAST(0x0000A50F009C499E AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 8, 0, 0, 1, NULL, CAST(0x0000A50F009C54A4 AS DateTime), CAST(0x0000A50F009C54A4 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 9, 0, 0, 1, NULL, CAST(0x0000A50F009C6CE3 AS DateTime), CAST(0x0000A50F009C6CE3 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 10, 0, 0, 1, NULL, CAST(0x0000A50F009C7967 AS DateTime), CAST(0x0000A50F009C7967 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 11, 0, 0, 1, NULL, CAST(0x0000A50F009CA2A7 AS DateTime), CAST(0x0000A50F009CA2A7 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 12, 0, 0, 1, NULL, CAST(0x0000A50F009CAE5E AS DateTime), CAST(0x0000A50F009CAE5E AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 13, 0, 0, 1, NULL, CAST(0x0000A50F009CC6D0 AS DateTime), CAST(0x0000A50F009CC6D0 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 14, 0, 0, 1, NULL, CAST(0x0000A50F009CD463 AS DateTime), CAST(0x0000A50F009CD463 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 20, 1, 1, 0, NULL, CAST(0x0000A50F009CF99F AS DateTime), CAST(0x0000A50F009CF99F AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 24, 1, 1, 0, NULL, CAST(0x0000A50F009D0FA9 AS DateTime), CAST(0x0000A50F009D0FA9 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, NULL, 28, 1, 1, 0, NULL, CAST(0x0000A50F009D2839 AS DateTime), CAST(0x0000A50F009D283A AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 20, 1, 1, 0, NULL, CAST(0x0000A50F009DAE19 AS DateTime), CAST(0x0000A50F009DAE19 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 24, 1, 1, 0, NULL, CAST(0x0000A50F009DBB8F AS DateTime), CAST(0x0000A50F009DBB90 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 28, 1, 1, 0, NULL, CAST(0x0000A50F009DCC57 AS DateTime), CAST(0x0000A50F009DCC57 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 1, 0, 0, 1, NULL, CAST(0x0000A50F009DE55D AS DateTime), CAST(0x0000A50F009DE55E AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 2, 0, 0, 1, NULL, CAST(0x0000A50F009DF03A AS DateTime), CAST(0x0000A50F009DF03A AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 3, 0, 0, 1, NULL, CAST(0x0000A50F009E00B7 AS DateTime), CAST(0x0000A50F009E00B7 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 4, 0, 0, 1, NULL, CAST(0x0000A50F009E0F03 AS DateTime), CAST(0x0000A50F009E0F03 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 5, 0, 0, 1, NULL, CAST(0x0000A50F009E24FC AS DateTime), CAST(0x0000A50F009E24FC AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 6, 0, 0, 1, NULL, CAST(0x0000A50F009E45CC AS DateTime), CAST(0x0000A50F009E45CC AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 34, 0, 0, 1, NULL, CAST(0x0000A50F009ED140 AS DateTime), CAST(0x0000A50F009ED140 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 10, 0, 0, 1, NULL, CAST(0x0000A50F009EEA13 AS DateTime), CAST(0x0000A50F009EEA13 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 11, 0, 0, 1, NULL, CAST(0x0000A50F009F2A40 AS DateTime), CAST(0x0000A50F009F2A40 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 35, 0, 0, 1, NULL, CAST(0x0000A50F009F3B68 AS DateTime), CAST(0x0000A50F009F3B68 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 13, 0, 0, 1, NULL, CAST(0x0000A50F009F4E46 AS DateTime), CAST(0x0000A50F009F4E46 AS DateTime), N'ShowController - PostShowAttendeeInformation')
INSERT [dbo].[ATT_Attendee] ( [ShowId], [AddressId], [CompanyId], [IsAttending], [IsSponsor], [IsExhibitDay], [BoothNumber], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, NULL, 14, 0, 0, 1, NULL, CAST(0x0000A50F009F5971 AS DateTime), CAST(0x0000A50F009F5971 AS DateTime), N'ShowController - PostShowAttendeeInformation')

