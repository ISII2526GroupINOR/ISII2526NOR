SET IDENTITY_INSERT [dbo].[Restocks] ON
INSERT INTO [dbo].[Restocks] ([Id], [Title], [DeliveryAddress], [Description], [ExpectedDate], [RestockDate], [TotalPrice], [RestockResponsibleId]) VALUES (1, N'First Restock', N'Avd España 45', N'A restock', N'2025-12-04 00:00:00', N'2025-12-02 00:00:00', NULL, N'1')
INSERT INTO [dbo].[Restocks] ([Id], [Title], [DeliveryAddress], [Description], [ExpectedDate], [RestockDate], [TotalPrice], [RestockResponsibleId]) VALUES (2, N'Second Restock', N'C/a Ancha 23', N'New restock for a gym', N'2025-12-08 00:00:00', N'2025-12-07 00:00:00', CAST(42.00 AS Decimal(5, 2)), N'1')
INSERT INTO [dbo].[Restocks] ([Id], [Title], [DeliveryAddress], [Description], [ExpectedDate], [RestockDate], [TotalPrice], [RestockResponsibleId]) VALUES (3, N'Third Restock', N'C/a Nueva 34', N'Other restock', N'2025-12-08 00:00:00', N'2025-12-07 00:00:00', NULL, N'1')
SET IDENTITY_INSERT [dbo].[Restocks] OFF

INSERT INTO [dbo].[RestockItems] ([ItemId], [RestockId], [Quantity], [RestockPrice]) VALUES (1, 1, 4, CAST(60.00 AS Decimal(5, 2)))
INSERT INTO [dbo].[RestockItems] ([ItemId], [RestockId], [Quantity], [RestockPrice]) VALUES (2, 1, 3, NULL)
INSERT INTO [dbo].[RestockItems] ([ItemId], [RestockId], [Quantity], [RestockPrice]) VALUES (3, 2, 6, CAST(42.00 AS Decimal(5, 2)))
INSERT INTO [dbo].[RestockItems] ([ItemId], [RestockId], [Quantity], [RestockPrice]) VALUES (4, 3, 2, NULL)