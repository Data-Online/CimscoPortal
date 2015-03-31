delete from [dbo].[Customers] 
SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT INTO [dbo].[Customers] ([CustomerId], [CustomerName]) VALUES (1, N'Customer1')
SET IDENTITY_INSERT [dbo].[Customers] OFF
