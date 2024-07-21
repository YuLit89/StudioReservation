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
CREATE PROCEDURE [dbo].[RoomReservationHistory_Insert]
	-- Add the parameters for the stored procedure here
  
  @RoomId int ,
  @Date Datetime,
  @Times nvarchar(50),
  @Status int,
  @ReservationBy nvarchar(128),
  @CreateTime DateTime ,
  @UpdatedTime DateTime,
  @Remark nvarchar(128),
  @Price decimal(18,0),
  @Id bigint output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into RoomReservationHistory
	(RoomId,Date,Time,Status,ReservationBy,CreateTime,UpdatedTime,Remark,Price)
	values 
	(@RoomId,@Date,@Times,@Status,@ReservationBy,@CreateTime,@UpdatedTime,@Remark,@Price)

	set @Id = SCOPE_IDENTITY()
END
GO
