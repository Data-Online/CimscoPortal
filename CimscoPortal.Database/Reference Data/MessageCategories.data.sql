SET IDENTITY_INSERT [dbo].[MessageCategories] ON
INSERT INTO [dbo].[MessageCategories] ([MessageCategoryId], [CategoryName], [Element1], [Element2]) VALUES (2, N'alert-phone-blue', N'fa fa-phone bg-themeprimary white', N'fa fa-clock-o themeprimary')
INSERT INTO [dbo].[MessageCategories] ([MessageCategoryId], [CategoryName], [Element1], [Element2]) VALUES (4, N'alert-tick-red', N'fa fa-check bg-darkorange white', N'fa fa-clock-o darkorange')
INSERT INTO [dbo].[MessageCategories] ([MessageCategoryId], [CategoryName], [Element1], [Element2]) VALUES (5, N'alert-event-orange', N'fa fa-gift bg-warning white', N'fa fa-calendar warning')
SET IDENTITY_INSERT [dbo].[MessageCategories] OFF
