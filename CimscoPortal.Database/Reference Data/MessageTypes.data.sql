SET IDENTITY_INSERT [dbo].[MessageTypes] ON
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [TypeName], [TypeElement]) VALUES (1, N'Alert', N'pg-alert')
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [TypeName], [TypeElement]) VALUES (2, N'Mail', N'pg-mail')
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [TypeName], [TypeElement]) VALUES (3, N'Task', N'pg-task')
INSERT INTO [dbo].[MessageTypes] ([MessageTypeId], [TypeName], [TypeElement]) VALUES (4, N'Chat', N'pg-chat')
SET IDENTITY_INSERT [dbo].[MessageTypes] OFF
