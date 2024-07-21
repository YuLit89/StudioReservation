USE [aspnet-StudioReservation-20240703074957]
GO

/****** Object:  Table [dbo].[UserReservationStatus]    Script Date: 7/18/2024 11:54:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoomReservationHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RoomId] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[Time] [nvarchar](50) NOT NULL,
	[Status] [int] NOT NULL,
	[ReservationBy] [nvarchar](128) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdatedTime] [datetime] NOT NULL,
	[Remark] [nvarchar](128) NULL,
	[Price] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_RoomReservationHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


