/****** Script for SelectTopNRows command from SSMS  ******/
use CimscoNZ
go

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