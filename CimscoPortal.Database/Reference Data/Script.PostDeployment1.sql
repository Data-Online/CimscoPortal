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
:r ".\Delete.data.sql"
go
:r ".\MessageCategories.data.sql"
go
:r ".\MessageTypes.data.sql"
go
:r ".\Customers.data.sql"
go
:r ".\PortalMessages.data.sql"
go
:r ".\InvoiceSummaries.data.sql"
go


