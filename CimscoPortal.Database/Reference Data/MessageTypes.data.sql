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