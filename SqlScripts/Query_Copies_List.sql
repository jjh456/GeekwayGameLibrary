USE GeekwayLibrary

SELECT Title GameTitle
	  ,Copies.LibraryID
	  ,Copies.OwnerName
  FROM [GeekwayLibrary].[dbo].[Games] g
  LEFT JOIN Copies on g.ID = Copies.GameID
