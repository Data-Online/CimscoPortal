CREATE TABLE [dbo].[MessageTypes](
	[MessageTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](20) NULL,
	[PageElement] [nvarchar](20) NULL,
 CONSTRAINT [PK_MessageTypes] PRIMARY KEY CLUSTERED 
(
	[MessageTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

