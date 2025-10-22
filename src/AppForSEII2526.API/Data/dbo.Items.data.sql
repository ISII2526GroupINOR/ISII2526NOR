SET IDENTITY_INSERT [dbo].[Brands] ON
INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (1, N'Rogue Fitness')
INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (2, N'Life Fitness')
INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (3, N'Precor')
INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (4, N'Cybex')
SET IDENTITY_INSERT [dbo].[Brands] OFF

SET IDENTITY_INSERT [dbo].[TypeItems] ON
INSERT INTO [dbo].[TypeItems] ([Id], [Name], [ClassId]) VALUES (1, N'Dumbbell', NULL)
INSERT INTO [dbo].[TypeItems] ([Id], [Name], [ClassId]) VALUES (2, N'Band', NULL)
INSERT INTO [dbo].[TypeItems] ([Id], [Name], [ClassId]) VALUES (3, N'Machine', NULL)
SET IDENTITY_INSERT [dbo].[TypeItems] OFF

SET IDENTITY_INSERT [dbo].[Items] ON
INSERT INTO [dbo].[Items] ([Id], [Name], [Description], [PurchasePrice], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [BrandId], [TypeItemId]) VALUES (1, N'10 Kg Dumbbell', N'A dumbbell', CAST(20.00 AS Decimal(5, 2)), 10, 4, CAST(15.00 AS Decimal(5, 2)), 1, 1)
INSERT INTO [dbo].[Items] ([Id], [Name], [Description], [PurchasePrice], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [BrandId], [TypeItemId]) VALUES (2, N'8 Kg Kettlebell', N'Circular dumbbell with a handle on top ', CAST(30.00 AS Decimal(5, 2)), 4, 1, CAST(25.00 AS Decimal(5, 2)), 2, 1)
INSERT INTO [dbo].[Items] ([Id], [Name], [Description], [PurchasePrice], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [BrandId], [TypeItemId]) VALUES (3, N'Resistance Band', N'Band for exercising', CAST(10.00 AS Decimal(5, 2)), 8, 12, CAST(7.00 AS Decimal(5, 2)), 1, 2)
INSERT INTO [dbo].[Items] ([Id], [Name], [Description], [PurchasePrice], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [BrandId], [TypeItemId]) VALUES (4, N'Lat Pulldown Machine', N'Machine for exercising lats and back', CAST(180.00 AS Decimal(5, 2)), 8, 2, CAST(150.00 AS Decimal(5, 2)), 3, 3)
INSERT INTO [dbo].[Items] ([Id], [Name], [Description], [PurchasePrice], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [BrandId], [TypeItemId]) VALUES (5, N'Chest Press Machine', N'For exercising chest in an upright position', CAST(240.00 AS Decimal(5, 2)), 5, 1, CAST(200.00 AS Decimal(5, 2)), 4, 3)
INSERT INTO [dbo].[Items] ([Id], [Name], [Description], [PurchasePrice], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [BrandId], [TypeItemId]) VALUES (6, N'15 Kg Dumbbell', N'A regular dumbbell', CAST(25.00 AS Decimal(5, 2)), 16, 4, CAST(20.00 AS Decimal(5, 2)), 3, 1)
INSERT INTO [dbo].[Items] ([Id], [Name], [Description], [PurchasePrice], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [BrandId], [TypeItemId]) VALUES (7, N'16 Kg Kettlebell', N'Circular dumbbell with handle upon ', CAST(35.00 AS Decimal(5, 2)), 6, 9, CAST(28.00 AS Decimal(5, 2)), 4, 1)
INSERT INTO [dbo].[Items] ([Id], [Name], [Description], [PurchasePrice], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [BrandId], [TypeItemId]) VALUES (8, N'20 Kg Dumbbell', N'A dumbell', CAST(40.00 AS Decimal(5, 2)), 4, 8, CAST(33.00 AS Decimal(5, 2)), 2, 1)
INSERT INTO [dbo].[Items] ([Id], [Name], [Description], [PurchasePrice], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [BrandId], [TypeItemId]) VALUES (9, N'20 Kg Kettlebell', N'Circular dumbbell', CAST(50.00 AS Decimal(5, 2)), 2, 5, CAST(40.00 AS Decimal(5, 2)), 3, 1)
SET IDENTITY_INSERT [dbo].[Items] OFF
