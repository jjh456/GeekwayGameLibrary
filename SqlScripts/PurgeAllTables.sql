declare @attendeeIdsToKeep table (id int);

USE Library
insert @attendeeIdsToKeep(id) values(400),(401);

--USE Test
--insert @attendeeIdsToKeep(id) values(2980),(2981);

-- Query ids of checkouts and copies to be deleted
--DECLARE @checkoutIdsToDelete table (id int);
--INSERT @checkoutIdsToDelete 
--	SELECT c.ID
--		FROM [dbo].[Checkouts] c
--		LEFT JOIN Copies on Copy_ID = Copies.ID
--		LEFT JOIN CopyCollections cc on cc.ID = Copies.CopyCollectionID
--		WHERE cc.[Name] = 'Play and Win'

--DECLARE @copyIdsToDelete table (id int);
--INSERT @copyIdsToDelete
--	SELECT c.[ID]
--		FROM [dbo].[Copies] c
--		LEFT JOIN dbo.CopyCollections cc ON cc.ID = c.CopyCollectionID
--		WHERE cc.[Name] = 'Play and Win'

---- Clear out foreign keys on checkouts
UPDATE dbo.Checkouts SET Copy_ID = null
--SELECT * FROM dbo.Checkouts
	WHERE Attendee_ID not in (select id from @attendeeIdsToKeep)

UPDATE dbo.Checkouts SET Attendee_ID = null
--SELECT * FROM dbo.Checkouts
	WHERE Attendee_ID not in (select id from @attendeeIdsToKeep)

------ Clear out foreign keys on copies
--UPDATE dbo.Copies SET CurrentCheckout_ID = null 
----SELECT * FROM dbo.Copies
--	WHERE ID in (select id from @copyIdsToDelete)

---- Delete most of the things
DELETE FROM [dbo].Ratings
DELETE FROM [dbo].Players
DELETE FROM [dbo].Plays
DELETE FROM [dbo].[Checkouts] WHERE Copy_ID is null
DELETE FROM [dbo].Attendees WHERE ID not in (select id from @attendeeIdsToKeep)
--DELETE FROM [dbo].Copies WHERE ID in (select id from @copyIdsToDelete)

-- Determine which games have 0 copies, delete them
--declare @copyCounts table (gameId int, gameTitle nvarchar(max), copies int)

--INSERT @copyCounts 
--	SELECT g.[ID] GameId
--		,g.Title
--		,COUNT(c.ID) NumberOfCopies
--	FROM [dbo].[Games] g
--	LEFT JOIN dbo.Copies c on c.GameID = g.ID
--	GROUP BY g.ID, g.Title

--declare @gameIdsToDelete table (id int)

--INSERT @gameIdsToDelete 
--	SELECT gameId FROM @copyCounts
--	WHERE copies = 0

----SELECT * FROM dbo.Games
--DELETE FROM [dbo].Games 
--	WHERE ID in (select id from @gameIdsToDelete)