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

GO
INSERT [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [Message], [MessageFormatId], [TimeStamp], [Footer], [ExpiryDate]) VALUES (1, 1, N'Test phone message', 2, CAST(0x0000A466002932E0 AS DateTime), N'Test footer', CAST(0x0000A44E00000000 AS DateTime))
GO
INSERT [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [Message], [MessageFormatId], [TimeStamp], [Footer], [ExpiryDate]) VALUES (2, 1, N'Test tick message', 2, CAST(0x0000A466003DCC50 AS DateTime), N'Test footer2', CAST(0x0000A46D00000000 AS DateTime))
GO
INSERT [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [Message], [MessageFormatId], [TimeStamp], [Footer], [ExpiryDate]) VALUES (3, 1, N'Blue phone', 1, CAST(0x0000A48B00000000 AS DateTime), N'With footer', CAST(0x0000A4A600000000 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[PortalMessages] OFF
GO


