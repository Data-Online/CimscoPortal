CREATE TABLE [dbo].[MessageTypes]
(
	[MessageTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TypeName] NVARCHAR(10) NULL, 
    [TypeElement] NVARCHAR(20) NULL
)
