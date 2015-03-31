CREATE TABLE [dbo].[MessageCategories]
(
	[MessageCategoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CategoryName] NVARCHAR(20) NULL, 
    [Element1] NVARCHAR(50) NULL, 
    [Element2] NVARCHAR(50) NULL
)
