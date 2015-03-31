﻿/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
delete from dbo.PortalMessages
delete from dbo.MessageCategories
delete from dbo.MessageTypes
GO
SET IDENTITY_INSERT [dbo].[MessageCategories] ON
INSERT INTO [dbo].[MessageCategories] ([MessageCategoryId], [CategoryName], [Element1], [Element2]) VALUES (2, N'alert-phone-blue', N'fa fa-phone bg-themeprimary white', N'fa fa-clock-o themeprimary')
INSERT INTO [dbo].[MessageCategories] ([MessageCategoryId], [CategoryName], [Element1], [Element2]) VALUES (4, N'alert-tick-red', N'fa fa-check bg-darkorange white', N'fa fa-clock-o darkorange')
INSERT INTO [dbo].[MessageCategories] ([MessageCategoryId], [CategoryName], [Element1], [Element2]) VALUES (5, N'alert-event-orange', N'fa fa-gift bg-warning white', N'fa fa-calendar warning')
SET IDENTITY_INSERT [dbo].[MessageCategories] OFF

GO
SET IDENTITY_INSERT [dbo].[MessageTypes] ON
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [TypeName], [TypeElement]) VALUES (1, N'Alert', N'pg-alert')
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [TypeName], [TypeElement]) VALUES (2, N'Mail', N'pg-mail')
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [TypeName], [TypeElement]) VALUES (3, N'Task', N'pg-task')
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [TypeName], [TypeElement]) VALUES (4, N'Chat', N'pg-chat')
SET IDENTITY_INSERT [dbo].[MessageTypes] OFF

GO
delete from [dbo].[Customers] 
SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT INTO [dbo].[Customers] ([CustomerId], [CustomerName]) VALUES (1, N'Customer1')
SET IDENTITY_INSERT [dbo].[Customers] OFF

GO
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





GO
