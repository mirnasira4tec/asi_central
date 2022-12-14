SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[SP_INSERTCOLORMAPPING]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [SP_INSERTCOLORMAPPING]
GO
/******************************************************************************************
-- Procedure: dbo.SP_INSERTCOLORMAPPING
--
-- Developer:	Ameer Khan
-- Application:	ASI Admin Application
--
-- Parameters:	
--		@CompanyId: Supplier Id.
--		@SupplierColor: Color provided by supplier.
--		@ColorGroup: Velocity color.
--
-- Description:	Insert the color mapping details.
-- Creation Date: 06March2017
-- Returns: Mapped color identity.
--
--Sample execution:
--	exec dbo.SP_INSERTCOLORMAPPING 1,'Rich Blue Heather', 'Medium Blue'
--
--
-- Modification Log:
-- Date			Developer			Comments
-- -------		----------------	----------------
--
--
******************************************************************************************/

CREATE PROCEDURE [dbo].[SP_INSERTCOLORMAPPING]
(
	@CompanyId int,
	@SupplierColor varchar(100),
	@ColorGroup varchar(1500),
	@UserId bigint = NULL
)
as

begin

 SET NOCOUNT ON

--*******************************************************************
-- Operational section
--*******************************************************************
  declare 	@$$prog varchar(30), @$$errno int,  
			@$$errmsg varchar(1000),  @$$proc_section_nm varchar(30),
			@$$error_group_cd char(4), @$$row_cnt INT, @$$error_db_name varchar(50),
			@$$CreateUserName varchar(128) 	,		-- last user changed the data 
          	@$$CreateMachineName varchar(128) 	,	-- last machine changes-procedure were run from
          	@$$CreateSource varchar(128),			-- last process that made a changes
			@$$severitylevel int	,			-- error severety level. Usually 16. But developer may override (be very carefull)
			@$$err int	, 				-- define general number for logical error 
			@$$errline int,  @$$errstate int,  @$$errseverity int;

-- additional for non trans proc if a wrapper for transactional
declare @$$tran_flag int, @$$tran_flag_err int;

-- Initialize operational variables
      set @$$CreateUserName = suser_sname()
      set @$$CreateMachineName = host_name()
      set @$$CreateSource = 'App=(' + rtrim(isnull(APP_NAME(), '')) + ') Proc=(' + isnull(object_name(@@procid), '') + ')'

  select 	@$$errno = 0,  @$$errmsg = NULL, @$$proc_section_nm = NULL, @$$prog = LEFT(object_name(@@procid),30),
			@$$error_group_cd = null,		-- assign group code or process group code
			@$$row_cnt = NULL, @$$error_db_name = db_name(), @$$severitylevel = 16, @$$err = 999999999;

  set @$$tran_flag = @@trancount;

--************************************************************************
-- User Declaration and initialization section
-- All variables should be declared at the beginning of the proc
--**********************************************************************

 

--**********************************************************************
-- User Source code
--**********************************************************************
--=========
BEGIN TRY
--=========
if ( @CompanyId is not null and  @SupplierColor <> '' and @ColorGroup <> '' )
begin
	if not exists(select 1 from dbo.CUST_CriteriaValueMapping where CodeValueGroup_CD = isnull((select CodeValueGroup_CD from PROD_Master.dbo.LOOK_CodeValueGroup where Descr = @SupplierColor and CriteriaType_CD = 'COLR'),'$2HM') and Code_Value = @ColorGroup and Company_ID = @CompanyId)
	begin
		Insert into dbo.CUST_CriteriaValueMapping(CriteriaType_CD, CodeValueGroup_CD, Code_Value, Descr, Display_NM, CreateDateUTC, UpdateDateUTC, UpdateSource, AuditStatus_CD, Company_ID, IP_Address, SignOn_ID, Custom_Value_CD)
		select
		CriteriaType_CD = 'COLR',
		CodeValueGroup_CD = isnull((select CodeValueGroup_CD from PROD_Master.dbo.LOOK_CodeValueGroup where Descr = @SupplierColor and CriteriaType_CD = 'COLR'),'$2HM'),
		Code_Value = @ColorGroup,
		Descr = null,
		Display_NM = null,
		CreateDateUTC = GETUTCDATE(),
		UpdateDateUTC = null,
		UpdateSource = suser_name(),
		AuditStatus_CD = 'A',
		Company_ID = @CompanyId,
		IP_Address = null,
		SignOn_ID = null,
		Custom_Value_CD = null

		select cast(scope_identity() as bigint)
	end
	else
	begin
		select cast(0 as bigint)
	end
end
--========
END TRY
--========
--***********************************************************************
--===========
BEGIN CATCH
--===========
set @$$tran_flag_err = @@trancount;

-- if outputs result from a transactional proc that was not commited or rollbacked
IF (@$$tran_flag = 0 and @$$tran_flag_err > 0 )
		rollback;

IF @$$errno = 0 
	set @$$errno = ISNULL( NULLIF( ERROR_NUMBER(), 0) ,@$$err);

	IF (@$$errno > 888000000 ) -- logical application error
		begin
			select @$$errmsg =  ISNULL(ERRM_Description,'')  + '; ' + ISNULL(@$$errmsg,'')
			from opr_support.dbo.LOOK_ErrorMessage_ERRM
			where ERRM_Id = @$$errno;
		end	
	
    	set @$$errseverity = ERROR_SEVERITY();
    	set @$$errstate = ERROR_STATE();
    	set @$$prog = ISNULL(@$$prog, ERROR_PROCEDURE() );
    	set @$$errline = ERROR_LINE();
    	set @$$errmsg =  left('UserID = ' + ISNULL(Cast(@UserId as varchar),'NULL') + '; User Msg: ' + ltrim(IsNull(@$$errmsg,'')) + '; Sys Msg: ' + ERROR_MESSAGE(), 1000);

  	set @$$errmsg = 'Error inproc ' + isnull(@$$prog,' ') + ' ' + isnull(@$$errmsg,' ');

	IF (@$$errno > 888000000 and @$$errno <> 999999999) -- logical application error
		begin
  			RAISERROR (@$$errno, @$$severitylevel, 1, @$$errmsg );
		end
	ELSE
		begin
			raiserror (@$$errmsg, @$$severitylevel, 1);
		end

--======================================================
-- insert an error 
--======================================================
        EXEC dbo.SP_ERROR_LOG_2005
          	@CreateUserName  = @$$CreateUserName,
          	@CreateMachineName = @$$CreateMachineName,
          	@CreateSource = @$$CreateSource,
          	@ERROR_LOG_PROGRAM_NM = @$$prog,
         	@ERROR_LOG_PROGRAM_SECTION_NM = @$$proc_section_nm,
          	@ERROR_LOG_ERROR_NO = @$$errno,
          	@ERROR_LOG_ERROR_DSC = @$$errmsg,
			@error_group_cd = @$$error_group_cd,
			@error_db_name = @$$error_db_name,
			@ERROR_LINE_NO = @$$errline ,
			@ERROR_SEVERITY_NO = @$$errseverity ,
			@ERROR_STATE_NO = @$$errstate

--===========
END CATCH
--===========

--*******************************************************************************
-- Logical error should have next line if required raise error and exit proc
/*
	
	IF <logical condition>
	    begin
		set @$$errno = @$$err;   
		set @$$errmsg = '<MESSAGE> ';
		set @$$proc_section_nm = 'Section:  <section ##>';
		raiserror (@$$errmsg, @$$severitylevel, 1);
	   end
*/
--*******************************************************************************
  SET NOCOUNT OFF

  return @$$errno

end
--===================================================================
-- End of procedure: SP_INSERTCOLORMAPPING
--===================================================================