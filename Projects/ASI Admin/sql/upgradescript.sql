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
(1, 'InventoryUrlTest', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(2, 'LoginValidateUUrlTest', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(3, 'OrderCreateUrlTest', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(4, 'OrderStatusUrlTest', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(5, 'AccountNoTest', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(6, 'UserNameTest', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(7, 'PasswordTest', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(8, 'EmailTest', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(11, 'InventoryUrlProd', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(12, 'LoginValidateUUrlProd', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(13, 'OrderCreateUrlProd', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(14, 'OrderStatusUrlProd', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(15, 'AccountNoProd', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(16, 'UserNameProd', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(17, 'PasswordProd', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(18, 'EmailProd', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(31, 'Warehouse', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(32, 'WarehouseAvailable', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(33, 'CheckInventory', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(34, 'OrderNumber', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(35, 'OrderInHandsDate', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(36, 'DistributorAcknowledgementContactPhoneNumber', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN'),
(37, 'OrderLineItemViewSKU', '', GETUTCDATE(), GETUTCDATE(), 'ADMIN')
SET IDENTITY_INSERT EXCT_SupUpdateField OFF

