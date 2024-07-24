USE [aspnet-StudioReservation-20240703074957]
GO
/****** Object:  StoredProcedure [dbo].[RoomReservationHistory_Insert]    Script Date: 7/23/2024 11:34:30 PM ******/
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
  @Status int,
  @ReservationBy nvarchar(128),
  @CreateTime DateTime ,
  @UpdatedTime DateTime,
  @BookingId nvarchar(50),
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
	(RoomId,Date,Status,ReservationBy,CreateTime,UpdatedTime,BookingId,Remark,Price)
	values 
	(@RoomId,@Date,@Status,@ReservationBy,@CreateTime,@UpdatedTime,@BookingId,@Remark,@Price)

	set @Id = SCOPE_IDENTITY()
END
