ALTER TABLE USR_FormType 
ADD IsDynamic bit Not Null DEFAULT 'False' WITH VALUES

SET IDENTITY_INSERT USR_FormType ON 
INSERT INTO USR_FormType (TypeId, Name,NotificationEmails,TermsAndConditions ,CreateDateUTC ,UpdateDateUTC ,UpdateSource ,IsObsolete, IsDynamic) 
VALUES (18,'Proposal Form' ,null ,null ,GETDATE() ,GETDATE() ,'Initial' ,'false' ,'true') 
SET IDENTITY_INSERT USR_FormType OFF 

CREATE TABLE USR_FormQuestion(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	FormTypeId int NOT NULL FOREIGN KEY REFERENCES USR_FormType (TypeId),
	Name nvarchar(250) NOT NULL,
	Sequence int NOT NULL,
	InputType nvarchar(100) NOT NULL,
	PlaceHolder nvarchar(250) NULL,
	Description nvarchar(500) NULL,
	ParentQuestionId int NULL,
	FollowingUpQuestions [nvarchar](250) NULL,
	IsRequired bit NOT NULL,
	IsVisible bit NOT NULL,
	ValidationRule nvarchar(150) NULL,
	ValidationMessage nvarchar(150) NULL,
	CssStyle nvarchar(150) NULL,
	CreateDateUTC datetime2(0) NOT NULL,
	UpdateDateUTC datetime2(0) NOT NULL,
	UpdateSource nvarchar(250) NOT NULL
) 

CREATE TABLE USR_FormQuestionOption(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	FormQuestionId int NOT NULL FOREIGN KEY REFERENCES USR_FormQuestion (Id), 
	Description nvarchar(500) NULL,
	Name nvarchar(500) NULL,
	Sequence int NOT NULL,
	CreateDateUTC datetime2(0) NOT NULL,
	UpdateDateUTC datetime2(0) NOT NULL,
	UpdateSource nvarchar(250) NOT NULL,
	AdditionalData nvarchar(1000) NULL
 ) 
 
 SET IDENTITY_INSERT USR_FormQuestion ON 
INSERT USR_FormQuestion (Id, FormTypeId, Name, Sequence, InputType,  PlaceHolder, Description,  ParentQuestionId, FollowingUpQuestions, 
 IsRequired, IsVisible, ValidationRule, ValidationMessage, CssStyle, CreateDateUTC, UpdateDateUTC, UpdateSource)
VALUES 
(1, 18, 'ClientName', 0, 'text', NULL, 'Client Name', NULL, NULL,  1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(2, 18, 'UploadLogo', 1, 'file', NULL, 'Upload Logo', NULL, NULL,  1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(3, 18, 'ClientType', 2, 'radio',  NULL, 'Client Type', NULL, NULL,  1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(4, 18, 'ExecutiveDirector', 3, 'dropdown',  NULL, 'Executive Director', NULL,  NULL,  1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(5, 18, 'AccountManager', 4, 'dropdown',  NULL, 'Account Manager', NULL,  NULL,  1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(6, 18, 'PlanDetails', 5, 'checkboxlist',  NULL, 'Select Plan Details:', NULL,  NULL,  1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(7, 18, 'PricingTool', 6, 'none',  NULL, 'Pricing Tool Select one or both:', NULL,  NULL,  0, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(8, 18, 'CurrentServices', 6, 'checkbox',  NULL, 'Current Services', NULL,  '9,10',  1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(9, 18, 'CurrentServicesName', 7, 'text',  NULL, 'Services for Current', NULL,  NULL,  1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(10, 18, 'CurrentServicesPrice', 8, 'text',  NULL, 'Price for Current', NULL, NULL,   1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(11, 18, 'ProposedServices', 8, 'checkbox',  NULL, 'Proposed Services', NULL,  '12,13',  1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(12, 18, 'ProposedServicesName', 8, 'text',  NULL, 'Services for Proposed', NULL, NULL,   1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
,(13, 18, 'ProposedServicesPirce', 10, 'text',  NULL, 'Price for Proposed', NULL,  NULL, 1, 1, NULL, NULL, NULL,GETDATE(), GETDATE(), 'Initail')
SET IDENTITY_INSERT USR_FormQuestion OFF 

SET IDENTITY_INSERT USR_FormQuestionOption ON 
INSERT INTO USR_FormQuestionOption
           (Id, FormQuestionId,Description,Name,Sequence,CreateDateUTC,UpdateDateUTC,UpdateSource,AdditionalData)
     VALUES
            (1,4,'Michael D’Ottaviano, Executive Director, Corporate Accounts','Michael D’Ottaviano',1, GETDATE(),GETDATE(),'Initial','/proposalTool/michael.jpg')
		   ,(2,4,'Joan Miracle, Corporate Accounts','Joan Miracle',2, GETDATE(),GETDATE(),'Initial','/proposalTool/Joan.jpg')
		   ,(4,5,'Melissa Hall Senior, Account Manager, Corporate Accounts','Melissa Hall',3, GETDATE(),GETDATE(),'Initial','/proposalTool/Melissa.jpg')
		   ,(3,5,'Jillian DiBella, Account Manager, Corporate Accounts','Jillian DiBella',4, GETDATE(),GETDATE(),'Initial','/proposalTool/Jillian.jpg')
		   ,(5,5,'Ann Gergal, Senior Account Manager, Corporate Accounts, Corporate Accounts','Ann Gergal',5, GETDATE(),GETDATE(),'Initial','/proposalTool/Ann.jpg')
		   ,(6,3,'New Client','New Client',1, GETDATE(),GETDATE(),'Initial',NULL)
		   ,(7,3,'Current Client', 'Current Client',2, GETDATE(),GETDATE(),'Initial',NULL)
		   ,(8,6,'ESP Platform','ESP Platform',1, GETDATE(),GETDATE(),'Initial',NULL)
		   ,(9,6,'Company Stores', 'Company Stores',2, GETDATE(),GETDATE(),'Initial',NULL)
		   ,(10,6,'ESP Websites', 'ESP Websites',3, GETDATE(),GETDATE(),'Initial',NULL)
		   ,(11,6,'Catalogs', 'Catalogs',4, GETDATE(),GETDATE(),'Initial',NULL)
		   
SET IDENTITY_INSERT USR_FormQuestionOption OFF 


Create Table USR_DataValue(
Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
QuestionId int NOT NULL FOREIGN KEY REFERENCES USR_FormQuestion (Id),
InstanceId int NOT NULL FOREIGN KEY REFERENCES USR_FormInstance (InstanceId),
Value nvarchar(1000) NULL,
UpdateValue nvarchar(1000) NULL,
CreateDateUTC datetime2 NOT NULL,
UpdateDateUTC datetime2 NOT NULL,
UpdateSource nvarchar(100) NOT NULL,
);

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

ALTER TABLE [dbo].[USR_FormData]  WITH CHECK ADD FOREIGN KEY([InstanceId])
REFERENCES [dbo].[USR_FormInstance] ([InstanceId])
GO

ALTER TABLE [dbo].[USR_FormData]  WITH CHECK ADD FOREIGN KEY([QuestionId])
REFERENCES [dbo].[USR_FormQuestion] ([Id])
GO
