--USE Test
USE Library

DECLARE @badgeIdsToKeep table (Id nvarchar(max))
INSERT INTO @badgeIdsToKeep 
	VALUES ('8675309'), ('99123')

DELETE r
--SELECT *
FROM dbo.Ratings r
INNER JOIN dbo.Players player on player.ID = r.ID
INNER JOIN dbo.Plays play on play.ID = player.Play_ID
INNER JOIN dbo.Checkouts ch on ch.ID = play.ID
INNER JOIN dbo.Attendees a on a.ID = ch.Attendee_ID
WHERE a.BadgeID NOT in (SELECT Id from @badgeIdsToKeep)

DELETE player
--SELECT *
FROM dbo.Players player
INNER JOIN dbo.Plays play on play.ID = player.Play_ID
INNER JOIN dbo.Checkouts ch on ch.ID = play.ID
INNER JOIN dbo.Attendees a on a.ID = ch.Attendee_ID
WHERE a.BadgeID NOT in (SELECT Id from @badgeIdsToKeep)

DELETE play
--SELECT *
FROM dbo.Plays play
INNER JOIN dbo.Checkouts ch on ch.ID = play.ID
INNER JOIN dbo.Attendees a on a.ID = ch.Attendee_ID
WHERE a.BadgeID NOT in (SELECT Id from @badgeIdsToKeep)

UPDATE cop
SET cop.CurrentCheckout_ID = NULL
--SELECT *
FROM dbo.Copies cop
INNER JOIN dbo.Checkouts ch on ch.Copy_ID = cop.ID
INNER JOIN dbo.Attendees a on a.ID = ch.Attendee_ID
WHERE a.BadgeID NOT in (SELECT Id from @badgeIdsToKeep)

DELETE ch
--SELECT *
FROM dbo.Checkouts ch
INNER JOIN dbo.Attendees a on a.ID = ch.Attendee_ID
WHERE a.BadgeID NOT in (SELECT Id from @badgeIdsToKeep)

DELETE a
--SELECT *
FROM dbo.Attendees a
WHERE a.BadgeID NOT in (SELECT Id from @badgeIdsToKeep)