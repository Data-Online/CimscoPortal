CREATE TABLE [dbo].[PortalMessages]
(
	[PortalMessageId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomerId] INT NULL, 
    [MessageCategoryId] INT NULL, 
    [Message] NVARCHAR(100) NULL, 
    [MessageTypeId] INT NULL, 
    [MessageFormatId] INT NULL, 
    [TimeStamp] DATETIME NULL, 
    [Footer] NVARCHAR(100) NULL, 
    [ExpiryDate] DATETIME NULL, 
    CONSTRAINT [FK_PortalMessages_MessageCategories] FOREIGN KEY ([MessageCategoryId]) REFERENCES [MessageCategories]([MessageCategoryId]), 
    CONSTRAINT [FK_PortalMessages_MessageTypes] FOREIGN KEY ([MessageTypeId]) REFERENCES [MessageTypes]([MessageTypeId]), 
    CONSTRAINT [FK_PortalMessages_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([CustomerId]) 
)
