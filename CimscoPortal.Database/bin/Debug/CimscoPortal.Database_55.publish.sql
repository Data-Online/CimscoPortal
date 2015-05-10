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
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                RECOVERY FULL 
            WITH ROLLBACK IMMEDIATE;
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CLOSE OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE,
                DISABLE_BROKER 
            WITH ROLLBACK IMMEDIATE;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Rename refactoring operation with key 9bfb47b0-d5a6-44e6-96df-e204a848fda7, c07bdb09-54bb-4914-80f1-9757ed247fcb, 266a0ed9-6076-461c-9dd9-2f6d5c713b13 is skipped, element [dbo].[MessageTypes].[MessageType] (SqlSimpleColumn) will not be renamed to TypeName';


GO
PRINT N'Rename refactoring operation with key dd816c77-6f5a-4805-ac7d-649ee2f9ba6f is skipped, element [dbo].[PortalMessages].[MessageTypeId] (SqlSimpleColumn) will not be renamed to MessageCategoryId';


GO
PRINT N'Rename refactoring operation with key 4b1c1485-aaf0-449a-82d4-9fdd02567558, 404c309d-1520-4850-a4a0-5f2614f91adb is skipped, element [dbo].[MessageCategories].[Id] (SqlSimpleColumn) will not be renamed to MessageCategoryId';


GO
PRINT N'Rename refactoring operation with key 1648c6af-d16f-4600-a31e-70de6d907acc, 55b8f9e9-fc36-446a-a377-6250f4b289ab, 4eb1ee66-848c-424b-9c68-c63eb6c4187a, 5487428a-f4f6-4024-83c9-6d9f92454f24 is skipped, element [dbo].[MessageCategories].[MessageCategoryy] (SqlSimpleColumn) will not be renamed to CategoryName';


GO
PRINT N'Rename refactoring operation with key ec87f5fa-e7ed-4a37-9f71-9d048af80d0b is skipped, element [dbo].[MessageTypes].[Id] (SqlSimpleColumn) will not be renamed to MessageTypeId';


GO
PRINT N'Rename refactoring operation with key dab1295a-d6f9-4657-a4b2-5240e634dfd0 is skipped, element [dbo].[Customers].[Id] (SqlSimpleColumn) will not be renamed to CustomerId';


GO
PRINT N'Rename refactoring operation with key 7e859cea-f22d-4e98-b839-2e9838aaa99f is skipped, element [dbo].[PortalMessages].[MessageText] (SqlSimpleColumn) will not be renamed to Message';


GO
PRINT N'Rename refactoring operation with key 8b7a3875-5260-4933-be07-c62d780f2c75 is skipped, element [dbo].[MessageFormats].[Id] (SqlSimpleColumn) will not be renamed to MessageFormatId';


GO
PRINT N'Rename refactoring operation with key 5c9539a8-f852-44b9-b48e-ca3c2d8b03a6 is skipped, element [dbo].[PortalMessages].[ExpriyDate] (SqlSimpleColumn) will not be renamed to ExpiryDate';


GO
PRINT N'Rename refactoring operation with key 002b2ff0-b908-4be3-bbda-76f84e2649ed is skipped, element [dbo].[ElectricityBillTotals].[Id] (SqlSimpleColumn) will not be renamed to ElectricityBillTotalId';


GO
PRINT N'Rename refactoring operation with key 7803ea87-eeca-4201-a1b0-4f14dd57ec52 is skipped, element [dbo].[ElectricityBillTotals].[InvoiceDate] (SqlSimpleColumn) will not be renamed to InvoiceId';


GO
PRINT N'Rename refactoring operation with key 770b909d-6cd7-43a7-b71b-7416a3608c69 is skipped, element [dbo].[ElectricityBillTotals].[EnergyCharg] (SqlSimpleColumn) will not be renamed to EnergyCharge';


GO
PRINT N'Rename refactoring operation with key 31b91379-4a3e-412f-a2d8-879695bfa668 is skipped, element [dbo].[EnergyPoints].[Id] (SqlSimpleColumn) will not be renamed to EnergyPointId';


GO
PRINT N'Rename refactoring operation with key 6d2775aa-d2f7-4916-a4a9-88ee05c12cb3 is skipped, element [dbo].[InvoiceSummaries].[Net] (SqlSimpleColumn) will not be renamed to NetworkChargesTotal';


GO
PRINT N'Rename refactoring operation with key aa39e07b-e3d6-45b2-8983-0aee8e367ee0 is skipped, element [dbo].[MessageTypes].[MessageType] (SqlSimpleColumn) will not be renamed to Description';


GO
PRINT N'Rename refactoring operation with key ff8e2d26-7248-4414-81b8-1cc6646208f3 is skipped, element [dbo].[MessageTypes].[MessageElement] (SqlSimpleColumn) will not be renamed to PageElement';


GO
PRINT N'Creating [dbo].[Customers]...';


GO
CREATE TABLE [dbo].[Customers] (
    [CustomerId]   INT            IDENTITY (1, 1) NOT NULL,
    [CustomerName] NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([CustomerId] ASC)
);


GO
PRINT N'Creating [dbo].[InvoiceSummaries]...';


GO
CREATE TABLE [dbo].[InvoiceSummaries] (
    [InvoiceId]           INT             NOT NULL,
    [InvoiceDate]         DATE            NOT NULL,
    [InvoiceNumber]       VARCHAR (20)    NOT NULL,
    [GstTotal]            DECIMAL (8, 2)  NOT NULL,
    [InvoiceTotal]        DECIMAL (10, 2) NOT NULL,
    [AccountNumber]       NVARCHAR (12)   NOT NULL,
    [CustomerNumber]      NVARCHAR (12)   NOT NULL,
    [SiteId]              INT             NOT NULL,
    [NetworkChargesTotal] DECIMAL (10, 2) NOT NULL,
    [EnergyChargesTotal]  DECIMAL (10, 2) NOT NULL,
    [MiscChargesTotal]    DECIMAL (10, 2) NOT NULL,
    [TotalCharges]        DECIMAL (8, 2)  NOT NULL,
    [GSTCharges]          DECIMAL (8, 2)  NOT NULL,
    [TotalNetworkCharges] NUMERIC (12, 2) NOT NULL,
    [TotalMiscCharges]    NUMERIC (12, 2) NOT NULL,
    [TotalEnergyCharges]  NUMERIC (14, 2) NOT NULL,
    [ConnectionNumber]    NVARCHAR (30)   NOT NULL,
    [SiteName]            NVARCHAR (50)   NOT NULL,
    [EnergyPointId]       INT             NOT NULL,
    [InvoiceSummaryId]    INT             IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_InvoiceSummaries] PRIMARY KEY CLUSTERED ([InvoiceSummaryId] ASC)
) ON [PRIMARY];


GO
PRINT N'Creating [dbo].[MessageFormats]...';


GO
CREATE TABLE [dbo].[MessageFormats] (
    [MessageFormatId] INT           IDENTITY (1, 1) NOT NULL,
    [MessageTypeId]   INT           NULL,
    [Element1]        NVARCHAR (50) NULL,
    [Element2]        NVARCHAR (50) NULL,
    [DisplayFormat]   NVARCHAR (50) NULL,
    CONSTRAINT [PK__MessageT__9BA1E2BAAD5F02E6] PRIMARY KEY CLUSTERED ([MessageFormatId] ASC) ON [PRIMARY]
) ON [PRIMARY];


GO
PRINT N'Creating [dbo].[MessageTypes]...';


GO
CREATE TABLE [dbo].[MessageTypes] (
    [MessageTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [Description]   NVARCHAR (20) NULL,
    [PageElement]   NVARCHAR (20) NULL,
    CONSTRAINT [PK_MessageTypes] PRIMARY KEY CLUSTERED ([MessageTypeId] ASC) ON [PRIMARY]
) ON [PRIMARY];


GO
PRINT N'Creating [dbo].[PortalMessages]...';


GO
CREATE TABLE [dbo].[PortalMessages] (
    [PortalMessageId] INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]      INT            NULL,
    [Message]         NVARCHAR (100) NULL,
    [MessageFormatId] INT            NULL,
    [TimeStamp]       DATETIME       NULL,
    [Footer]          NVARCHAR (100) NULL,
    [ExpiryDate]      DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([PortalMessageId] ASC) ON [PRIMARY]
) ON [PRIMARY];


GO
PRINT N'Creating [dbo].[FK_MessageFormats_MessageTypes]...';


GO
ALTER TABLE [dbo].[MessageFormats] WITH NOCHECK
    ADD CONSTRAINT [FK_MessageFormats_MessageTypes] FOREIGN KEY ([MessageTypeId]) REFERENCES [dbo].[MessageTypes] ([MessageTypeId]);


GO
PRINT N'Creating [dbo].[FK_PortalMessages_Customers]...';


GO
ALTER TABLE [dbo].[PortalMessages] WITH NOCHECK
    ADD CONSTRAINT [FK_PortalMessages_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([CustomerId]);


GO
PRINT N'Creating [dbo].[FK_PortalMessages_MessageTypes]...';


GO
ALTER TABLE [dbo].[PortalMessages] WITH NOCHECK
    ADD CONSTRAINT [FK_PortalMessages_MessageTypes] FOREIGN KEY ([MessageFormatId]) REFERENCES [dbo].[MessageFormats] ([MessageFormatId]);


GO
-- Refactoring step to update target server with deployed transaction logs

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'dd816c77-6f5a-4805-ac7d-649ee2f9ba6f')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('dd816c77-6f5a-4805-ac7d-649ee2f9ba6f')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '4b1c1485-aaf0-449a-82d4-9fdd02567558')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('4b1c1485-aaf0-449a-82d4-9fdd02567558')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '404c309d-1520-4850-a4a0-5f2614f91adb')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('404c309d-1520-4850-a4a0-5f2614f91adb')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '1648c6af-d16f-4600-a31e-70de6d907acc')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('1648c6af-d16f-4600-a31e-70de6d907acc')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'ec87f5fa-e7ed-4a37-9f71-9d048af80d0b')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('ec87f5fa-e7ed-4a37-9f71-9d048af80d0b')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'dab1295a-d6f9-4657-a4b2-5240e634dfd0')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('dab1295a-d6f9-4657-a4b2-5240e634dfd0')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '9bfb47b0-d5a6-44e6-96df-e204a848fda7')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('9bfb47b0-d5a6-44e6-96df-e204a848fda7')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '55b8f9e9-fc36-446a-a377-6250f4b289ab')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('55b8f9e9-fc36-446a-a377-6250f4b289ab')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '4eb1ee66-848c-424b-9c68-c63eb6c4187a')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('4eb1ee66-848c-424b-9c68-c63eb6c4187a')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'c07bdb09-54bb-4914-80f1-9757ed247fcb')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('c07bdb09-54bb-4914-80f1-9757ed247fcb')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '7e859cea-f22d-4e98-b839-2e9838aaa99f')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('7e859cea-f22d-4e98-b839-2e9838aaa99f')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '266a0ed9-6076-461c-9dd9-2f6d5c713b13')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('266a0ed9-6076-461c-9dd9-2f6d5c713b13')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '5487428a-f4f6-4024-83c9-6d9f92454f24')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('5487428a-f4f6-4024-83c9-6d9f92454f24')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '8b7a3875-5260-4933-be07-c62d780f2c75')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('8b7a3875-5260-4933-be07-c62d780f2c75')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '5c9539a8-f852-44b9-b48e-ca3c2d8b03a6')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('5c9539a8-f852-44b9-b48e-ca3c2d8b03a6')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '002b2ff0-b908-4be3-bbda-76f84e2649ed')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('002b2ff0-b908-4be3-bbda-76f84e2649ed')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '7803ea87-eeca-4201-a1b0-4f14dd57ec52')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('7803ea87-eeca-4201-a1b0-4f14dd57ec52')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '770b909d-6cd7-43a7-b71b-7416a3608c69')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('770b909d-6cd7-43a7-b71b-7416a3608c69')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '31b91379-4a3e-412f-a2d8-879695bfa668')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('31b91379-4a3e-412f-a2d8-879695bfa668')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '6d2775aa-d2f7-4916-a4a9-88ee05c12cb3')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('6d2775aa-d2f7-4916-a4a9-88ee05c12cb3')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'aa39e07b-e3d6-45b2-8983-0aee8e367ee0')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('aa39e07b-e3d6-45b2-8983-0aee8e367ee0')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'ff8e2d26-7248-4414-81b8-1cc6646208f3')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('ff8e2d26-7248-4414-81b8-1cc6646208f3')

GO

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
delete from dbo.MessageTypes
delete from dbo.MessageFormats
delete from dbo.InvoiceSummaries


GO
SET IDENTITY_INSERT [dbo].[MessageTypes] ON 

GO
INSERT [dbo].[MessageTypes] ([MessageTypeId], [Description], [PageElement]) VALUES (1, N'Alert', N'pg-alert')
GO
INSERT [dbo].[MessageTypes] ([MessageTypeId], [Description], [PageElement]) VALUES (2, N'Note', N'pg-note')
GO
INSERT [dbo].[MessageTypes] ([MessageTypeId], [Description], [PageElement]) VALUES (3, N'ToDo', N'pg-todo')
GO
SET IDENTITY_INSERT [dbo].[MessageTypes] OFF
GO
GO
SET IDENTITY_INSERT [dbo].[MessageFormats] ON 
GO
INSERT [dbo].[MessageFormats] ([MessageFormatId], [MessageTypeId], [Element1], [Element2], [DisplayFormat]) VALUES (1, 1, N'fa fa-phone bg-themeprimary white', N'fa fa-clock-o themeprimary', N'Phone Blue')
INSERT [dbo].[MessageFormats] ([MessageFormatId], [MessageTypeId], [Element1], [Element2], [DisplayFormat]) VALUES (2, 1, N'fa fa-check bg-darkorange white', N'fa fa-clock-o darkorange', N'Tick Red')
INSERT [dbo].[MessageFormats] ([MessageFormatId], [MessageTypeId], [Element1], [Element2], [DisplayFormat]) VALUES (3, 1, N'fa fa-gift bg-warning white', N'fa fa-gift bg-warning white', N'Event Orange')
INSERT [dbo].[MessageFormats] ([MessageFormatId], [MessageTypeId], [Element1], [Element2], [DisplayFormat]) VALUES (4, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[MessageFormats] OFF
GO
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

GO
INSERT [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [Message], [MessageFormatId], [TimeStamp], [Footer], [ExpiryDate]) VALUES (1, 1, N'Test phone message', 2, CAST(0x0000A466002932E0 AS DateTime), N'Test footer', CAST(0x0000A44E00000000 AS DateTime))
GO
INSERT [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [Message], [MessageFormatId], [TimeStamp], [Footer], [ExpiryDate]) VALUES (2, 1, N'Test tick message', 2, CAST(0x0000A466003DCC50 AS DateTime), N'Test footer2', CAST(0x0000A46D00000000 AS DateTime))
GO
INSERT [dbo].[PortalMessages] ([PortalMessageId], [CustomerId], [Message], [MessageFormatId], [TimeStamp], [Footer], [ExpiryDate]) VALUES (3, 1, N'Blue phone', 1, CAST(0x0000A48B00000000 AS DateTime), N'With footer', CAST(0x0000A4A600000000 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[PortalMessages] OFF
GO



GO
SET IDENTITY_INSERT [dbo].[InvoiceSummaries] ON
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (20, N'2011-02-01', N'01022011', CAST(3512.00 AS Decimal(8, 2)), CAST(26931.00 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(4887.00 AS Decimal(10, 2)), CAST(14462.00 AS Decimal(10, 2)), CAST(786.00 AS Decimal(10, 2)), CAST(23418.00 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(5307.00 AS Decimal(12, 2)), CAST(705.00 AS Decimal(12, 2)), CAST(17406.00 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 16)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (21, N'2010-04-07', N'336815', CAST(2885.62 AS Decimal(8, 2)), CAST(25970.61 AS Decimal(10, 2)), N'8807926410', N'400175020', 2, CAST(4633.29 AS Decimal(10, 2)), CAST(17781.00 AS Decimal(10, 2)), CAST(670.00 AS Decimal(10, 2)), CAST(23084.99 AS Decimal(8, 2)), CAST(2885.62 AS Decimal(8, 2)), CAST(4633.29 AS Decimal(12, 2)), CAST(670.00 AS Decimal(12, 2)), CAST(17781.45 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 17)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (22, N'2011-03-01', N'01032011', CAST(3020.00 AS Decimal(8, 2)), CAST(23155.00 AS Decimal(10, 2)), N'Unknown', N'50419690', 2, CAST(4887.00 AS Decimal(10, 2)), CAST(14462.00 AS Decimal(10, 2)), CAST(749.00 AS Decimal(10, 2)), CAST(20135.00 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(4887.00 AS Decimal(12, 2)), CAST(749.00 AS Decimal(12, 2)), CAST(14462.00 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 18)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (23, N'2010-02-01', N'01022010', CAST(2779.00 AS Decimal(8, 2)), CAST(25008.00 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(4950.00 AS Decimal(10, 2)), CAST(16604.00 AS Decimal(10, 2)), CAST(675.00 AS Decimal(10, 2)), CAST(22229.00 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(4950.00 AS Decimal(12, 2)), CAST(675.00 AS Decimal(12, 2)), CAST(16604.19 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 19)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (24, N'2011-07-05', N'656845', CAST(2190.02 AS Decimal(8, 2)), CAST(16790.13 AS Decimal(10, 2)), N'2707108410', N'Unknown', 1, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(14600.11 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(3457.22 AS Decimal(12, 2)), CAST(174.50 AS Decimal(12, 2)), CAST(10968.39 AS Decimal(14, 2)), N'0001452560UNB21', N'Site not set', 4, 20)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (25, N'2010-03-01', N'01032010', CAST(2595.47 AS Decimal(8, 2)), CAST(23359.19 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(4851.00 AS Decimal(10, 2)), CAST(15302.55 AS Decimal(10, 2)), CAST(641.00 AS Decimal(10, 2)), CAST(20763.72 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(4851.00 AS Decimal(12, 2)), CAST(641.00 AS Decimal(12, 2)), CAST(15302.55 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 21)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (26, N'2010-05-01', N'01052010', CAST(0.00 AS Decimal(8, 2)), CAST(0.00 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(4601.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(603.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(4601.00 AS Decimal(12, 2)), CAST(603.00 AS Decimal(12, 2)), CAST(0.00 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 22)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (27, N'2010-06-01', N'01062010', CAST(3452.00 AS Decimal(8, 2)), CAST(31067.00 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(4609.00 AS Decimal(10, 2)), CAST(22340.66 AS Decimal(10, 2)), CAST(665.00 AS Decimal(10, 2)), CAST(27615.00 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(4609.00 AS Decimal(12, 2)), CAST(665.00 AS Decimal(12, 2)), CAST(22340.66 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 23)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (28, N'2010-07-01', N'01072010', CAST(3429.00 AS Decimal(8, 2)), CAST(30862.00 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(4655.00 AS Decimal(10, 2)), CAST(22135.97 AS Decimal(10, 2)), CAST(642.00 AS Decimal(10, 2)), CAST(27433.00 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(4655.00 AS Decimal(12, 2)), CAST(642.00 AS Decimal(12, 2)), CAST(22135.97 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 24)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (29, N'2010-08-01', N'01082010', CAST(3427.84 AS Decimal(8, 2)), CAST(30850.57 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(4660.00 AS Decimal(10, 2)), CAST(22135.97 AS Decimal(10, 2)), CAST(654.00 AS Decimal(10, 2)), CAST(27422.73 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(4660.00 AS Decimal(12, 2)), CAST(654.00 AS Decimal(12, 2)), CAST(22135.97 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 25)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (30, N'2010-09-01', N'01092010', CAST(3202.00 AS Decimal(8, 2)), CAST(28815.00 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(4635.00 AS Decimal(10, 2)), CAST(20320.40 AS Decimal(10, 2)), CAST(658.00 AS Decimal(10, 2)), CAST(25613.00 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(4635.00 AS Decimal(12, 2)), CAST(658.00 AS Decimal(12, 2)), CAST(20320.40 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 26)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (31, N'2010-10-01', N'01102010', CAST(2961.00 AS Decimal(8, 2)), CAST(26645.00 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(4569.00 AS Decimal(10, 2)), CAST(18473.26 AS Decimal(10, 2)), CAST(642.00 AS Decimal(10, 2)), CAST(23685.00 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(4569.00 AS Decimal(12, 2)), CAST(642.00 AS Decimal(12, 2)), CAST(18473.26 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 27)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (32, N'2010-11-01', N'01112010', CAST(3338.46 AS Decimal(8, 2)), CAST(25594.83 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(4660.00 AS Decimal(10, 2)), CAST(16918.93 AS Decimal(10, 2)), CAST(660.00 AS Decimal(10, 2)), CAST(22256.37 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(4660.00 AS Decimal(12, 2)), CAST(660.00 AS Decimal(12, 2)), CAST(16918.93 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 28)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (33, N'2010-12-01', N'01122010', CAST(3353.00 AS Decimal(8, 2)), CAST(25704.00 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(4940.00 AS Decimal(10, 2)), CAST(16738.13 AS Decimal(10, 2)), CAST(673.00 AS Decimal(10, 2)), CAST(22351.00 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(4940.00 AS Decimal(12, 2)), CAST(673.00 AS Decimal(12, 2)), CAST(16738.13 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 29)
INSERT INTO [dbo].[InvoiceSummaries] ([InvoiceId], [InvoiceDate], [InvoiceNumber], [GstTotal], [InvoiceTotal], [AccountNumber], [CustomerNumber], [SiteId], [NetworkChargesTotal], [EnergyChargesTotal], [MiscChargesTotal], [TotalCharges], [GSTCharges], [TotalNetworkCharges], [TotalMiscCharges], [TotalEnergyCharges], [ConnectionNumber], [SiteName], [EnergyPointId], [InvoiceSummaryId]) VALUES (34, N'2011-01-01', N'01012011', CAST(3483.11 AS Decimal(8, 2)), CAST(26703.83 AS Decimal(10, 2)), N'Unknown', N'Unknown', 2, CAST(5000.00 AS Decimal(10, 2)), CAST(17503.04 AS Decimal(10, 2)), CAST(700.00 AS Decimal(10, 2)), CAST(23220.72 AS Decimal(8, 2)), CAST(0.00 AS Decimal(8, 2)), CAST(5000.00 AS Decimal(12, 2)), CAST(700.00 AS Decimal(12, 2)), CAST(17503.04 AS Decimal(14, 2)), N'0000103216TR397', N'Pak ''n Save Upper Hutt', 2, 30)
SET IDENTITY_INSERT [dbo].[InvoiceSummaries] OFF

GO


GO

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[MessageFormats] WITH CHECK CHECK CONSTRAINT [FK_MessageFormats_MessageTypes];

ALTER TABLE [dbo].[PortalMessages] WITH CHECK CHECK CONSTRAINT [FK_PortalMessages_Customers];

ALTER TABLE [dbo].[PortalMessages] WITH CHECK CHECK CONSTRAINT [FK_PortalMessages_MessageTypes];


GO
PRINT N'Update complete.';


GO
