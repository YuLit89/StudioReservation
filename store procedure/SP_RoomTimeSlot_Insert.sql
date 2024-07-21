USE [aspnet-StudioReservation-20240703074957]
GO
/****** Object:  StoredProcedure [dbo].[RoomTimeSlot_Insert]    Script Date: 7/20/2024 2:23:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[RoomTimeSlot_Insert]
	-- Add the parameters for the stored procedure here
	@RoomId int ,
	@Date date,
	@Times nvarchar(MAX),
	@CreateBy nvarchar(128),
	@CreateTime datetime,
	@UpdateBy nvarchar(128),
	@UpdateTime datetime,
	@Enable bit,
	@Id bigint output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into RoomTimeSlot
	(RoomId,Date,Times,CreateBy,CreateTime,UpdateBy,UpdateTime,Enable)
	values 
	(@RoomId,@Date,@Times,@CreateBy,@CreateTime,@UpdateBy,@UpdateTime,@Enable)

	set @Id = SCOPE_IDENTITY()
END
