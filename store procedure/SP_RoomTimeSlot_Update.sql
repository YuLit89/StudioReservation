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
CREATE PROCEDURE [dbo].[RoomTimeSlot_Update] 
	-- Add the parameters for the stored procedure here
	@Id bigint ,
	@Times nvarchar(MAX),
	@UpdateBy nvarchar(128),
	@UpdateTime datetime,
	@Enable bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	update RoomTimeSlot 
	set Times = @Times,
	UpdateBy = @UpdateBy,
	UpdateTime = @UpdateTime,
	Enable = @Enable 
	where Id = @Id 
END
GO
