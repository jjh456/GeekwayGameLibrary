--USE GeekwayLibrary
USE Library

-- Work way up from the bottom of dependencies like you
-- originally suggested. Delete in order of joins: rating, player, play, etc
SELECT * -- To delete instead: DELETE r
FROM dbo.Ratings r
INNER JOIN dbo.Players player on player.ID = r.ID
INNER JOIN dbo.Plays play on play.ID = player.Play_ID
INNER JOIN dbo.Checkouts ch on ch.ID = play.ID
INNER JOIN dbo.Copies c on c.ID = ch.Copy_ID
WHERE c.LibraryID in ('1399', '1974')