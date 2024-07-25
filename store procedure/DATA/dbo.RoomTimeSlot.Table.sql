USE [aspnet-StudioReservation-20240703074957]
GO
/****** Object:  Table [dbo].[RoomTimeSlot]    Script Date: 7/26/2024 1:13:22 AM ******/
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
SET IDENTITY_INSERT [dbo].[RoomTimeSlot] ON 

INSERT [dbo].[RoomTimeSlot] ([Id], [RoomId], [Date], [Times], [CreateBy], [CreateTime], [UpdateBy], [UpdateTime], [Enable]) VALUES (1, 1, CAST(N'2024-07-22' AS Date), N'24:00:00', N'', CAST(N'2024-07-22T03:51:18.147' AS DateTime), N'', CAST(N'2024-07-22T03:51:18.147' AS DateTime), 1)
INSERT [dbo].[RoomTimeSlot] ([Id], [RoomId], [Date], [Times], [CreateBy], [CreateTime], [UpdateBy], [UpdateTime], [Enable]) VALUES (2, 1, CAST(N'2024-07-10' AS Date), N'13:00:00,14:00:00,17:00:00,23:00:00,05:00:00', N'', CAST(N'2024-07-25T23:12:38.437' AS DateTime), N'', CAST(N'2024-07-25T23:12:38.437' AS DateTime), 1)
INSERT [dbo].[RoomTimeSlot] ([Id], [RoomId], [Date], [Times], [CreateBy], [CreateTime], [UpdateBy], [UpdateTime], [Enable]) VALUES (3, 1, CAST(N'2024-07-21' AS Date), N'13:00:00,14:00:00,17:00:00,23:00:00,05:00:00', N'', CAST(N'2024-07-25T23:12:38.437' AS DateTime), N'', CAST(N'2024-07-25T23:12:38.437' AS DateTime), 1)
INSERT [dbo].[RoomTimeSlot] ([Id], [RoomId], [Date], [Times], [CreateBy], [CreateTime], [UpdateBy], [UpdateTime], [Enable]) VALUES (4, 1, CAST(N'2024-07-22' AS Date), N'13:00:00,14:00:00,17:00:00,23:00:00,05:00:00', N'', CAST(N'2024-07-25T23:12:38.437' AS DateTime), N'', CAST(N'2024-07-25T23:12:38.437' AS DateTime), 1)
INSERT [dbo].[RoomTimeSlot] ([Id], [RoomId], [Date], [Times], [CreateBy], [CreateTime], [UpdateBy], [UpdateTime], [Enable]) VALUES (5, 2, CAST(N'2024-06-10' AS Date), N'13:00:00,14:00:00,07:00:00,13:00:00,05:00:00', N'', CAST(N'2024-07-25T23:12:39.800' AS DateTime), N'', CAST(N'2024-07-25T23:12:39.800' AS DateTime), 1)
INSERT [dbo].[RoomTimeSlot] ([Id], [RoomId], [Date], [Times], [CreateBy], [CreateTime], [UpdateBy], [UpdateTime], [Enable]) VALUES (6, 2, CAST(N'2024-06-21' AS Date), N'13:00:00,14:00:00,07:00:00,13:00:00,05:00:00', N'', CAST(N'2024-07-25T23:12:39.800' AS DateTime), N'', CAST(N'2024-07-25T23:12:39.800' AS DateTime), 1)
INSERT [dbo].[RoomTimeSlot] ([Id], [RoomId], [Date], [Times], [CreateBy], [CreateTime], [UpdateBy], [UpdateTime], [Enable]) VALUES (7, 2, CAST(N'2024-06-22' AS Date), N'13:00:00,14:00:00,07:00:00,13:00:00,05:00:00', N'', CAST(N'2024-07-25T23:12:39.800' AS DateTime), N'', CAST(N'2024-07-25T23:12:39.800' AS DateTime), 1)
INSERT [dbo].[RoomTimeSlot] ([Id], [RoomId], [Date], [Times], [CreateBy], [CreateTime], [UpdateBy], [UpdateTime], [Enable]) VALUES (8, 2, CAST(N'2024-07-25' AS Date), N'13:00:00,14:00:00,07:00:00,13:00:00,05:00:00', N'', CAST(N'2024-07-25T23:16:08.613' AS DateTime), N'', CAST(N'2024-07-25T23:16:08.613' AS DateTime), 1)
INSERT [dbo].[RoomTimeSlot] ([Id], [RoomId], [Date], [Times], [CreateBy], [CreateTime], [UpdateBy], [UpdateTime], [Enable]) VALUES (9, 2, CAST(N'2024-07-26' AS Date), N'13:00:00,14:00:00,07:00:00,13:00:00,05:00:00', N'', CAST(N'2024-07-25T23:16:08.613' AS DateTime), N'', CAST(N'2024-07-25T23:16:08.613' AS DateTime), 1)
INSERT [dbo].[RoomTimeSlot] ([Id], [RoomId], [Date], [Times], [CreateBy], [CreateTime], [UpdateBy], [UpdateTime], [Enable]) VALUES (10, 2, CAST(N'2024-07-28' AS Date), N'13:00:00,14:00:00,07:00:00,13:00:00,05:00:00', N'', CAST(N'2024-07-25T23:16:08.613' AS DateTime), N'', CAST(N'2024-07-25T23:16:08.613' AS DateTime), 1)
INSERT [dbo].[RoomTimeSlot] ([Id], [RoomId], [Date], [Times], [CreateBy], [CreateTime], [UpdateBy], [UpdateTime], [Enable]) VALUES (11, 2, CAST(N'2024-07-29' AS Date), N'13:00:00,14:00:00,07:00:00,13:00:00,05:00:00', N'', CAST(N'2024-07-25T23:16:08.613' AS DateTime), N'', CAST(N'2024-07-25T23:16:08.613' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[RoomTimeSlot] OFF
GO
