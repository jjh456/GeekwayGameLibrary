-- UPDATE [Library].[dbo].[Copies] SET Winnable=0
SELECT * FROM [Library].[dbo].[Copies]
WHERE CopyCollectionID in (1,2,3)
AND Winnable = 1