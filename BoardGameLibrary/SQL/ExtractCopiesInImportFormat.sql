SELECT Games.Title, Copies.LibraryID, Copies.OwnerName
  FROM [GeekwayLibrary].[dbo].Copies Copies
  JOIN GeekwayLibrary.dbo.Games Games on Games.ID = Copies.GameID