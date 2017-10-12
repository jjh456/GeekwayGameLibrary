USE GeekwayLibrary

SELECT Attendees.BadgeID
      ,[TimeOut]
	  ,Copies.LibraryID as CopyID
      ,TimeIn

  FROM [GeekwayLibrary].[dbo].[Checkouts]
  LEFT JOIN Attendees on Attendee_ID = Attendees.ID
  LEFT JOIN Copies on Copy_ID = Copies.ID