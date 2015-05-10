CREATE TABLE [dbo].[PortalMessages](
	[PortalMessageId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[Message] [nvarchar](100) NULL,
	[MessageFormatId] [int] NULL,
	[TimeStamp] [datetime] NULL,
	[Footer] [nvarchar](100) NULL,
	[ExpiryDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PortalMessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
)

GO

ALTER TABLE [dbo].[PortalMessages]  WITH CHECK ADD  CONSTRAINT [FK_PortalMessages_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
GO

ALTER TABLE [dbo].[PortalMessages] CHECK CONSTRAINT [FK_PortalMessages_Customers]
GO

ALTER TABLE [dbo].[PortalMessages]  WITH CHECK ADD  CONSTRAINT [FK_PortalMessages_MessageTypes] FOREIGN KEY([MessageFormatId])
REFERENCES [dbo].[MessageFormats] ([MessageFormatId])
GO

ALTER TABLE [dbo].[PortalMessages] CHECK CONSTRAINT [FK_PortalMessages_MessageTypes]
GO


