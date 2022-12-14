USE [Umbraco]
GO
ALTER TABLE USR_FormType 
ADD IsDynamic bit Not Null DEFAULT 'False' WITH VALUES

SET IDENTITY_INSERT USR_FormType ON 
INSERT INTO USR_FormType (TypeId, Name,NotificationEmails,TermsAndConditions ,CreateDateUTC ,UpdateDateUTC ,UpdateSource ,IsObsolete, IsDynamic) 
VALUES (18,'Proposal Form' ,null ,null ,GETDATE() ,GETDATE() ,'Initial' ,'false' ,'true') 
SET IDENTITY_INSERT USR_FormType OFF 
Go
/****** Object:  Table [dbo].[USR_FormData]    Script Date: 25-05-2021 10:56:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USR_FormData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NOT NULL,
	[InstanceId] [int] NOT NULL,
	[Value] [nvarchar](1000) NULL,
	[UpdateValue] [nvarchar](1000) NULL,
	[CreateDateUTC] [datetime2](7) NOT NULL,
	[UpdateDateUTC] [datetime2](7) NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USR_FormQuestion]    Script Date: 25-05-2021 10:56:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USR_FormQuestion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FormTypeId] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Sequence] [int] NOT NULL,
	[InputType] [nvarchar](100) NOT NULL,
	[PlaceHolder] [nvarchar](250) NULL,
	[Description] [nvarchar](500) NULL,
	[ParentQuestionId] [int] NULL,
	[FollowingUpQuestions] [nvarchar](250) NULL,
	[IsRequired] [bit] NOT NULL,
	[IsVisible] [bit] NOT NULL,
	[ValidationRule] [nvarchar](150) NULL,
	[ValidationMessage] [nvarchar](150) NULL,
	[CreateDateUTC] [datetime2](0) NOT NULL,
	[UpdateDateUTC] [datetime2](0) NOT NULL,
	[UpdateSource] [nvarchar](250) NOT NULL,
	[CssStyle] [nvarchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USR_FormQuestionOption]    Script Date: 25-05-2021 10:56:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USR_FormQuestionOption](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FormQuestionId] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Name] [nvarchar](500) NULL,
	[Sequence] [int] NOT NULL,
	[AdditionalData] [nvarchar](1000) NULL,
	[CreateDateUTC] [datetime2](0) NOT NULL,
	[UpdateDateUTC] [datetime2](0) NOT NULL,
	[UpdateSource] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK__USR_Form__69CBEBB5A3065B88] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[USR_FormQuestion] ON 

INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (1, 18, N'ClientName', 0, N'text', NULL, N'Client Name', NULL, NULL, 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (2, 18, N'UploadLogo', 1, N'file', NULL, N'Upload Logo', NULL, NULL, 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (3, 18, N'ClientType', 2, N'radio', NULL, N'Client Type', NULL, NULL, 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (4, 18, N'ExecutiveDirector', 3, N'dropdown', NULL, N'Executive Director', NULL, NULL, 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (5, 18, N'AccountManager', 4, N'dropdown', NULL, N'Account Manager', NULL, NULL, 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (6, 18, N'PlanDetails', 5, N'checkboxlist', NULL, N'Select Plan Details:', NULL, NULL, 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (7, 18, N'PricingTool', 6, N'none', NULL, N'Pricing Tool Select one or both:', NULL, NULL, 0, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (8, 18, N'CurrentServices', 6, N'checkbox', NULL, N'Current Services', NULL, N'9,10', 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (9, 18, N'CurrentServicesName', 7, N'text', NULL, N'Services for Current', 8, NULL, 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (10, 18, N'CurrentServicesPrice', 8, N'text', NULL, N'Price for Current', 8, NULL, 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (11, 18, N'ProposedServices', 8, N'checkbox', NULL, N'Proposed Services', NULL, N'12,13', 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (12, 18, N'ProposedServicesName', 8, N'text', NULL, N'Services for Proposed', 11, NULL, 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (13, 18, N'ProposedServicesPirce', 10, N'text', NULL, N'Price for Proposed', 11, NULL, 1, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) VALUES (14, 18, N'SourceFile', 11, N'sourceFile', NULL, N'Source File', NULL, NULL, 0, 1, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
INSERT [dbo].[USR_FormQuestion] ([Id], [FormTypeId], [Name], [Sequence], [InputType], [PlaceHolder], [Description], [ParentQuestionId], [FollowingUpQuestions], [IsRequired], [IsVisible], [ValidationRule], [ValidationMessage], [CreateDateUTC], [UpdateDateUTC], [UpdateSource], [CssStyle]) 
VALUES (14, 18, N'SourceFile', 11, N'sourceFile', NULL, N'Generated word doc', NULL, NULL, 0, 0, NULL, NULL, CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), CAST(N'2021-04-28T08:22:35.0000000' AS DateTime2), N'Initail', NULL)
SET IDENTITY_INSERT [dbo].[USR_FormQuestion] OFF

SET IDENTITY_INSERT [dbo].[USR_FormQuestionOption] ON 

INSERT [dbo].[USR_FormQuestionOption] ([Id], [FormQuestionId], [Description], [Name], [Sequence], [AdditionalData], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (1, 4, N'Michael D’Ottaviano, Executive Director, Corporate Accounts', N'Michael D’Ottaviano', 1, N'michael.jpg;michael_sign.jpg', CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), N'Initial')
INSERT [dbo].[USR_FormQuestionOption] ([Id], [FormQuestionId], [Description], [Name], [Sequence], [AdditionalData], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (2, 4, N'Joan Miracle, Corporate Accounts', N'Joan Miracle', 2, N'joan.jpg;joan_sign.jpg', CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), N'Initial')
INSERT [dbo].[USR_FormQuestionOption] ([Id], [FormQuestionId], [Description], [Name], [Sequence], [AdditionalData], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (3, 5, N'Jillian DiBella, Account Manager, Corporate Accounts', N'Jillian DiBella', 4, N'jillian.jpg', CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), N'Initial')
INSERT [dbo].[USR_FormQuestionOption] ([Id], [FormQuestionId], [Description], [Name], [Sequence], [AdditionalData], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (4, 5, N'Melissa Hall Senior, Account Manager, Corporate Accounts', N'Melissa Hall', 3, N'hall.jpg', CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), N'Initial')
INSERT [dbo].[USR_FormQuestionOption] ([Id], [FormQuestionId], [Description], [Name], [Sequence], [AdditionalData], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (5, 5, N'Ann Gergal, Senior Account Manager, Corporate Accounts, Corporate Accounts', N'Ann Gergal', 5, N'ann.jpg', CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), N'Initial')
INSERT [dbo].[USR_FormQuestionOption] ([Id], [FormQuestionId], [Description], [Name], [Sequence], [AdditionalData], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (6, 3, N'New Client', N'New Client', 1, NULL, CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), N'Initial')
INSERT [dbo].[USR_FormQuestionOption] ([Id], [FormQuestionId], [Description], [Name], [Sequence], [AdditionalData], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (7, 3, N'Current Client', N'Current Client', 2, NULL, CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), N'Initial')
INSERT [dbo].[USR_FormQuestionOption] ([Id], [FormQuestionId], [Description], [Name], [Sequence], [AdditionalData], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (8, 6, N'ESP Platform', N'ESP Platform', 1, NULL, CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), N'Initial')
INSERT [dbo].[USR_FormQuestionOption] ([Id], [FormQuestionId], [Description], [Name], [Sequence], [AdditionalData], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (9, 6, N'Company Stores', N'Company Stores', 2, NULL, CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), N'Initial')
INSERT [dbo].[USR_FormQuestionOption] ([Id], [FormQuestionId], [Description], [Name], [Sequence], [AdditionalData], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (10, 6, N'ESP Websites', N'ESP Websites', 3, NULL, CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), N'Initial')
INSERT [dbo].[USR_FormQuestionOption] ([Id], [FormQuestionId], [Description], [Name], [Sequence], [AdditionalData], [CreateDateUTC], [UpdateDateUTC], [UpdateSource]) VALUES (11, 6, N'Catalogs', N'Catalogs', 4, NULL, CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), CAST(N'2021-04-28T08:28:08.0000000' AS DateTime2), N'Initial')
SET IDENTITY_INSERT [dbo].[USR_FormQuestionOption] OFF
ALTER TABLE [dbo].[USR_FormData]  WITH CHECK ADD FOREIGN KEY([InstanceId])
REFERENCES [dbo].[USR_FormInstance] ([InstanceId])
GO
ALTER TABLE [dbo].[USR_FormData]  WITH CHECK ADD FOREIGN KEY([QuestionId])
REFERENCES [dbo].[USR_FormQuestion] ([Id])
GO
ALTER TABLE [dbo].[USR_FormQuestion]  WITH CHECK ADD FOREIGN KEY([FormTypeId])
REFERENCES [dbo].[USR_FormType] ([TypeId])
GO
ALTER TABLE [dbo].[USR_FormQuestionOption]  WITH CHECK ADD  CONSTRAINT [FK__USR_FormQ__FormQ__7DCDAAA2] FOREIGN KEY([FormQuestionId])
REFERENCES [dbo].[USR_FormQuestion] ([Id])
GO
ALTER TABLE [dbo].[USR_FormQuestionOption] CHECK CONSTRAINT [FK__USR_FormQ__FormQ__7DCDAAA2]
GO
