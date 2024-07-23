USE [aspnet-StudioReservation-20240703074957]
GO

/****** Object:  Table [dbo].[MemberReservationHistory]    Script Date: 7/23/2024 11:46:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MemberReservationHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UUID] [nvarchar](50) NOT NULL,
	[TimeSlot] [nvarchar](max) NOT NULL,
	[TotalPrice] [decimal](18, 6) NOT NULL,
	[Status] [int] NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_MemberReservationHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


