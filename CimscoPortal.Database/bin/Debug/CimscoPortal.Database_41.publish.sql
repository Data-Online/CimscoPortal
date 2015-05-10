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
The column [dbo].[MessageTypes].[TypeElement] is being dropped, data loss could occur.

The column [dbo].[MessageTypes].[TypeName] is being dropped, data loss could occur.
*/

IF EXISTS (select top 1 1 from [dbo].[MessageTypes])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
/*
The column [dbo].[PortalMessages].[MessageCategoryId] is being dropped, data loss could occur.

The column [dbo].[PortalMessages].[MessageTypeId] is being dropped, data loss could occur.
*/

IF EXISTS (select top 1 1 from [dbo].[PortalMessages])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
PRINT N'Dropping [dbo].[FK_PortalMessages_MessageTypes]...';


GO
ALTER TABLE [dbo].[PortalMessages] DROP CONSTRAINT [FK_PortalMessages_MessageTypes];


GO
PRINT N'Dropping [dbo].[FK_PortalMessages_Customers]...';


GO
ALTER TABLE [dbo].[PortalMessages] DROP CONSTRAINT [FK_PortalMessages_Customers];


GO
PRINT N'Dropping [dbo].[FK_PortalMessages_MessageCategories]...';


GO
ALTER TABLE [dbo].[PortalMessages] DROP CONSTRAINT [FK_PortalMessages_MessageCategories];


GO
PRINT N'Starting rebuilding table [dbo].[MessageTypes]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_MessageTypes] (
    [MessageTypeId]  INT           IDENTITY (1, 1) NOT NULL,
    [MessageType]    NVARCHAR (20) NULL,
    [MessageElement] NVARCHAR (20) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_MessageTypes] PRIMARY KEY CLUSTERED ([MessageTypeId] ASC) ON [PRIMARY]
) ON [PRIMARY];

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[MessageTypes])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_MessageTypes] ON;
        INSERT INTO [dbo].[tmp_ms_xx_MessageTypes] ([MessageTypeId])
        SELECT   [MessageTypeId]
        FROM     [dbo].[MessageTypes]
        ORDER BY [MessageTypeId] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_MessageTypes] OFF;
    END

DROP TABLE [dbo].[MessageTypes];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_MessageTypes]', N'MessageTypes';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_MessageTypes]', N'PK_MessageTypes', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Altering [dbo].[PortalMessages]...';


GO
ALTER TABLE [dbo].[PortalMessages] DROP COLUMN [MessageCategoryId], COLUMN [MessageTypeId];


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
PRINT N'Creating [dbo].[FK_PortalMessages_MessageTypes]...';


GO
ALTER TABLE [dbo].[PortalMessages] WITH NOCHECK
    ADD CONSTRAINT [FK_PortalMessages_MessageTypes] FOREIGN KEY ([MessageFormatId]) REFERENCES [dbo].[MessageFormats] ([MessageFormatId]);


GO
PRINT N'Creating [dbo].[FK_MessageFormats_MessageTypes]...';


GO
ALTER TABLE [dbo].[MessageFormats] WITH NOCHECK
    ADD CONSTRAINT [FK_MessageFormats_MessageTypes] FOREIGN KEY ([MessageTypeId]) REFERENCES [dbo].[MessageTypes] ([MessageTypeId]);


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
--:r ".\MessageCategories.data.sql"
GO
--:r ".\MessageTypes.data.sql"
GO
delete from [dbo].[Customers] 
SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT INTO [dbo].[Customers] ([CustomerId], [CustomerName]) VALUES (1, N'Customer1')
SET IDENTITY_INSERT [dbo].[Customers] OFF

GO
--:r ".\PortalMessages.data.sql"
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
ALTER TABLE [dbo].[PortalMessages] WITH CHECK CHECK CONSTRAINT [FK_PortalMessages_MessageTypes];

ALTER TABLE [dbo].[MessageFormats] WITH CHECK CHECK CONSTRAINT [FK_MessageFormats_MessageTypes];


GO
PRINT N'Update complete.';


GO
