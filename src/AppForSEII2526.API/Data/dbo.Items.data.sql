SET IDENTITY_INSERT [dbo].[Items] ON
INSERT INTO [dbo].[Items] ([Id], [Name], [Description], [PurchasePrice], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [BrandId], [TypeItemId]) VALUES (2, N'5Kg Dumbell', N'A dumbell', CAST(50.00 AS Decimal(5, 2)), 8, 2, CAST(40.00 AS Decimal(5, 2)), 1, 1)
SET IDENTITY_INSERT [dbo].[Items] OFF

SET IDENTITY_INSERT [dbo].[Brands] ON
INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (1, N'Adidas')
SET IDENTITY_INSERT [dbo].[Brands] OFF

SET IDENTITY_INSERT [dbo].[TypeItems] ON
INSERT INTO [dbo].[TypeItems] ([Id], [Name], [ClassId]) VALUES (1, N'Dumbell', NULL)
SET IDENTITY_INSERT [dbo].[TypeItems] OFF