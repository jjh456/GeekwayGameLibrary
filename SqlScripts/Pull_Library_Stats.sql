USE Library

SELECT Title GameTitle
	  ,Copies.LibraryID
	  ,Copies.OwnerName
  FROM [Library].[dbo].[Games] g
  LEFT JOIN Copies on g.ID = Copies.GameID

--SELECT Name
--      ,BadgeID
--  FROM [Library].[dbo].[Attendees]


SELECT Attendees.BadgeID
      ,[TimeOut]
	  ,Copies.LibraryID as CopyID
      ,TimeIn

  FROM [Library].[dbo].[Checkouts]
  LEFT JOIN Attendees on Attendee_ID = Attendees.ID
  LEFT JOIN Copies on Copy_ID = Copies.ID