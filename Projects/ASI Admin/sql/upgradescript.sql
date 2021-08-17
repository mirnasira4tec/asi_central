ALTER TABLE [SHW_FormQuestion]
    ADD IsAdminOnly bit Not NULL 
    CONSTRAINT DF__SHW_FormQ__IsAdminOnly DEFAULT 0
    WITH VALUES;
	
	
	SET IDENTITY_INSERT [dbo].[SHW_FormQuestion] ON 
INSERT INTO [dbo].[SHW_FormQuestion]
           ([FormQuestionId],[FormTypeId],[QuestionName],[Sequence],[InputType],[Category]
           ,[Description],[ShortDescription],[IsYesNoQUestion],[IsRequired],[IsVisible]
           ,[CreateDateUTC],[UpdateDateUTC],[UpdateSource],[ParentQuestionId],[IsAdminOnly])
     VALUES
           (21,1,'ESPproductionLink',40,'Text','Product'
           ,'ESP production link','ESP production link:',0,0,1
		   ,GETUTCDATE(),GETUTCDATE(),'Initial Script',NULL,1)
GO
SET IDENTITY_INSERT [dbo].[SHW_FormQuestion] OFF 


-------------------  ASIEVENT-147  Automate FASI attendee schedule generation -----------------------------


Alter Table ATT_EmployeeAttendee add Team int CONSTRAINT ATT_EmployeeAttendee_Team Default 1 with VALUES

Alter TABLE ATT_Attendee ADD Suite int Null;

Alter TABLE [dbo].[ATT_Show] ADD ShowScheduleId int null;

ALTER TABLE [dbo].[ATT_Show]  WITH CHECK ADD  CONSTRAINT [FK_Show_ShowSchedule] FOREIGN KEY([ShowScheduleId])
REFERENCES [dbo].[ATT_ShowSchedule] ([ShowScheduleId])
GO

--------------------------- ASIEVENT-147 - End -------------------------------------------------------
