USE [aspnet-StudioReservation-20240703074957]
GO

/****** Object:  Table [dbo].[Room]    Script Date: 7/18/2024 11:49:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Room](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Image] [nvarchar](max) NOT NULL,
	[Size] [nvarchar](50) NOT NULL,
	[Style] [nvarchar](max) NOT NULL,
	[Rate] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


