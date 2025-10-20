SET IDENTITY_INSERT [dbo].[Classes] ON
INSERT INTO [dbo].[Classes] ([Id], [Name], [Date], [Capacity], [Price]) VALUES (1, N'Introduction to Boxing', N'2025-11-01 10:00:00', 35, CAST(250.00 AS Decimal(5, 2)))
INSERT INTO [dbo].[Classes] ([Id], [Name], [Date], [Capacity], [Price]) VALUES (2, N'Intermediate Boxing', N'2025-11-01 10:00:00', 30, CAST(400.00 AS Decimal(5, 2)))
INSERT INTO [dbo].[Classes] ([Id], [Name], [Date], [Capacity], [Price]) VALUES (3, N'Advanced Boxing', N'2025-11-01 10:00:00', 20, CAST(600.00 AS Decimal(5, 2)))
SET IDENTITY_INSERT [dbo].[Classes] OFF
