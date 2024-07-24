USE [aspnet-StudioReservation-20240703074957]
GO

/****** Object:  StoredProcedure [dbo].[RoomTimeSlot_GetAll]    Script Date: 7/19/2024 1:49:05 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RoomTimeSlot_GetAll]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from RoomTimeSlot
END
GO


