ALTER TABLE [STOR_Order]
ADD IsNotificationSent bit NOT NULL 
CONSTRAINT STOR_Order_IsNotificationSent_Default DEFAULT (0)
WITH VALUES 