--USE GeekwayLibrary
USE Library
--USE Test

DECLARE @deleteCopyLibIds table (Id nvarchar(max))
INSERT INTO @deleteCopyLibIds 
	VALUES ('1399'), ('1974')

DELETE r
FROM dbo.Ratings r
INNER JOIN dbo.Players player on player.ID = r.ID
INNER JOIN dbo.Plays play on play.ID = player.Play_ID
INNER JOIN dbo.Checkouts ch on ch.ID = play.ID
INNER JOIN dbo.Copies c on c.ID = ch.Copy_ID
WHERE c.LibraryID in (SELECT Id from @deleteCopyLibIds)

DELETE player
FROM dbo.Players player
INNER JOIN dbo.Plays play on play.ID = player.Play_ID
INNER JOIN dbo.Checkouts ch on ch.ID = play.ID
INNER JOIN dbo.Copies c on c.ID = ch.Copy_ID
WHERE c.LibraryID in (SELECT Id from @deleteCopyLibIds)

DELETE play
FROM dbo.Plays play
INNER JOIN dbo.Checkouts ch on ch.ID = play.ID
INNER JOIN dbo.Copies c on c.ID = ch.Copy_ID
WHERE c.LibraryID in (SELECT Id from @deleteCopyLibIds)

UPDATE c
SET c.CurrentCheckout_ID = NULL
FROM dbo.Copies c
WHERE c.LibraryID in (SELECT Id from @deleteCopyLibIds)

DELETE ch
FROM dbo.Checkouts ch
INNER JOIN dbo.Copies c on c.ID = ch.Copy_ID
WHERE c.LibraryID in (SELECT Id from @deleteCopyLibIds)

DELETE c
FROM dbo.Copies c
WHERE c.LibraryID in (SELECT Id from @deleteCopyLibIds)