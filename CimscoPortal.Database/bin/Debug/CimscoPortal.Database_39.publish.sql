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
PRINT N'Altering [dbo].[InvoiceSummaries]...';


GO
ALTER TABLE [dbo].[InvoiceSummaries] ALTER COLUMN [TotalEnergyCharges] NUMERIC (14, 2) NOT NULL;

ALTER TABLE [dbo].[InvoiceSummaries] ALTER COLUMN [TotalMiscCharges] NUMERIC (12, 2) NOT NULL;


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
delete from dbo.InvoiceSummaries


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
/****** Script for SelectTopNRows command from SSMS  ******/
use CimscoNZ
GO

insert into  [CimscoPortal].[dbo].[InvoiceSummaries]  
SELECT        Invoices.InvoiceId, Invoices.InvoiceDate, Invoices.InvoiceNumber, Invoices.GstTotal, Invoices.InvoiceTotal, Invoices.AccountNumber, Invoices.CustomerNumber, Invoices.SiteId, Invoices.NetworkChargesTotal, 
                         Invoices.EnergyChargesTotal, Invoices.MiscChargesTotal, Invoices.TotalCharges, Invoices.GSTCharges, InvoicesElectricityNetworkCharges.TotalNetworkCharges, 
                         InvoicesElectricityMiscCharges.TotalMiscCharges, InvoicesElectricityCharges.TotalEnergyCharges, EnergyPoints.ConnectionNumber, Sites.SiteName, EnergyPoints.EnergyPointId
FROM            Invoices INNER JOIN
                         InvoicesElectricityCharges ON Invoices.InvoiceId = InvoicesElectricityCharges.InvoiceId INNER JOIN
                         InvoicesElectricityMiscCharges ON Invoices.InvoiceId = InvoicesElectricityMiscCharges.InvoiceId AND Invoices.InvoiceId = InvoicesElectricityMiscCharges.InvoiceId INNER JOIN
                         InvoicesElectricityNetworkCharges ON Invoices.InvoiceId = InvoicesElectricityNetworkCharges.InvoiceId AND Invoices.InvoiceId = InvoicesElectricityNetworkCharges.InvoiceId INNER JOIN
                         EnergyPoints ON InvoicesElectricityCharges.EnergyPointId = EnergyPoints.EnergyPointId INNER JOIN
                         Sites ON Invoices.SiteId = Sites.SiteId
GO


GO

GO
PRINT N'Update complete.';


GO
