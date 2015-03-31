/*
SET IDENTITY_INSERT [dbo].[PortalMessages] ON
INSERT INTO [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [MessageCategoryId], [Message], [MessageTypeId]) 
	VALUES (1, 
			(select CustomerId from dbo.Customers where CustomerName = 'Customer1'), 
			(select MessageCategoryId from dbo.MessageCategories where CategoryName = 'alert-phone-blue'), 
			N'Test phone message', 
			(select MessageTypeId from dbo.MessageTypes where [TypeName] = 'Alert')
			)
INSERT INTO [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [MessageCategoryId], [Message], [MessageTypeId]) 
	VALUES (2, 
			(select CustomerId from dbo.Customers where CustomerName = 'Customer1'), 
			(select MessageCategoryId from dbo.MessageCategories where CategoryName = 'alert-tick-red'), 
			N'Test tick message', 
			(select MessageTypeId from dbo.MessageTypes where [TypeName] = 'Alert')
			)
SET IDENTITY_INSERT [dbo].[PortalMessages] OFF
*/
SET IDENTITY_INSERT [dbo].[PortalMessages] ON
INSERT INTO [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [MessageCategoryId], [Message], [MessageTypeId], [MessageFormatId], [TimeStamp], [Footer], [ExpiryDate]) VALUES (1, 1, 2, N'Test phone message', 1, NULL, N'2015-03-25 02:30:00', N'Test footer', N'2015-03-01 00:00:00')
INSERT INTO [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [MessageCategoryId], [Message], [MessageTypeId], [MessageFormatId], [TimeStamp], [Footer], [ExpiryDate]) VALUES (2, 1, 4, N'Test tick message', 1, NULL, N'2015-03-25 03:45:00', N'Test footer2', N'2015-04-01 00:00:00')
SET IDENTITY_INSERT [dbo].[PortalMessages] OFF




