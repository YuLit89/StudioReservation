-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RoomReservationHistory_Update]
	-- Add the parameters for the stored procedure here
	
	 @Id bigint,
	 @Status int ,
	 @UpdatedTime DateTime ,
	 @Remark nvarchar(128)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update RoomReservationHistory 
	set Status = @Status ,
	    UpdatedTime = @UpdatedTime,
		Remark = @Remark 
	where Id = @Id
END
GO
