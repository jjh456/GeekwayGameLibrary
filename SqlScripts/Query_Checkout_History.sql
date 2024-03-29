USE [Library]

SELECT BadgeID
	  ,att.[Name] as AttendeeName
	  ,game.Title as Game
	  ,cop.LibraryID as CopyID
	  ,DATEDIFF(MINUTE, [TimeOut], [TimeIn]) CheckoutMinutes
      ,[TimeOut]
      ,TimeIn
FROM [dbo].[Checkouts] chout
LEFT JOIN Attendees att on att.ID = chout.Attendee_ID
LEFT JOIN Copies cop on cop.ID = chout.Copy_ID
LEFT JOIN Games game on game.ID = cop.GameID

WHERE [TimeOut] BETWEEN '2021-10-04 18:00:00' AND '2021-10-7 18:00:00'