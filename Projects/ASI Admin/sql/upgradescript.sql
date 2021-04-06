Alter Table ATT_EmployeeAttendee add HasReviewedGuidelines bit null
  INSERT INTO [dbo].[ATT_ProfileOptionalDataLabel] VALUES  
			('SellingProposition','What is your unique selling proposition?'
           ,GETUTCDATE(),GETUTCDATE(),'Sql Query',0,1,0),
		   ('Prop65Compliant','Is your company Prop 65 compliant?'
           ,GETUTCDATE(),GETUTCDATE(),'Sql Query',0,1,0),
		   ('LoyaltyProgramOffer','Do you offer a loyalty program?'
           ,GETUTCDATE(),GETUTCDATE(),'Sql Query',0,1,0)
		   
update [ATT_ProfileOptionalDataLabel] set IsObsolete=1 where ProfileOptionalDataLabelId in (8,9)