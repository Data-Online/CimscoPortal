﻿/*
Deployment script for CimscoPortal

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "CimscoPortal"
:setvar DefaultFilePrefix "CimscoPortal"
:setvar DefaultDataPath "c:\Program Files\Microsoft SQL Server\MSSQL11.SQLSERVER2012\MSSQL\DATA\"
:setvar DefaultLogPath "c:\Program Files\Microsoft SQL Server\MSSQL11.SQLSERVER2012\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
/*
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
INSERT INTO [dbo].[MessageCategories] ([MessageCategoryId], [Category]) VALUES (2, N'alert_phone_blue')
INSERT INTO [dbo].[MessageCategories] ([MessageCategoryId], [Category]) VALUES (4, N'alert_tick_red')
SET IDENTITY_INSERT [dbo].[MessageCategories] OFF

GO
SET IDENTITY_INSERT [dbo].[MessageTypes] ON
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [Type]) VALUES (1, N'Alert')
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [Type]) VALUES (2, N'Mail')
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [Type]) VALUES (3, N'Task')
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [Type]) VALUES (4, N'Chat')
SET IDENTITY_INSERT [dbo].[MessageTypes] OFF

GO
delete from [dbo].[Customers] 
SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT INTO [dbo].[Customers] ([CustomerId], [CustomerName]) VALUES (1, N'Customer1')
SET IDENTITY_INSERT [dbo].[Customers] OFF

GO
SET IDENTITY_INSERT [dbo].[PortalMessages] ON
INSERT INTO [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [MessageCategoryId], [Text], [MessageTypeId]) 
	VALUES (1, 
			(select CustomerId from dbo.Customers where CustomerName = 'Customer1'), 
			(select MessageCategoryId from dbo.MessageCategories where Category = 'alert_phone_blue'), 
			N'Test phone message', 
			(select MessageTypeId from dbo.MessageTypes where [Type] = 'Alert')
			)
INSERT INTO [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [MessageCategoryId], [Text], [MessageTypeId]) 
	VALUES (2, 
			(select CustomerId from dbo.Customers where CustomerName = 'Customer1'), 
			(select MessageCategoryId from dbo.MessageCategories where Category = 'alert_tick_red'), 
			N'Test tick message', 
			(select MessageTypeId from dbo.MessageTypes where [Type] = 'Alert')
			)
SET IDENTITY_INSERT [dbo].[PortalMessages] OFF

GO

GO
PRINT N'Update complete.';


GO
