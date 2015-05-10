CREATE TABLE [dbo].[MessageFormats](
	[MessageFormatId] [int] IDENTITY(1,1) NOT NULL,
	[MessageTypeId] [int] NULL,
	[Element1] [nvarchar](50) NULL,
	[Element2] [nvarchar](50) NULL,
	[DisplayFormat] [nvarchar](50) NULL,
 CONSTRAINT [PK__MessageT__9BA1E2BAAD5F02E6] PRIMARY KEY CLUSTERED 
(
	[MessageFormatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
) 

GO

ALTER TABLE [dbo].[MessageFormats]  WITH CHECK ADD  CONSTRAINT [FK_MessageFormats_MessageTypes] FOREIGN KEY([MessageTypeId])
REFERENCES [dbo].[MessageTypes] ([MessageTypeId])
GO

ALTER TABLE [dbo].[MessageFormats] CHECK CONSTRAINT [FK_MessageFormats_MessageTypes]
GO