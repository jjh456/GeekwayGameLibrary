USE GeekwayLibrary

SELECT Title GameTitle
	  ,Copies.LibraryID
	  ,Copies.OwnerName
  FROM [GeekwayLibrary].[dbo].[Games] g
  LEFT JOIN Copies on g.ID = Copies.GameID

SELECT Name
      ,BadgeID
  FROM [GeekwayLibrary].[dbo].[Attendees]


SELECT Attendees.BadgeID
      ,[TimeOut]
	  ,Copies.LibraryID as CopyID
      ,TimeIn

  FROM [GeekwayLibrary].[dbo].[Checkouts]
  LEFT JOIN Attendees on Attendee_ID = Attendees.ID
  LEFT JOIN Copies on Copy_ID = Copies.ID