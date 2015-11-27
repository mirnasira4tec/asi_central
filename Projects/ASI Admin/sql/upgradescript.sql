-- 
-- Allowing null for Email in ATT_Employee
--
ALTER TABLE [dbo].[ATT_Employee] 
ALTER COLUMN Email nvarchar(50) null
Go