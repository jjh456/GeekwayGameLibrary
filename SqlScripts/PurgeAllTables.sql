UPDATE Library.dbo.Checkouts SET Attendee_ID = null
UPDATE Library.dbo.Copies SET CurrentCheckout_ID = null
UPDATE Library.dbo.Checkouts SET Copy_ID = null

DELETE FROM [Library].[dbo].Attendees
DELETE FROM [Library].[dbo].[Checkouts]
DELETE FROM [Library].[dbo].Ratings
DELETE FROM [Library].[dbo].Players
DELETE FROM [Library].[dbo].Plays
DELETE FROM [Library].[dbo].Copies
DELETE FROM [Library].[dbo].Games