USE [Umbraco]
GO
/****** Object:  Table [dbo].[USR_RateSupplierForm]    Script Date: 2/28/2019 7:50:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USR_RateSupplierForm](
	[RateSupplierFormId] [int] IDENTITY(1,1) NOT NULL,
	[RateSupplierImportId] [int] NULL,
	[DistASINum] [nvarchar](10) NOT NULL,
	[DistCompanyName] [nvarchar](1000) NOT NULL,
	[DistFax] [nvarchar](50) NULL,
	[DistPhone] [nvarchar](50) NULL,
	[SubmitBy] [nvarchar](1000) NULL,
	[SubmitDateUTC] [datetime] NULL,
	[SubmitSuccessful] [bit] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL,
	[SubmitName] [varchar](250) NULL,
	[SubmitEmail] [varchar](250) NULL,
	[IPAddress] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[RateSupplierFormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USR_RateSupplierFormDetail]    Script Date: 2/28/2019 7:50:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USR_RateSupplierFormDetail](
	[RateSupplierFormDetailId] [int] IDENTITY(1,1) NOT NULL,
	[RateSupplierFormId] [int] NULL,
	[SupASINum] [nvarchar](10) NOT NULL,
	[SupCompanyName] [nvarchar](1000) NOT NULL,
	[NumOfTransImport] [int] NULL,
	[NumOfTransSubmit] [int] NULL,
	[OverallRating] [int] NULL,
	[ProdQualityRating] [int] NULL,
	[CommunicationRating] [int] NULL,
	[DeliveryRating] [int] NULL,
	[ImprintingRating] [int] NULL,
	[ProbResolutionRating] [int] NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL,
	[SubmitSuccessful] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RateSupplierFormDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[USR_RateSupplierImport]    Script Date: 2/28/2019 7:50:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USR_RateSupplierImport](
	[RateSupplierImportId] [int] IDENTITY(1,1) NOT NULL,
	[LastUpdatedBy] [nvarchar](500) NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL,
	[NumberOfImports] [int] NOT NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[RateSupplierImportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[USR_RateSupplierForm] ADD  CONSTRAINT [DF_USR_RateSupplierForm_SubmitSuccessful_1]  DEFAULT ((0)) FOR [SubmitSuccessful]
GO

ALTER TABLE [dbo].[USR_RateSupplierFormDetail] ADD  CONSTRAINT [DF_USR_RateSupplierFormDetail_SubmitSuccessful]  DEFAULT ((0)) FOR [SubmitSuccessful]
GO
ALTER TABLE [dbo].[USR_RateSupplierImport] ADD  DEFAULT ((1)) FOR [NumberOfImports]
GO
ALTER TABLE [dbo].[USR_RateSupplierForm]  WITH CHECK ADD FOREIGN KEY([RateSupplierImportId])
REFERENCES [dbo].[USR_RateSupplierImport] ([RateSupplierImportId])
GO
ALTER TABLE [dbo].[USR_RateSupplierFormDetail]  WITH CHECK ADD FOREIGN KEY([RateSupplierFormId])
REFERENCES [dbo].[USR_RateSupplierForm] ([RateSupplierFormId])
GO
