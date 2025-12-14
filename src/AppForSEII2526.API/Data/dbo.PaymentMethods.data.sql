SET IDENTITY_INSERT [dbo].[PaymentMethods] ON
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (1, N'2', N'CreditCard', NULL, N'4111111111111111', N'2026-05-31 00:00:00', NULL)
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (2, N'3', N'CreditCard', NULL, N'5500000000000004', N'2027-10-31 00:00:00', NULL)
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (3, N'0debecae-5326-49a7-b269-7618013b565c', N'CreditCard', NULL, N'5500000000000004', N'2027-10-31 00:00:00', NULL)
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF
