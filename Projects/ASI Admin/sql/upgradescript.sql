
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EXCT_SupUpdateField](
	[SupUpdateFieldId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](250) NULL DEFAULT (NULL),
	[CreateDateUTC] [datetime2](0) NOT NULL,
	[UpdateDateUTC] [datetime2](0) NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL DEFAULT (N'Not Specified'),
	[IsObsolete] [bit] NULL DEFAULT ((0)),
 CONSTRAINT [PK_EXCT_SupUpdateField_SupUpdateFieldId] PRIMARY KEY CLUSTERED 
(
	[SupUpdateFieldId] ASC
))

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EXCT_SupUpdateRequest](
	[SupUpdateRequestId] [int] IDENTITY(26,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CompanyName] [nvarchar](250) NULL DEFAULT (NULL),
	[RequestedBy] [nvarchar](250) NOT NULL,
	[ApprovedBy] [nvarchar](250) NULL DEFAULT (NULL),
	[Status] [int] NULL DEFAULT (NULL),
	[CreateDateUTC] [datetime2](0) NOT NULL,
	[UpdateDateUTC] [datetime2](0) NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL DEFAULT (N'Not Specified'),
 CONSTRAINT [PK_EXCT_SupUpdateRequest_SupUpdateRequestId] PRIMARY KEY CLUSTERED 
(
	[SupUpdateRequestId] ASC
))
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EXCT_SupUpdateRequestDetail](
	[SupUpdateRequestDetailId] [int] IDENTITY(150,1) NOT NULL,
	[SupUpdateRequestId] [int] NOT NULL,
	[SupUpdateFieldId] [int] NOT NULL,
	[UpdateValue] [nvarchar](500) NULL DEFAULT (NULL),
	[OrigValue] [nvarchar](500) NULL DEFAULT (NULL),	
	[CreateDateUTC] [datetime2](0) NOT NULL,
	[UpdateDateUTC] [datetime2](0) NOT NULL,
	[UpdateSource] [nvarchar](100) NOT NULL DEFAULT (N'Not Specified'),
 CONSTRAINT [PK_EXCT_SupUpdateRequestDetail_SupUpdateRequestDetailId] PRIMARY KEY CLUSTERED 
(
	[SupUpdateRequestDetailId] ASC
),
 CONSTRAINT [EXCT_SupUpdateRequestDetail$Field_UNIQUE] UNIQUE NONCLUSTERED 
(
	[SupUpdateRequestId] ASC,
	[SupUpdateFieldId] ASC
))
GO

ALTER TABLE [dbo].[EXCT_SupUpdateRequestDetail]  WITH NOCHECK ADD  CONSTRAINT [EXCT_SupUpdateRequestDetail$FK_EXCT_SupUpdateRequest] FOREIGN KEY([SupUpdateRequestId])
REFERENCES [dbo].[EXCT_SupUpdateRequest] ([SupUpdateRequestId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EXCT_SupUpdateRequestDetail] CHECK CONSTRAINT [EXCT_SupUpdateRequestDetail$FK_EXCT_SupUpdateRequest]
GO
ALTER TABLE [dbo].[EXCT_SupUpdateRequestDetail]  WITH NOCHECK ADD  CONSTRAINT [EXCT_SupUpdateRequestDetail$FK_EXCT_SupUpdateField] FOREIGN KEY([SupUpdateFieldId])
REFERENCES [dbo].[EXCT_SupUpdateField] ([SupUpdateFieldId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EXCT_SupUpdateRequestDetail] CHECK CONSTRAINT [EXCT_SupUpdateRequestDetail$FK_EXCT_SupUpdateField]
GO

SET IDENTITY_INSERT EXCT_SupUpdateField ON
INSERT INTO EXCT_SupUpdateField (SupUpdateFieldId, Name, Description, CreateDateUTC, UpdateDateUTC, UpdateSource)
VALUES
(1, 'InventoryUrlTest', 'Inventory Url (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(2, 'LoginValidateUrlTest', 'Login Validate Url (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(3, 'OrderCreateUrlTest', 'Order Create Url (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(4, 'OrderStatusUrlTest', 'Order Status Url (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(5, 'AccountNoTest', 'Account Number (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(6, 'UserNameTest', 'User Name (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(7, 'PasswordTest', 'Password (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(8, 'EmailTest', 'Email (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(11, 'InventoryUrlProd', 'Inventory Url (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(12, 'LoginValidateUrlProd', 'Login Validate Url (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(13, 'OrderCreateUrlProd', 'Order Create Url (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(14, 'OrderStatusUrlProd', 'Order Status Url (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(15, 'AccountNoProd', 'Account Number (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(16, 'UserNameProd', 'User Name (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(17, 'PasswordProd', 'Password (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(18, 'EmailProd', 'Email (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(31, 'WarehouseProd', 'Ware House (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(32, 'WarehouseAvailableProd', 'Ware House Available (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(33, 'CheckInventoryProd', 'Check Inventory (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(34, 'OrderNumberProd', 'Order Number (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(35, 'OrderInHandsDateProd', 'Order In Hands Date (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(36, 'DistributorAcknowledgementContactPhoneNumberProd', 'Distributor Phone Number (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(37, 'OrderLineItemViewSKUProd', 'Order Line Item View SKU (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(38, 'LoginInstructionTest', 'Login Instruction (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(39, 'LoginInstructionProd', 'Login Instruction (Prod)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(40, 'WarehouseTest', 'Ware House (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(41, 'WarehouseAvailableTest', 'Ware House Available (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(42, 'CheckInventoryTest', 'Check Inventory (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(43, 'OrderNumberTest', 'Order Number (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(44, 'OrderInHandsDateTest', 'Order In Hands Date (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(45, 'DistributorAcknowledgementContactPhoneNumberTest', 'Distributor Phone Number (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(46, 'OrderLineItemViewSKUTest', 'Order Line Item View SKU (Test)', GETUTCDATE(), GETUTCDATE(), 'ADMIN')
SET IDENTITY_INSERT EXCT_SupUpdateField OFF

