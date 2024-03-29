USE Library
--USE Test

declare @attendeeIdsToKeep table (id int);
insert @attendeeIdsToKeep(id) values(400),(401);
--insert @attendeeIdsToKeep(id) values(2980),(2981);


---- Clear out foreign keys on checkouts
-- TODO: Look up these checkouts first, capture their ids, 
-- clear them from any copies that still have them as the current checkout
--UPDATE dbo.Checkouts SET Copy_ID = null
--	WHERE Attendee_ID not in (select id from @attendeeIdsToKeep)

--UPDATE dbo.Checkouts SET Attendee_ID = null
--	WHERE Attendee_ID not in (select id from @attendeeIdsToKeep)

---- Delete the non-persistent things
--DELETE FROM [dbo].Ratings
--DELETE FROM [dbo].Players
--DELETE FROM [dbo].Plays
--DELETE FROM [dbo].[Checkouts] WHERE Copy_ID is null 
DELETE FROM [dbo].Attendees WHERE ID not in (select id from @attendeeIdsToKeep)