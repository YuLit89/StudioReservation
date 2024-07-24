USE [aspnet-StudioReservation-20240703074957]
GO

/****** Object:  Table [dbo].[RoomTimeSlot_1]    Script Date: 7/20/2024 2:23:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoomTimeSlot](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RoomId] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[Times] [nvarchar](max) NULL,
	[CreateBy] [nvarchar](128) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [nvarchar](128) NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_RoomTimeSlot] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


