
-- 
-- Added new column in `ATT_Attendee`
--
ALTER TABLE [dbo].[ATT_Attendee]
ADD  [IsPresentation] [bit] NOT NULL DEFAULT ('0'),[IsRoundTable] [bit] NOT NULL DEFAULT ('0')
Go