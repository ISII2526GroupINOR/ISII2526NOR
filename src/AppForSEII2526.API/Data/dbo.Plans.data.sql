SET IDENTITY_INSERT [dbo].[Plans] ON
INSERT INTO [dbo].[Plans] ([Id], [Name], [Description], [CreatedDate], [HealthIssues], [TotalPrice], [Weeks], [PaymentMethodId], [ApplicationUserId]) VALUES (1, N'MyBoxingPlan', N'Plan for boxing', N'2025-10-29 00:00:00', NULL, CAST(650.00 AS Decimal(5, 2)), 1, 1, N'2')
INSERT INTO [dbo].[Plans] ([Id], [Name], [Description], [CreatedDate], [HealthIssues], [TotalPrice], [Weeks], [PaymentMethodId], [ApplicationUserId]) VALUES (2, N'MyYogaPlan', N'Plan for yoga', N'2025-10-29 00:00:00', N'Nut allergy', CAST(200.00 AS Decimal(5, 2)), 2, 2, N'3')
SET IDENTITY_INSERT [dbo].[Plans] OFF
