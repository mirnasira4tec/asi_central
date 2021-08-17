USE [Umbraco_Show]
GO
/****** Object:  Table [dbo].[AttendeeSchedule]    Script Date: 2/10/2021 6:57:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATT_AttendeeSchedule](
	[AttendeeScheduleId] [int] IDENTITY(1,1) NOT NULL,
	[SupplierAttendeeId] [int] NOT NULL,
	[DistributorAttendeeId] [int] NOT NULL,
	[ShowScheduleDetailId] [int] NOT NULL,
	[Team] [int] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [varchar](128) NULL,	
 CONSTRAINT [PK_AttendeeSchedule] PRIMARY KEY CLUSTERED 
(
	[AttendeeScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[ATT_ShowSchedule]    Script Date: 2/15/2021 7:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ATT_ShowSchedule](
	[ShowScheduleId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[Duration] [int] NOT NULL,
	[Breaks] [varchar](500) NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [varchar](128) NULL,
 CONSTRAINT [PK_ShowSchedule] PRIMARY KEY CLUSTERED 
(
	[ShowScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ATT_ShowScheduleDetail]    Script Date: 2/15/2021 7:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ATT_ShowScheduleDetail](
	[ShowScheduleDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ShowScheduleId] [int] NOT NULL,
	[Day] [int] NOT NULL,
	[TimeSchedule] [varchar](250) NOT NULL,
	[IsBreak] [bit] NOT NULL,
	[Sequence] [int] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[UpdateDateUTC] [datetime] NOT NULL,
	[UpdateSource] [varchar](128) NULL,
 CONSTRAINT [PK_ShowScheduleDetail] PRIMARY KEY CLUSTERED 
(
	[ShowScheduleDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ATT_ShowScheduleDetail] ADD  CONSTRAINT [DF_ShowScheduleDetail_IsBreak]  DEFAULT ((0)) FOR [IsBreak]
GO
ALTER TABLE [dbo].[ATT_AttendeeSchedule]  WITH CHECK ADD  CONSTRAINT [FK_AttendeeSchedule_ATT_Attendee] FOREIGN KEY([DistributorAttendeeId])
REFERENCES [dbo].[ATT_Attendee] ([AttendeeId])
GO
ALTER TABLE [dbo].[ATT_AttendeeSchedule] CHECK CONSTRAINT [FK_AttendeeSchedule_ATT_Attendee]
GO
ALTER TABLE [dbo].[ATT_AttendeeSchedule]  WITH CHECK ADD  CONSTRAINT [FK_AttendeeSchedule_ATT_Attendee1] FOREIGN KEY([SupplierAttendeeId])
REFERENCES [dbo].[ATT_Attendee] ([AttendeeId])
GO
ALTER TABLE [dbo].[ATT_AttendeeSchedule] CHECK CONSTRAINT [FK_AttendeeSchedule_ATT_Attendee1]
GO
ALTER TABLE [dbo].[ATT_AttendeeSchedule]  WITH CHECK ADD  CONSTRAINT [FK_AttendeeSchedule_ShowScheduleDetail] FOREIGN KEY([ShowScheduleDetailId])
REFERENCES [dbo].[ATT_ShowScheduleDetail] ([ShowScheduleDetailId])
GO
ALTER TABLE [dbo].[ATT_AttendeeSchedule] CHECK CONSTRAINT [FK_AttendeeSchedule_ShowScheduleDetail]
GO
ALTER TABLE [dbo].[ATT_ShowScheduleDetail]  WITH CHECK ADD  CONSTRAINT [FK_ShowScheduleDetail_ShowSchedule] FOREIGN KEY([ShowScheduleId])
REFERENCES [dbo].[ATT_ShowSchedule] ([ShowScheduleId])
GO
ALTER TABLE [dbo].[ATT_AttendeeSchedule] ADD  CONSTRAINT [DF_ATT_AttendeeSchedule_Team]  DEFAULT ((1)) FOR [Team]
GO
ALTER TABLE [dbo].[ATT_ShowScheduleDetail] CHECK CONSTRAINT [FK_ShowScheduleDetail_ShowSchedule]
GO
