SET IDENTITY_INSERT [dbo].[MessageFormats] ON 
GO
INSERT [dbo].[MessageFormats] ([MessageFormatId], [MessageTypeId], [Element1], [Element2], [DisplayFormat]) VALUES (1, 1, N'fa fa-phone bg-themeprimary white', N'fa fa-clock-o themeprimary', N'Phone Blue')
INSERT [dbo].[MessageFormats] ([MessageFormatId], [MessageTypeId], [Element1], [Element2], [DisplayFormat]) VALUES (2, 1, N'fa fa-check bg-darkorange white', N'fa fa-clock-o darkorange', N'Tick Red')
INSERT [dbo].[MessageFormats] ([MessageFormatId], [MessageTypeId], [Element1], [Element2], [DisplayFormat]) VALUES (3, 1, N'fa fa-gift bg-warning white', N'fa fa-gift bg-warning white', N'Event Orange')
INSERT [dbo].[MessageFormats] ([MessageFormatId], [MessageTypeId], [Element1], [Element2], [DisplayFormat]) VALUES (4, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[MessageFormats] OFF
GO