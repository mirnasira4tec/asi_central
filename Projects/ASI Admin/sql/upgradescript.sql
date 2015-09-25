-- 
-- Delete column in `ATT_Attendee`
--
ALTER TABLE [dbo].[ATT_Attendee] 
DROP CONSTRAINT DF__ATT_Atten__IsAtt__65370702

ALTER TABLE [dbo].[ATT_Attendee]
DROP COLUMN IsAttending
Go
