--UPDATE Library.dbo.Checkouts SET Attendee_ID = null WHERE Attendee_ID not in (400, 401)
--UPDATE Library.dbo.Checkouts SET Copy_ID = null WHERE Attendee_ID not in (400, 401)

--DELETE FROM [Library].[dbo].[Checkouts] WHERE Attendee_ID not in (400, 401)
--DELETE FROM [Library].[dbo].Attendees WHERE BadgeID Like 'GW%'

--UPDATE PlayAndWin.dbo.Checkouts SET Attendee_ID = null
--UPDATE PlayAndWin.dbo.Copies SET CurrentCheckout_ID = null
--UPDATE PlayAndWin.dbo.Checkouts SET Copy_ID = null

--DELETE FROM [PlayAndWin].[dbo].Ratings
--DELETE FROM [PlayAndWin].[dbo].Players
--DELETE FROM [PlayAndWin].[dbo].Plays
--DELETE FROM [PlayAndWin].[dbo].[Checkouts]
--DELETE FROM [PlayAndWin].[dbo].Attendees
--DELETE FROM [PlayAndWin].[dbo].Copies
--DELETE FROM [PlayAndWin].[dbo].Games