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
