--
-- Table structure for table `ATT_Company`
--
CREATE TABLE [ATT_Company](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[WebUrl] [nvarchar](50) NULL,
	[ASINumber] [nvarchar](10) NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL DEFAULT 'Not Specified',
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
CREATE TABLE [ATT_Address](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[PhoneAreaCode] [nchar](10) NOT NULL,
	[FaxAreaCode] [nvarchar](50) NULL,
	[Fax] [nvarchar](50) NULL,
	[Street1] [nvarchar](50) NOT NULL,
	[Street2] [nvarchar](50) NULL,
	[Zip] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[City] [nchar](10) NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL DEFAULT 'Not Specified',
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

--
-- Table structure for table `ATT_CompanyAddress`
--
CREATE TABLE [ATT_CompanyAddress](
	[CompanyAddressId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[AddressId] [int] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL DEFAULT 'Not Specified',
 CONSTRAINT [PK_CompanyAddress] PRIMARY KEY CLUSTERED 
(
	[CompanyAddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ATT_CompanyAddress]  WITH CHECK ADD  CONSTRAINT [FK_CompanyAddress_Address] FOREIGN KEY([AddressId])
REFERENCES [ATT_Address] ([AddressId])
GO

ALTER TABLE [ATT_CompanyAddress] CHECK CONSTRAINT [FK_CompanyAddress_Address]
GO

ALTER TABLE [ATT_CompanyAddress]  WITH CHECK ADD  CONSTRAINT [FK_CompanyAddress_Company] FOREIGN KEY([CompanyId])
REFERENCES [ATT_Company] ([CompanyId])
GO

ALTER TABLE [ATT_CompanyAddress] CHECK CONSTRAINT [FK_CompanyAddress_Company]
GO

--
-- Table structure for table `ATT_Employee`
--
CREATE TABLE [ATT_Employee](
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
	[UpdateSource] [nvarchar](100) NOT NULL DEFAULT 'Not Specified',
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ATT_Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Address] FOREIGN KEY([AddressId])
REFERENCES [ATT_Address] ([AddressId])
GO

ALTER TABLE [ATT_Employee] CHECK CONSTRAINT [FK_Employee_Address]
GO

ALTER TABLE [ATT_Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Company] FOREIGN KEY([CompanyId])
REFERENCES [ATT_Company] ([CompanyId])
GO

ALTER TABLE [ATT_Employee] CHECK CONSTRAINT [FK_Employee_Company]
GO

--
-- Table structure for table `ATT_ShowType`
--
CREATE TABLE [ATT_ShowType](
	[ShowTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL DEFAULT 'Not Specified',
 CONSTRAINT [PK_ShowType] PRIMARY KEY CLUSTERED 
(
	[ShowTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

--
-- Table structure for table `ATT_Show`
--
CREATE TABLE [ATT_Show](
	[ShowId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[TypeId] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL DEFAULT 'Not Specified',
	[Address] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Show] PRIMARY KEY CLUSTERED 
(
	[ShowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ATT_Show]  WITH CHECK ADD  CONSTRAINT [FK_Show_ShowType] FOREIGN KEY([TypeId])
REFERENCES [ATT_ShowType] ([ShowTypeId])
GO

ALTER TABLE [ATT_Show] CHECK CONSTRAINT [FK_Show_ShowType]
GO

--
-- Table structure for table `ATT_Attendee`
--
CREATE TABLE [ATT_Attendee](
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
	[UpdateSource] [nvarchar](100) NOT NULL DEFAULT 'Not Specified',
 CONSTRAINT [PK_Attendees] PRIMARY KEY CLUSTERED 
(
	[AttendeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO



ALTER TABLE [ATT_Attendee]  WITH CHECK ADD  CONSTRAINT [FK_Attendee_Company] FOREIGN KEY([CompanyId])
REFERENCES [ATT_Company] ([CompanyId])
GO

ALTER TABLE [ATT_Attendee] CHECK CONSTRAINT [FK_Attendee_Company]
GO

ALTER TABLE [ATT_Attendee]  WITH CHECK ADD  CONSTRAINT [FK_Attendee_Show] FOREIGN KEY([ShowId])
REFERENCES [ATT_Show] ([ShowId])
GO

ALTER TABLE [ATT_Attendee] CHECK CONSTRAINT [FK_Attendee_Show]
GO

--
-- Table structure for table `ATT_EmployeeAttendee`
--

CREATE TABLE [ATT_EmployeeAttendee](
	[EmployeeAttendeeId] [int] IDENTITY(1,1) NOT NULL,
	[AttendeeId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL DEFAULT 'Not Specified',
 CONSTRAINT [PK_EmployeeAttendee] PRIMARY KEY CLUSTERED 
(
	[EmployeeAttendeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ATT_EmployeeAttendee]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeAttendee_Attendee] FOREIGN KEY([AttendeeId])
REFERENCES [ATT_Attendee] ([AttendeeId])
GO

ALTER TABLE [ATT_EmployeeAttendee] CHECK CONSTRAINT [FK_EmployeeAttendee_Attendee]
GO

ALTER TABLE [ATT_EmployeeAttendee]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeAttendee_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [Employee] ([EmployeeId])
GO

ALTER TABLE [ATT_EmployeeAttendee] CHECK CONSTRAINT [FK_EmployeeAttendee_Employee]
GO